using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Data.SqlClient;
using SmartBite.DAL.Allergy;
using SmartBite.DAL.Disease;
using SmartBite.DAL.Models;

namespace SmartBite.DAL.HealthProfile
{
    public class HealthProfileService: IHealthProfileService
    {
        private readonly dbHelper _helper;
        private readonly IDiseaseService _diseaseService;
        private readonly IAllergyService _allergyService;

        public HealthProfileService(dbHelper helper , IDiseaseService diseaseService , IAllergyService allergyService)
        {
            _helper = helper;
            _diseaseService = diseaseService;
            _allergyService = allergyService;
        }


        public bool AddUserHealthData(int userId, int age, decimal weight, decimal height, string gender, string activityLevel)
        {
            int rowsAffected = 0;

            using (SqlConnection connection = new SqlConnection(_helper.GetconnectionString()))
            {
                string query = @"UPDATE Users
                                 SET Age = @age,
                                     Weight = @weight,
                                     Height = @height,
                                     Gender = @gender,
                                     ActivityLevel = @activelevel
                                 WHERE UserID = @userId;";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@age", age);
                    command.Parameters.AddWithValue("@weight", weight);
                    command.Parameters.AddWithValue("@height", height);
                    command.Parameters.AddWithValue("@gender", gender);
                    command.Parameters.AddWithValue("@activelevel", activityLevel);
                    command.Parameters.AddWithValue("@userId", userId);

                    connection.Open();
                    rowsAffected = command.ExecuteNonQuery();

                }
            }

            return rowsAffected > 0;
        }


        public UserHealthDataModel GetUserHealth(int UserID)
        {
            UserHealthDataModel userHealthData = new UserHealthDataModel();

            using (SqlConnection connection = new SqlConnection(_helper.GetconnectionString()))
            {
                string query = "SELECT Age, Weight, Height, Gender, ActivityLevel, BMR, BMI FROM Users WHERE UserID = @UserID;";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@UserID", UserID);

                    connection.Open();

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            userHealthData.Age = reader["Age"] != DBNull.Value ? Convert.ToInt32(reader["Age"]) : 0;
                            userHealthData.Weight = reader["Weight"] != DBNull.Value ? Convert.ToDecimal(reader["Weight"]) : 0;
                            userHealthData.Height = reader["Height"] != DBNull.Value ? Convert.ToDecimal(reader["Height"]) : 0;
                            userHealthData.Gender = reader["Gender"] != DBNull.Value ? reader["Gender"].ToString() : null;
                            userHealthData.ActivityLevel = reader["ActivityLevel"] != DBNull.Value ? reader["ActivityLevel"].ToString() : null;
                            userHealthData.BMR = reader["BMR"] != DBNull.Value ? Convert.ToDecimal(reader["BMR"]) : 0;
                            userHealthData.BMI = reader["BMI"] != DBNull.Value ? Convert.ToDecimal(reader["BMI"]) : 0;
                        }
                    }
                }
            }

            return userHealthData;
        }

        private PersonModel GetUserData(int UserID)
        {
            PersonModel? persondata = null; // start with null

            using (SqlConnection connection = new SqlConnection(_helper.GetconnectionString()))
            {
                string Query = @"SELECT Name, Email, Password, Age, Weight, Height, Gender, ActivityLevel 
                         FROM Users WHERE UserID = @UserID;";
                using (SqlCommand command = new SqlCommand(Query, connection))
                {
                    command.Parameters.AddWithValue("@UserID", UserID);

                    connection.Open();

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            persondata = new PersonModel
                            {
                                Name = reader["Name"]?.ToString(),
                                Email = reader["Email"]?.ToString(),
                                Password = reader["Password"]?.ToString(),
                                Age = reader["Age"] != DBNull.Value ? Convert.ToInt32(reader["Age"]) : 0,
                                Weight = reader["Weight"] != DBNull.Value ? Convert.ToDecimal(reader["Weight"]) : 0m,
                                Height = reader["Height"] != DBNull.Value ? Convert.ToDecimal(reader["Height"]) : 0m,
                                Gender = reader["Gender"]?.ToString(),
                                ActivityLevel = reader["ActivityLevel"]?.ToString()
                            };
                        }
                        
                    }
                }
            }
            return persondata!;
        }


        public UserHealthprofileDataModel GetUserHealthProfileData(int UserID)
        {
            UserHealthprofileDataModel UserData = new UserHealthprofileDataModel();
            var UserMainData = GetUserData(UserID);

            UserData.Disease = _diseaseService.GetUserDisease(UserID);
            UserData.Allergirs = _allergyService.GetUserAllergies(UserID);
            
            UserData.Name=UserMainData.Name;
            UserData.Email = UserMainData.Email;
            UserData.Password=UserMainData.Password;
            UserData.Age = UserMainData.Age;
            UserData.Weight = UserMainData.Weight;
            UserData.Height = UserMainData.Height;
            UserData.Gender=UserMainData.Gender;
            UserData.ActivityLevel = UserMainData.ActivityLevel;

            return UserData;
        }

        public bool UpdateProfile(int ID, UserHealthprofileDataModel DataModel)
        {
            int rowsAffected;

            using (SqlConnection connection = new SqlConnection(_helper.GetconnectionString()))
            {
                string query = @"
                     UPDATE Users 
                     SET 
                     Name = @Name,
                     Email = @Email,
                     Password = @Password,
                     Height = @Height,
                     Weight = @Weight,
                     Gender = @Gender,
                     Age = @Age,
                     ActivityLevel = @ActivityLevel
                     WHERE UserID = @UserID";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@Name", DataModel.Name ?? (object)DBNull.Value);
                    command.Parameters.AddWithValue("@Email", DataModel.Email ?? (object)DBNull.Value);
                    command.Parameters.AddWithValue("@Password", DataModel.Password ?? (object)DBNull.Value);
                    command.Parameters.AddWithValue("@Height", DataModel.Height);
                    command.Parameters.AddWithValue("@Weight", DataModel.Weight);
                    command.Parameters.AddWithValue("@Gender", DataModel.Gender ?? (object)DBNull.Value);
                    command.Parameters.AddWithValue("@Age", DataModel.Age);
                    command.Parameters.AddWithValue("@ActivityLevel", DataModel.ActivityLevel ?? (object)DBNull.Value);
                    command.Parameters.AddWithValue("@UserID", ID);

                    connection.Open();
                    rowsAffected = command.ExecuteNonQuery();
                }
            }
            return rowsAffected > 0;
        }

        public int GetTakeAnsolin(int UserID)
        {
            int takeInsolin = 0;

            using (SqlConnection connection = new SqlConnection(_helper.GetconnectionString()))
            {
                string query = "SELECT TakeInsolin FROM Users WHERE UserID = @UserID;";
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@UserID", UserID);
                    connection.Open();

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            takeInsolin = reader["TakeInsolin"] != DBNull.Value ? Convert.ToInt32(reader["TakeInsolin"]) : 0;
                        }
                    }
                }
            }

            return takeInsolin;
        }


        public bool UpdateUserImage(int userId, byte[] imageData)
        {
            int rowsAffected;

            using (SqlConnection connection = new SqlConnection(_helper.GetconnectionString()))
            {
                string query = @"UPDATE Users 
                                 SET Image = @Image 
                                 WHERE UserID = @UserID";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.Add("@Image", SqlDbType.VarBinary).Value = (object)imageData ?? DBNull.Value;
                    command.Parameters.Add("@UserID", SqlDbType.Int).Value = userId;

                    connection.Open();
                    rowsAffected = command.ExecuteNonQuery();
                }
            }

            return rowsAffected > 0;
        }


        public byte[]? GetUserImage(int userId)
        {
            object result;
            using (SqlConnection connection = new SqlConnection(_helper.GetconnectionString()))
            {
                string query = @"SELECT Image FROM Users WHERE UserID = @UserID";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.Add("@UserID", SqlDbType.Int).Value = userId;

                    connection.Open();
                    result = command.ExecuteScalar();
                }
            }

            return result != DBNull.Value ? (byte[])result : null;
        }


        public bool DeleteUserImage(int userId)
        {
            int rowsAffected;

            using (SqlConnection connection = new SqlConnection(_helper.GetconnectionString()))
            {
                string query = @"UPDATE Users SET Image = NULL WHERE UserID = @UserID";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.Add("@UserID", SqlDbType.Int).Value = userId;

                    connection.Open();
                    rowsAffected = command.ExecuteNonQuery();
                }
            }

            return rowsAffected > 0;
        }
    }
}
