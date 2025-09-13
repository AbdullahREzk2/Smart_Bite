using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using SmartBite.BAL.CalorieCalculator;
using SmartBite.BAL.DTOS;
using SmartBite.BAL.HealthDataPreparing;
using SmartBite.DAL.Allergy;
using SmartBite.DAL.Authantication;
using SmartBite.DAL.Calories;
using SmartBite.DAL.Disease;
using SmartBite.DAL.DiseaseNutrient;
using SmartBite.DAL.HealthProfile;
using SmartBite.DAL.Models;
using static System.Runtime.InteropServices.JavaScript.JSType;


namespace SmartBite.BAL.HealthProfileOperations
{
    public class HealthProfileManager : IHealthProfileManager
    {
        private readonly IMapper _mapper;
        private readonly IAuthanticationService _authantication;
        private readonly IAllergyService _allergyService;
        private readonly IDiseaseService _diseaseService;
        private readonly IHealthPrepareservice _healthPrepareservice;
        private readonly IDiseaseNutrientService _diseaseNutrientService;
        private readonly ICaloriesService _caloriesService;
        private readonly IHealthProfileService _healthProfileService;
        private readonly ICalorieCalculatorManager _calculatorManager;

        public HealthProfileManager(IMapper mapper ,IAuthanticationService authantication,IAllergyService allergyService, IDiseaseService diseaseService, IHealthPrepareservice healthPrepareservice,IDiseaseNutrientService diseaseNutrientService,ICaloriesService caloriesService ,IHealthProfileService healthProfileService ,ICalorieCalculatorManager calculatorManager)
        {
            _mapper = mapper;
            _authantication = authantication;
            _allergyService = allergyService;
            _diseaseService = diseaseService;
            _healthPrepareservice = healthPrepareservice;
            _diseaseNutrientService = diseaseNutrientService;
            _caloriesService = caloriesService;
            _healthProfileService = healthProfileService;
            _calculatorManager = calculatorManager;
        }


        public bool AddUserDiseaseandAllergies(int UserID,DiseaseandAllergyiesDataDTO data)
        {
            int hasAllrgies; // Check if the user has Allergy 

            // User has allergy & disease -->
            if (data.Allergies != null && data.diseseObject != null)   
            {
               bool isAllergiesAdded = _allergyService.AddUserAllergies(UserID, data.Allergies);
               bool isDiseaseAdded = _diseaseService.AddUserDisease(UserID,data.diseseObject.DiseaseName!);

                hasAllrgies = 1;

                if (data.diseseObject.DiseaseName == "Diabetes")
                {
                    _diseaseService.UpdateUserInsolin(UserID,Convert.ToInt32(data.diseseObject.TakeInsolin));
                }

                _healthPrepareservice.CallLimitsModel(UserID, hasAllrgies, data.diseseObject);

                return true;
            }
            // User has disease Only -->
            else if (data.Allergies == null && data.diseseObject != null)     
            {
                bool isDiseaseAdded = _diseaseService.AddUserDisease(UserID, data.diseseObject.DiseaseName!);
                
                hasAllrgies = 0; //User Don't has Allergy 

                if (data.diseseObject.DiseaseName == "Diabetes")
                {
                    _diseaseService.UpdateUserInsolin(UserID, Convert.ToInt32(data.diseseObject.TakeInsolin));
                }

                _healthPrepareservice.CallLimitsModel(UserID, hasAllrgies, data.diseseObject);

                return true;
            }
            // User has Allergy Only -->
            else if (data.Allergies != null && data.diseseObject == null)         
            {
                bool isAllergiesAdded = _allergyService.AddUserAllergies(UserID, data.Allergies);
                hasAllrgies = 1;

                return true;
            }
            // User don't have allergy and disease
            else if (data.Allergies == null && data.diseseObject == null)            
            {
                hasAllrgies = 0;

                return true;
            }

            return false;
        }

        public bool AddUserHealthData(HealthDataDTO data)
        {
            bool updated = _healthProfileService.AddUserHealthData(data.UserID, data.Age, data.Weight, data.Height, data.Gender!, data.ActivityLevel!);

            if (!updated)
                return false;

            var CalData = _calculatorManager.CalculateDailyCalories(data);

            _caloriesService.UpdatedailyCalories(data.UserID, CalData.DailyCalories);
            _caloriesService.AddDailyCalorieToVariable(data.UserID, CalData.DailyCalories);
            _caloriesService.AddBMIandBMR(data.UserID, CalData.BmI, CalData.Bmr);

            bool Added = AddUserDiseaseandAllergies(data.UserID, data.Input!);

            if (!Added)
                return false;

            return true;
        }

        private List<NutrientLimitModel> GetDefaultNutrientLimits()
        {
            return new List<NutrientLimitModel>
          {
            new NutrientLimitModel { NutrientName = "Carbs", LimitPerDay = 0, NutrientUnit = "g" },
            new NutrientLimitModel { NutrientName = "Protien", LimitPerDay = 0, NutrientUnit = "g" },
            new NutrientLimitModel { NutrientName = "Fat", LimitPerDay = 0, NutrientUnit = "g" },
            new NutrientLimitModel { NutrientName = "Saturated Fat", LimitPerDay = 0, NutrientUnit = "g" },
            new NutrientLimitModel { NutrientName = "Suger", LimitPerDay = 0, NutrientUnit = "g" },
            new NutrientLimitModel { NutrientName = "Sodium", LimitPerDay = 0, NutrientUnit = "mg" },
            new NutrientLimitModel { NutrientName = "Cholecterol", LimitPerDay = 0, NutrientUnit = "mg" },
            new NutrientLimitModel { NutrientName = "Fiber", LimitPerDay = 0, NutrientUnit = "g" }
           };
        }

        public HomeDataDTO GetUserLimitsAndCalories(int UserID)
        {
            HomeDataDTO limitsAndCalories = new HomeDataDTO();

          if (_diseaseService.IsUserHasDisease(UserID) && !_allergyService.IsUserHasAllergies(UserID))
          {

                var Name = _authantication.GetName(UserID);
                var DailyLimits = _diseaseNutrientService.GetDiseaseNutrientLimitsFromVariable(UserID);
                var DailyCalories = _caloriesService.GetUserRemainingDailyCalories(UserID);

                limitsAndCalories.Limits = DailyLimits;
                limitsAndCalories.Calories = DailyCalories;
                limitsAndCalories.Name= Name;

                return limitsAndCalories;
          }

          else if(_allergyService.IsUserHasAllergies(UserID) && !_diseaseService.IsUserHasDisease(UserID))
          {
                var Name = _authantication.GetName(UserID);
                var DailyLimits = GetDefaultNutrientLimits();
                var DailyCalories = _caloriesService.GetUserRemainingDailyCalories(UserID);

                limitsAndCalories.Limits = DailyLimits;
                limitsAndCalories.Calories = DailyCalories;
                limitsAndCalories.Name = Name;

                return limitsAndCalories;
          }

          else if(_allergyService.IsUserHasAllergies(UserID) && _diseaseService.IsUserHasDisease(UserID))
          {
                var Name = _authantication.GetName(UserID);
                var DailyLimits = _diseaseNutrientService.GetDiseaseNutrientLimitsFromVariable(UserID);
                var DailyCalories = _caloriesService.GetUserRemainingDailyCalories(UserID);

                limitsAndCalories.Limits = DailyLimits;
                limitsAndCalories.Calories = DailyCalories;
                limitsAndCalories.Name = Name;

                return limitsAndCalories;
          }

          else
          {
                var Name = _authantication.GetName(UserID);
                var DailyLimits = GetDefaultNutrientLimits();
                var DailyCalories = _caloriesService.GetUserRemainingDailyCalories(UserID);

                limitsAndCalories.Limits = DailyLimits;
                limitsAndCalories.Calories = DailyCalories;
                limitsAndCalories.Name = Name;

                return limitsAndCalories;
          }

            
        }

        public MetersDTO GetUserOriginalLimits(int UserID)
        {
            MetersDTO meters = new MetersDTO();

            if (_diseaseService.IsUserHasDisease(UserID) && !_allergyService.IsUserHasAllergies(UserID))
            {
                
                var DailyLimits = _diseaseNutrientService.GetDiseaseNutrientLimits(UserID);
                var DailyCalories = _caloriesService.GetUserOriginalCalories(UserID);

                meters.Limits = DailyLimits;
                meters.Calories = DailyCalories;
                return meters;

            }

            else if (_allergyService.IsUserHasAllergies(UserID) && !_diseaseService.IsUserHasDisease(UserID))
            {
                var DailyLimits = GetDefaultNutrientLimits();
                var DailyCalories = _caloriesService.GetUserOriginalCalories(UserID);

                meters.Limits = DailyLimits;
                meters.Calories = DailyCalories;
                return meters;
            }

            else if (_allergyService.IsUserHasAllergies(UserID) && _diseaseService.IsUserHasDisease(UserID))
            {
                var DailyLimits = _diseaseNutrientService.GetDiseaseNutrientLimits(UserID);
                var DailyCalories = _caloriesService.GetUserOriginalCalories(UserID);

                meters.Limits = DailyLimits;
                meters.Calories = DailyCalories;
                return meters;
            }

            else
            {
                var DailyLimits = GetDefaultNutrientLimits();
                var DailyCalories = _caloriesService.GetUserOriginalCalories(UserID);

                meters.Limits = DailyLimits;
                meters.Calories = DailyCalories;
                return meters;
            }

        }

        public UserHealthprofileDataDTO GetUserHealthProfileData(int UserID)
        {
            return _mapper.Map<UserHealthprofileDataDTO>(_healthProfileService.GetUserHealthProfileData(UserID));

        }

        public bool UpdateHealthProfile(int UserID, UserHealthprofileDataDTO data)
        {
            // update data 
            bool Updated = _healthProfileService.UpdateProfile(UserID, _mapper.Map<UserHealthprofileDataModel>(data));

            // call calorie calc and give it new heatlh values and  get from it bmi and bmr and calories
            PersonDTO person = new PersonDTO();

            person.Name = data.Name;
            person.Email = data.Email;
            person.Password = data.Password;
            person.Age = data.Age;
            person.Weight = data.Weight;
            person.Height = data.Height;
            person.Gender = data.Gender;
            person.ActivityLevel = data.ActivityLevel;

            var CalData = _calculatorManager.CalculateDailyCalories(person);

            // update calories in daily calories table
            _caloriesService.UpdatedailyCalories(UserID, CalData.DailyCalories);
            _caloriesService.UpdateDailyCaloriesToVarible(UserID, CalData.DailyCalories);

            // update bmi and bmr and calories in users table 
            _caloriesService.AddBMIandBMR(UserID, CalData.BmI, CalData.Bmr);



            // check if user has disease 
            bool hasAllergy = _allergyService.IsUserHasAllergies(UserID);

            int takeAnsolin = 0;


            if (_diseaseService.IsUserHasDisease(UserID))
            {

                MyAiObjectDTO myAiObject = new MyAiObjectDTO();

                if(_diseaseService.GetUserDisease(UserID) == "Diabetes")
                {
                    takeAnsolin = _healthProfileService.GetTakeAnsolin(UserID);
                    
                }

                var BMIandBMR = _caloriesService.GetBMIandBMR(UserID);

                myAiObject.Age= data.Age;
                myAiObject.Weight= data.Weight;
                myAiObject.Height = data.Height;
                myAiObject.Gender = data.Gender;
                myAiObject.ActivityLevel = data.ActivityLevel;
                myAiObject.DiseaseName =_diseaseService.GetUserDisease(UserID);
                myAiObject.BMI =(float)BMIandBMR.BMI;
                myAiObject.BMR = (float)BMIandBMR.BMR;
                myAiObject.Insolin = takeAnsolin;
                myAiObject.Allergy = Convert.ToInt32(hasAllergy);

                // limits model
                _healthPrepareservice.CallLimitsModelForUpdate(UserID, myAiObject);

                
            }



            return true;
           
        }

        public bool IsUserHealthDataCompleted(int UserID)
        {
            return _caloriesService.GetUserOriginalCalories(UserID) == null ? false : true;
        }


        public bool UpdateUserImage(int userId, byte[] imageData)
        {
            return _healthProfileService.UpdateUserImage(userId, imageData);
        }

        public byte[]? GetUserImage(int userId)
        {
            return _healthProfileService.GetUserImage(userId);
        }

        public bool DeleteUserImage(int userId)
        {
            return _healthProfileService.DeleteUserImage(userId);
        }
    }
}
