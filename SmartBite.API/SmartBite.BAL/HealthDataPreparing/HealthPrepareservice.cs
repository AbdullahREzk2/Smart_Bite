using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using SmartBite.BAL.Authantication;
using SmartBite.BAL.DTOS;
using SmartBite.DAL.Authantication;
using SmartBite.DAL.DiseaseNutrient;
using SmartBite.DAL.HealthProfile;
using SmartBite.DAL.Models;

namespace SmartBite.BAL.HealthDataPreparing
{
    public class HealthPrepareservice: IHealthPrepareservice
    {
        private readonly IMapper _mapper;
        private readonly IHealthProfileService _healthProfile;
        private readonly AiPredictionNutritionService _aiPredictionNutritionService;
        private readonly IDiseaseNutrientService _diseaseNutrientService;

        public HealthPrepareservice(IMapper mapper,IHealthProfileService healthProfile,AiPredictionNutritionService aiPredictionNutritionService,IDiseaseNutrientService diseaseNutrientService )
        {
            _mapper = mapper;
            _healthProfile = healthProfile;
            _aiPredictionNutritionService = aiPredictionNutritionService;
            _diseaseNutrientService = diseaseNutrientService;
        }


        private BinaryDisesesDTO ConvertDiseseNameToBinary(string diseses, int TakeInsolin = 0)
        {
            BinaryDisesesDTO binaryDiseses = new BinaryDisesesDTO();

            switch (diseses.Trim().ToLower())
            {
                case "Hypertension":
                    binaryDiseses.Haypertension = 1;
                    binaryDiseses.Obisty = 0;
                    binaryDiseses.Diabites = 0;
                    binaryDiseses.heart = 0;
                    binaryDiseses.Insolin = 0;
                    break;

                case "Diabetes":
                    binaryDiseses.Haypertension = 0;
                    binaryDiseses.Obisty = 0;
                    binaryDiseses.Diabites = 1;
                    binaryDiseses.heart = 0;
                    binaryDiseses.Insolin = TakeInsolin;
                    break;

                case "Heart":
                    binaryDiseses.Haypertension = 0;
                    binaryDiseses.Obisty = 0;
                    binaryDiseses.Diabites = 0;
                    binaryDiseses.heart = 1;
                    binaryDiseses.Insolin = 0;
                    break;

                case "Obesity":
                    binaryDiseses.Haypertension = 0;
                    binaryDiseses.Obisty = 1;
                    binaryDiseses.Diabites = 0;
                    binaryDiseses.heart = 0;
                    binaryDiseses.Insolin = 0;
                    break;

                default:
                    // All flags remain 0
                    break;
            }

            return binaryDiseses;
        }

        private int ConvertGenderToInt(string Gender)
        {
            return Gender == "Male" ? 1 : 0;
        }

        private int ConvertActivityLevelToInt(string ActivityLevel)
        {
            switch (ActivityLevel)
            {
                case "sedentary":
                    return 1;
                case "lightly active":
                    return 2;
                case "moderate":
                    return 3;
                case "very active":
                    return 4;
                case "super active":
                    return 5;
                default:
                    return 0;
            }
        }

        private MyAiObjectDTO PrepareUserData(int UserID , int hasAllergy , DiseseObjectDTO diseseObject)
        {

            MyAiObjectDTO myAiObject = new MyAiObjectDTO();
            
            var HealthData = _healthProfile.GetUserHealth(UserID); 

            myAiObject.Age= HealthData.Age;
            myAiObject.Gender = HealthData.Gender;
            myAiObject.Weight = HealthData.Weight;
            myAiObject.Height = HealthData.Height;
            myAiObject.ActivityLevel = HealthData.ActivityLevel;
            myAiObject.BMI = (float)HealthData.BMI;
            myAiObject.BMR = (float)HealthData.BMR;
            myAiObject.Allergy = hasAllergy;
            myAiObject .DiseaseName = diseseObject.DiseaseName;
            myAiObject.Insolin = diseseObject.TakeInsolin ? 1 : 0;

            return myAiObject;


        }

        private AiObjectDTO PerpareAIObjectInput(MyAiObjectDTO OldObject)
        {
            BinaryDisesesDTO binaryDiseses = ConvertDiseseNameToBinary(OldObject.DiseaseName!);

            AiObjectDTO NewObject = new AiObjectDTO
            {
                Age = (float)OldObject.Age,
                Weight = (float)OldObject.Weight,
                Height = (float)OldObject.Height,
                Gender = ConvertGenderToInt(OldObject.Gender!),
                ActivityLevel = ConvertActivityLevelToInt(OldObject.ActivityLevel!),
                BMR = OldObject.BMR,
                BMI = OldObject.BMI,
                Insolin = binaryDiseses.Insolin,
                Allergy = OldObject.Allergy,
                Haypertension = binaryDiseses.Haypertension,
                Diabites = binaryDiseses.Diabites,
                Obisty = binaryDiseses.Obisty,
                Heart = binaryDiseses.heart
            };

            return NewObject;
        }

        // La Finaaaaaaaaaaaaaaaaaaaaaal Input -->
        private AiObjectDTO FinalModelInput(int UserID, int hasAllergy, DiseseObjectDTO diseseObject)
        {
            return PerpareAIObjectInput(PrepareUserData(UserID, hasAllergy, diseseObject));
        }

        
        // Call The Model & Save The Nutrition Model Output -->
        public async void CallLimitsModel(int UserID, int hasAllergy, DiseseObjectDTO diseseObject)
        {
            var Data = FinalModelInput(UserID, hasAllergy, diseseObject);
            var Limits =  await _aiPredictionNutritionService.PredictNutrientsAsync(Data);
            
            // Add Disease Nutrient Limits in DiseaseNutrientLimits Table -->
            _diseaseNutrientService.AddDiseaseNutrientLimitsForUser(UserID, _mapper.Map<List<stNutrientLimitModel>>(Limits));

            //Add Disease Nutrient Limits To DailyLimits Table -->
            _diseaseNutrientService.AddDiseaseNutrientLimitsToVariable(UserID, _mapper.Map<List<stNutrientLimitModel>>(Limits));

        }

        // Call the Model to update --> 
        public async void CallLimitsModelForUpdate(int UserID, MyAiObjectDTO myAiObject)
        {
            var Data = PerpareAIObjectInput(myAiObject);
            var Limits = await _aiPredictionNutritionService.PredictNutrientsAsync(Data);

            // Update Disease Nutrient Limits in DiseaseNutrientLimits Table -->
            _diseaseNutrientService.UpdateDiseaseNutrientLimitForUser(UserID, _mapper.Map<List<stNutrientLimitModel>>(Limits));

            // Update Disease Nutrient Limits To DailyLimits Table -->
            _diseaseNutrientService.UpdateDiseaseNutrientLimitsToVariable(UserID, _mapper.Map<List<stNutrientLimitModel>>(Limits));

        }


    }
}
