using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SmartBite.BAL.DTOS;

namespace SmartBite.BAL.CalorieCalculator
{
    public interface ICalorieCalculatorManager
    {

        public CalDataDTO CalculateDailyCalories(PersonDTO person);

        public CalDataDTO CalculateDailyCalories(HealthDataDTO Data);


    }
}
