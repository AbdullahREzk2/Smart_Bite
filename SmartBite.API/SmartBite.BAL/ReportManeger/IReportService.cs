using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SmartBite.DAL.Models;

namespace SmartBite.BAL.ReportManeger
{
    public interface IReportService
    {
        public string GenerateDiseaseReport(List<FoodItemModel> foodItems, List<NutrientLimitModel> nutrientLimits);
        public string GenerateAllergyReport(List<FoodItemModel> foodItems, List<AllergyItemModel> allergies);
        public string GenerateAllergyandDiseaseReport(List<FoodItemModel> foodItems, List<AllergyItemModel> allergies, List<NutrientLimitModel> nutrientLimits);
        public string GenerateCaloriesReport(List<FoodItemModel> foodItems, decimal remainingDailyCalories);

        public string GenerateProductAllergyReport(List<string> ingredients, List<AllergyIngModel> allergies);


    }
}
