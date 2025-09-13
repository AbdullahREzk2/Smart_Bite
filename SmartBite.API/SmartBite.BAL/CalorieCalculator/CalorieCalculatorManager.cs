using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SmartBite.BAL.DTOS;

namespace SmartBite.BAL.CalorieCalculator
{

    public class CalorieCalculatorManager : ICalorieCalculatorManager
    {
        private enum enGender { Male, Female }

        private enum enActivityLevel { Sedentary, LightlyActive, ModeratelyActive, VeryActive, SuperActive }

        public CalDataDTO CalculateDailyCalories(PersonDTO person)
        {
            enGender gender = ParseGender(person.Gender!);
            enActivityLevel activityLevel = ParseActivityLevel(person.ActivityLevel!);

            decimal bmr = CalculateBMR(person.Weight, person.Height, person.Age, gender);
            decimal tdee = CalculateTDEE(bmr, activityLevel);
            decimal bmi = CalculateBMI(person.Weight, person.Height);

            return new CalDataDTO
            {
                DailyCalories = Math.Round(tdee),
                Bmr = Math.Round(bmr),
                BmI = Math.Round(bmi, 2)
            };
        }

        public CalDataDTO CalculateDailyCalories(HealthDataDTO Data)
        {
            enGender gender = ParseGender(Data.Gender!);
            enActivityLevel activityLevel = ParseActivityLevel(Data.ActivityLevel!);

            decimal bmr = CalculateBMR(Data.Weight, Data.Height, Data.Age, gender);
            decimal tdee = CalculateTDEE(bmr, activityLevel);
            decimal bmi = CalculateBMI(Data.Weight, Data.Height);

            return new CalDataDTO
            {
                DailyCalories = Math.Round(tdee),
                Bmr = Math.Round(bmr),
                BmI = Math.Round(bmi, 2)
            };
        }



        private static decimal CalculateBMR(decimal weight, decimal height, int age, enGender gender)
        {
            return gender == enGender.Male
                ? (10 * weight) + (6.25m * height) - (5 * age) + 5
                : (10 * weight) + (6.25m * height) - (5 * age) - 161;
        }

        private static decimal CalculateTDEE(decimal bmr, enActivityLevel activityLevel)
        {
            decimal activityFactor = GetActivityFactor(activityLevel);
            return bmr * activityFactor;
        }

        private static decimal CalculateBMI(decimal weight, decimal height)
        {
            if (height == 0) return 0;
            decimal heightInMeters = height / 100;
            return weight / (heightInMeters * heightInMeters);
        }

        private static decimal GetActivityFactor(enActivityLevel activityLevel)
        {
            return activityLevel switch
            {
                enActivityLevel.Sedentary => 1.2m,
                enActivityLevel.LightlyActive => 1.375m,
                enActivityLevel.ModeratelyActive => 1.55m,
                enActivityLevel.VeryActive => 1.725m,
                enActivityLevel.SuperActive => 1.9m,
                _ => 1.2m
            };
        }

        private static enGender ParseGender(string genderString)
        {
            return string.IsNullOrEmpty(genderString) ? enGender.Male :
                   genderString.Trim().ToLower() == "Female" ? enGender.Female : enGender.Male;
        }

        private static enActivityLevel ParseActivityLevel(string activityLevelString)
        {
            return activityLevelString?.Trim().ToLower() switch
            {
                "sedentary" => enActivityLevel.Sedentary,
                "lightly active" => enActivityLevel.LightlyActive,
                "moderate" => enActivityLevel.ModeratelyActive,
                "very active" => enActivityLevel.VeryActive,
                "super active" => enActivityLevel.SuperActive,
                _ => enActivityLevel.Sedentary
            };
        }



    }

}
