using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SmartBite.DAL.Models;

namespace SmartBite.DAL.Calories
{
    public interface ICaloriesService
    {
        public bool UpdatedailyCalories(int userId, decimal calories);
        public bool UpdateDailyCaloriesToVarible(int userID, decimal remainingcalories);
        public bool AddDailyCalorieToVariable(int userId, decimal calories);
        public decimal GetUserRemainingDailyCalories(int userId);
        public decimal? GetUserOriginalCalories(int userId);
        public void AddBMIandBMR(int UserID, decimal BMI, decimal BMR);
        public BMIandBMRModel GetBMIandBMR(int UserID);
    }
}
