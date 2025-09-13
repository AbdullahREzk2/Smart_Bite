using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SmartBite.DAL.Models;

namespace SmartBite.BAL.LimitsOperations
{
    public class LimitService: ILimitService
    {
        public List<NutrientLimitModel> CalculateRemainingLimits(List<FoodItemModel> items, List<NutrientLimitModel> limits)
        {
            if (items == null || limits == null)
                return limits ?? new List<NutrientLimitModel>();

            // Create a copy of the limits to avoid modifying the original list
            var remainingLimits = limits.Select(limit => new NutrientLimitModel
            {
                NutrientName = limit.NutrientName,
                LimitPerDay = limit.LimitPerDay,
                NutrientUnit = limit.NutrientUnit
            }).ToList();

            // Calculate total consumed amounts for each nutrient across all food items
            var totalConsumedNutrients = new Dictionary<string, decimal>();

            foreach (var item in items)
            {
                if (item.Nutrients == null) continue;

                foreach (var nutrient in item.Nutrients)
                {
                    if (string.IsNullOrEmpty(nutrient.Name)) continue;

                    var nutrientKey = nutrient.Name.ToLower().Trim();

                    if (totalConsumedNutrients.ContainsKey(nutrientKey))
                    {
                        totalConsumedNutrients[nutrientKey] += nutrient.AmountPerServing;
                    }
                    else
                    {
                        totalConsumedNutrients[nutrientKey] = nutrient.AmountPerServing;
                    }
                }
            }

            // Subtract consumed amounts from the limits
            foreach (var limit in remainingLimits)
            {
                if (string.IsNullOrEmpty(limit.NutrientName) || !limit.LimitPerDay.HasValue)
              continue;

                var limitKey = limit.NutrientName.ToLower().Trim();

                if (totalConsumedNutrients.ContainsKey(limitKey))
                {
                    limit.LimitPerDay = Math.Max(0, limit.LimitPerDay.Value - totalConsumedNutrients[limitKey]);
                }
            }

            return remainingLimits;
        }

        public decimal CalculateRemainingCaloriesSafe(decimal dailyCalorie, List<FoodItemModel> items)
        {
            // Handle null list
            if (items == null)
                return dailyCalorie;

            // Sum up all EnergyPerServing from the food items
            decimal totalConsumedCalories = items.Sum(item => item.EnergyPerServing);

            // Subtract consumed calories from daily calorie budget
            decimal remainingCalories = dailyCalorie - totalConsumedCalories;

            return Math.Round(remainingCalories);

        }



    }
}
