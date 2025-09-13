using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SmartBite.DAL.Models;

namespace SmartBite.BAL.LimitsOperations
{
    public interface ILimitService
    {
        public List<NutrientLimitModel> CalculateRemainingLimits(List<FoodItemModel> items, List<NutrientLimitModel> limits);

        public decimal CalculateRemainingCaloriesSafe(decimal dailyCalorie, List<FoodItemModel> items);


    }
}
