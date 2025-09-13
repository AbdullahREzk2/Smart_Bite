using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SmartBite.BAL.DTOS;
using SmartBite.BAL.FoodTransactions;
using SmartBite.BAL.LimitsOperations;
using SmartBite.BAL.ReportManeger;
using SmartBite.DAL.Allergy;
using SmartBite.DAL.Calories;
using SmartBite.DAL.Disease;
using SmartBite.DAL.DiseaseNutrient;
using SmartBite.DAL.Models;

namespace SmartBite.BAL.MealOperations
{
    public class MealService : IMealService
    {
        private readonly IFoodTransactionManager _transactions;
        private readonly IAllergyService _allergyService;
        private readonly IDiseaseService _diseaseService;
        private readonly IReportService _reportService;
        private readonly IDiseaseNutrientService _diseaseNutrientService;
        private readonly ICaloriesService _caloriesService;
        private readonly ILimitService _limitService;

        public MealService(IFoodTransactionManager transactions ,IAllergyService allergyService,IDiseaseService diseaseService , IReportService reportService ,IDiseaseNutrientService diseaseNutrientService,ICaloriesService caloriesService,ILimitService limitService )
        {

            _transactions = transactions;
            _allergyService = allergyService;
            _diseaseService = diseaseService;
            _reportService = reportService;
            _diseaseNutrientService = diseaseNutrientService;
            _caloriesService = caloriesService;
            _limitService = limitService;
        }


        public string MealReport(List<TinyFoodItemDTO> TinyItems, int UserID)
        {
            List<FoodItemModel> FinalItems = _transactions.GetFoodItemsWithEditedSize(TinyItems);

            if(_diseaseService.IsUserHasDisease(UserID) && !_allergyService.IsUserHasAllergies(UserID))
            {

               var limits = _diseaseNutrientService.GetDiseaseNutrientLimitsFromVariable(UserID);
               return _reportService.GenerateDiseaseReport(FinalItems,limits);
                
            }

            else if(_allergyService.IsUserHasAllergies(UserID) && !_diseaseService.IsUserHasDisease(UserID))
            {

              var AllergyWithHarmfulItems = _allergyService.GetUserAllergiesWithHarmfulItems(UserID);
              return  _reportService.GenerateAllergyReport(FinalItems, AllergyWithHarmfulItems);


            }

            else if(_allergyService.IsUserHasAllergies(UserID) && _diseaseService.IsUserHasDisease(UserID))
            {

                var limits = _diseaseNutrientService.GetDiseaseNutrientLimitsFromVariable(UserID);
                var AllergyWithHarmfulItems = _allergyService.GetUserAllergiesWithHarmfulItems(UserID);
                return _reportService.GenerateAllergyandDiseaseReport(FinalItems, AllergyWithHarmfulItems, limits);

            }

            else
            {

                var UserRemainingCalories =  _caloriesService.GetUserRemainingDailyCalories(UserID);
                return _reportService.GenerateCaloriesReport(FinalItems, UserRemainingCalories);

            }


        }

        public bool Save(List<TinyFoodItemDTO> TinyItems, int UserID)
        {
            LimitsAndCaloriesDTO RemaininglimitsAndCalories = new LimitsAndCaloriesDTO();

            List<FoodItemModel> FinalItems = _transactions.GetFoodItemsWithEditedSize(TinyItems);

            var DailyLimits = _diseaseNutrientService.GetDiseaseNutrientLimitsFromVariable(UserID);
            var RemainingLimits =  _limitService.CalculateRemainingLimits(FinalItems, DailyLimits);
            RemaininglimitsAndCalories.Limits = RemainingLimits;
            _diseaseNutrientService.UpdateDailyLimits(UserID, RemainingLimits);

            var DailyCalories = _caloriesService.GetUserRemainingDailyCalories(UserID);
            var RemainingCalories = _limitService.CalculateRemainingCaloriesSafe(DailyCalories, FinalItems);
            RemaininglimitsAndCalories.Calories = RemainingCalories;
            _caloriesService.UpdateDailyCaloriesToVarible(UserID, RemainingCalories);

            return RemaininglimitsAndCalories != null ? true : false;
        }



    }
}
