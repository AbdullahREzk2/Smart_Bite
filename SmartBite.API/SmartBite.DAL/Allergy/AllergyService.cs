using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Data.SqlClient;
using SmartBite.DAL.Models;

namespace SmartBite.DAL.Allergy
{
    public class AllergyService : IAllergyService
    {
        private readonly dbHelper _helper;

        public AllergyService(dbHelper helper)
        {
            _helper = helper;
        }



        public List<string> GetUserAllergies(int UserId)
        {
            List<string> allergies = new List<string>();

            using (SqlConnection connection = new SqlConnection(_helper.GetconnectionString()))
            {

                string query = @"select Allergies.Name from 
                                Users inner join UserAllergies on Users.UserID = UserAllergies.User_ID
                                inner join Allergies on UserAllergies.Allergie_ID = Allergies.AllergieID
                                where Users.UserID = @UserID;";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@UserID", UserId);

                    connection.Open();

                    using (SqlDataReader reader = command.ExecuteReader())
                    {

                        while (reader.Read())
                        {
                            if (reader["Name"] != DBNull.Value)
                            {
                                allergies.Add(reader["Name"].ToString()!);
                            }
                        }
                    }

                }
            }

            return allergies;
        }

        private bool AddUserAllergy(int UserId, string AllergyName)
        {
            int rowsAffected;

            using (SqlConnection connection = new SqlConnection(_helper.GetconnectionString()))
            {
                string query = @"INSERT INTO UserAllergies(User_ID, Allergie_ID)
                                 SELECT @UserID, AllergieID
                                 FROM Allergies
                                 WHERE Name = @AllergyName;";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@UserID", UserId);
                    command.Parameters.AddWithValue("@AllergyName", AllergyName);

                    connection.Open();
                    rowsAffected = command.ExecuteNonQuery();
                }
            }

            return rowsAffected > 0 ? true : false;
        }

        public bool AddUserAllergies(int userId, List<string> allergies)
        {
            foreach (string allergyName in allergies)
            {
               bool added = AddUserAllergy(userId, allergyName);
                if (!added)
                    return false;
            }
            return true;
        }

        public bool UpdateUserAllergies(int UserId, List<string> allergies)
        {
            bool deleted = DeleteUserAllergies(UserId);
            if (!deleted)
                return false;

            foreach (string allergyName in allergies)
            {
                bool added = AddUserAllergy(UserId, allergyName);
                if (!added)
                    return false;
            }

            return true;
        }

        public bool DeleteUserAllergies(int UserId)
        {
            int rowsAffected;

            using (SqlConnection connection = new SqlConnection(_helper.GetconnectionString()))
            {
                string query = @"DELETE FROM UserAllergies WHERE User_ID = @UserID;";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@UserID", UserId);

                    connection.Open();

                    rowsAffected = command.ExecuteNonQuery();
                }
            }

            return rowsAffected > 0 ? true : false;
        }

        public bool IsUserHasAllergies(int userId)
        {
            int count;

            using (SqlConnection connection = new SqlConnection(_helper.GetconnectionString()))
            {

                string query = @"SELECT COUNT(*) FROM UserAllergies WHERE User_ID = @UserID;";


                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@UserID", userId);

                    connection.Open();

                    count = (int)command.ExecuteScalar();
                }
            }

            return count > 0;
        }

        public List<AllergyItemModel> GetUserAllergiesWithHarmfulItems(int userId)
        {
            List<AllergyItemModel> allergies = new List<AllergyItemModel>();

            using (SqlConnection connection = new SqlConnection(_helper.GetconnectionString()))
            {

                string allergiesQuery = @"select Allergies.Name from 
                                   Users inner join UserAllergies on Users.UserID = UserAllergies.User_ID
                                   inner join Allergies on UserAllergies.Allergie_ID = Allergies.AllergieID
                                   where Users.UserID = @UserID;";

                using (var command = new SqlCommand(allergiesQuery, connection))
                {
                    command.Parameters.AddWithValue("@UserID", userId);

                    connection.Open();

                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            var allergyModel = new AllergyItemModel
                            {
                                Name = reader["Name"].ToString(),
                                HarmfulItems = new List<string>()
                            };

                            allergies.Add(allergyModel);
                        }
                    }
                }

                foreach (var allergy in allergies)
                {
                    string harmfulItemsQuery = @"select FoodItems.Name from 
                                          AllergiesHarmfulItems inner join FoodItems on 
                                          AllergiesHarmfulItems.FoodItem_ID = FoodItems.ItemID
                                          inner join Allergies on AllergiesHarmfulItems.Allergie_ID = Allergies.AllergieID
                                          where Allergies.Name = @Name;";

                    using (var command = new SqlCommand(harmfulItemsQuery, connection))
                    {
                        command.Parameters.AddWithValue("@Name", allergy.Name);

                        using (var reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                allergy.HarmfulItems!.Add(
                                reader.IsDBNull(reader.GetOrdinal("Name")) ? string.Empty : reader.GetString(reader.GetOrdinal("Name"))
                                );
                            }
                        }
                    }
                }
            }

            return allergies;
        }

        public List<AllergyIngModel> GetUserAllergiesWithHarmfulIngredients(int userId)
        {
            List<AllergyIngModel> allergies = new List<AllergyIngModel>();

            using (SqlConnection connection = new SqlConnection(_helper.GetconnectionString()))
            {
                // First query to get all allergies for the specified user
                string allergiesQuery = @"SELECT Allergies.Name 
                                  FROM Users 
                                  INNER JOIN UserAllergies ON Users.UserID = UserAllergies.User_ID
                                  INNER JOIN Allergies ON UserAllergies.Allergie_ID = Allergies.AllergieID
                                  WHERE Users.UserID = @UserID;";

                using (var command = new SqlCommand(allergiesQuery, connection))
                {
                    command.Parameters.AddWithValue("@UserID", userId);
                    connection.Open();
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            var allergyModel = new AllergyIngModel
                            {
                                Name = reader["Name"].ToString(),
                                HarmfulIngredients = new List<string>()
                            };
                            allergies.Add(allergyModel);
                        }
                    }
                }

                foreach (var allergy in allergies)
                {
                    string harmfulIngredientsQuery = @"SELECT Ingredients.Name 
                                               FROM AllergyHarmfulIngredients 
                                               INNER JOIN Ingredients ON AllergyHarmfulIngredients.Ingredient_ID = Ingredients.ID
                                               INNER JOIN Allergies ON AllergyHarmfulIngredients.Allergy_ID = Allergies.AllergieID
                                               WHERE Allergies.Name = @Name;";

                    using (var command = new SqlCommand(harmfulIngredientsQuery, connection))
                    {
                        command.Parameters.AddWithValue("@Name", allergy.Name);
                        using (var reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                allergy.HarmfulIngredients!.Add(
                                    reader.IsDBNull(reader.GetOrdinal("Name")) ? string.Empty : reader.GetString(reader.GetOrdinal("Name"))
                                );
                            }
                        }
                    }
                }
            }

            return allergies;
        }

       
    }
}
  