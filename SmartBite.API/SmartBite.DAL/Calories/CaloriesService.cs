using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Data.SqlClient;
using SmartBite.DAL.Models;

namespace SmartBite.DAL.Calories
{
    public class CaloriesService: ICaloriesService
    {
        private readonly dbHelper _helper;

        public CaloriesService(dbHelper helper)
        {
            _helper = helper;
        }

        public bool UpdatedailyCalories(int userId, decimal calories)
        {
            int rowsAffected;

            using (SqlConnection connection = new SqlConnection(_helper.GetconnectionString()))
            {
                string query = @"UPDATE Users SET DailyCalories = @Calories WHERE UserID = @UserId;";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@UserId", userId);
                    command.Parameters.AddWithValue("@Calories", calories);

                    connection.Open();

                    rowsAffected = command.ExecuteNonQuery();
                }
            }

            return rowsAffected > 0;
        }

        // we use it after Save operation -->
        public bool UpdateDailyCaloriesToVarible(int userID, decimal remainingcalories)
        {
            int rowsAffected;

            using (SqlConnection connection = new SqlConnection(_helper.GetconnectionString()))
            {
                string query = "UPDATE DailyCalorie SET VariableCalorie = @VariableCalorie WHERE User_ID = @UserID";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@UserID", userID);
                    command.Parameters.AddWithValue("@VariableCalorie", remainingcalories);

                    connection.Open();
                    rowsAffected = command.ExecuteNonQuery();

                }
            }

            return rowsAffected > 0;
        }

        public bool AddDailyCalorieToVariable(int userId, decimal calories)
        {
            int rowsAffected;

            using (SqlConnection connection = new SqlConnection(_helper.GetconnectionString()))
            {
                string query = @"INSERT INTO DailyCalorie (VariableCalorie, User_ID)
                          VALUES (@Calories, @UserId);";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@UserId", userId);
                    command.Parameters.AddWithValue("@Calories", calories);

                    connection.Open();

                    rowsAffected = command.ExecuteNonQuery();
                }
            }

            return rowsAffected > 0;
        }

        public decimal GetUserRemainingDailyCalories(int userId)
        {
            decimal calories = 0;

            using (SqlConnection connection = new SqlConnection(_helper.GetconnectionString()))
            {
                string query = @"SELECT VariableCalorie FROM DailyCalorie WHERE User_ID = @UserId;";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@UserId", userId);

                    connection.Open();
                    object result = command.ExecuteScalar();

                    if (result != null && decimal.TryParse(result.ToString(), out decimal parsedCalories))
                    {
                        calories = parsedCalories;
                    }
                }
            }

            return calories;
        }


        public decimal? GetUserOriginalCalories(int userId)
        {
            decimal calories = 0;

            using (SqlConnection connection = new SqlConnection(_helper.GetconnectionString()))
            {
                string query = @"SELECT DailyCalories FROM Users WHERE UserID = @UserId;";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@UserId", userId);

                    connection.Open();
                    object result = command.ExecuteScalar();

                    if (result == null || result == DBNull.Value)
                    {
                        return null;
                    }
                    else if (result != null && decimal.TryParse(result.ToString(), out decimal parsedCalories))
                    {
                        calories = parsedCalories;
                    }
                }
            }

            return calories;
        }


        public void AddBMIandBMR(int UserID,decimal BMI,decimal BMR)
        {
            using(SqlConnection connection=new SqlConnection(_helper.GetconnectionString()))
            {
                string Query = "UPDATE Users SET BMR = @BMR, BMI = @BMI WHERE UserID = @UserID";

                using (SqlCommand command=new SqlCommand(Query, connection))
                {
                    command.Parameters.AddWithValue("@UserID", UserID);
                    command.Parameters.AddWithValue("@BMI", BMI);
                    command.Parameters.AddWithValue("@BMR", BMR);

                    connection.Open();

                    command.ExecuteNonQuery();
                }
            }
        }

        public BMIandBMRModel GetBMIandBMR(int UserID)
        {
            BMIandBMRModel result = new BMIandBMRModel();

            using (SqlConnection connection = new SqlConnection(_helper.GetconnectionString()))
            {
                string query = "SELECT BMR, BMI FROM Users WHERE UserID = @UserID;";
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@UserID", UserID);
                    connection.Open();

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            result.BMR = reader["BMR"] != DBNull.Value ? Convert.ToDecimal(reader["BMR"]) : 0m;
                            result.BMI = reader["BMI"] != DBNull.Value ? Convert.ToDecimal(reader["BMI"]) : 0m;
                        }
                       
                    }
                }
            }

            return result;
        }




    }
}
