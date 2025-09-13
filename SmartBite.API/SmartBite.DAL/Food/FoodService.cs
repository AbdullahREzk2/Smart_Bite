using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Data.SqlClient;
using SmartBite.DAL.Models;

namespace SmartBite.DAL.Food
{
    public class FoodService: IFoodService
    {
        private readonly dbHelper _helper;

        public FoodService(dbHelper helper)
        {
            _helper = helper;
        }

        public FoodItemModel GetFoodItemByName(string name)
        {
            using (SqlConnection connection = new SqlConnection(_helper.GetconnectionString()))
            {

                string query = @"SELECT Name, DefaultServing, ServingUnit, EnergyPerServing FROM FoodItems WHERE Name = @Name;";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@Name",name);

                    FoodItemModel Item = new FoodItemModel();
                    connection.Open();

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {

                            Item.Name = reader.GetString(reader.GetOrdinal("Name"));
                            Item.ServingSize = reader.GetDecimal(reader.GetOrdinal("DefaultServing"));
                            Item.ServingUnit = reader.GetString(reader.GetOrdinal("ServingUnit"));
                            Item.EnergyPerServing = reader.IsDBNull(reader.GetOrdinal("EnergyPerServing"))
                           ? 0
                           : reader.GetDecimal(reader.GetOrdinal("EnergyPerServing"));


                        }
                    }
                 Item.Nutrients = GetFoodItemNutrients(Item.Name!);
                 return Item;
                }
            }

        }

        public List<ItemNutrientModel> GetFoodItemNutrients(string name)
        {
            var nutrients = new List<ItemNutrientModel>();

            using (SqlConnection connection = new SqlConnection(_helper.GetconnectionString()))
            {
                string query = @"
            SELECT 
                Nutrients.Name, 
                ItemNutrients.IntakePerServing, 
                Nutrients.DefaultUnit
            FROM Nutrients
            INNER JOIN ItemNutrients ON Nutrients.NutrientID = ItemNutrients.Nutrient_ID
            INNER JOIN FoodItems ON ItemNutrients.FoodItem_ID = FoodItems.ItemID
            WHERE FoodItems.ItemID IN (
                SELECT ItemID FROM FoodItems WHERE Name = @ItemName
            );";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    // ✅ تأكد إنك بتمرر المعامل هنا:
                    command.Parameters.AddWithValue("@ItemName", name);

                    connection.Open();

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            var nutrient = new ItemNutrientModel
                            {
                                Name = reader.IsDBNull(reader.GetOrdinal("Name"))
                                    ? string.Empty
                                    : reader.GetString(reader.GetOrdinal("Name")),

                                AmountPerServing = reader.IsDBNull(reader.GetOrdinal("IntakePerServing"))
                                    ? 0m
                                    : reader.GetDecimal(reader.GetOrdinal("IntakePerServing")),

                                AmountUnit = reader.IsDBNull(reader.GetOrdinal("DefaultUnit"))
                                    ? string.Empty
                                    : reader.GetString(reader.GetOrdinal("DefaultUnit"))
                            };

                            nutrients.Add(nutrient);
                        }
                    }
                }
            }

            return nutrients;
        }


        public TinyFoodItemModel GetTinyItemByName(string Name)
        {
            using (SqlConnection connection = new SqlConnection(_helper.GetconnectionString()))
            {
                string query = @"SELECT Name, DefaultServing, ServingUnit FROM FoodItems WHERE Name = @Name;";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@Name", Name);

                    TinyFoodItemModel Item = new TinyFoodItemModel();

                    connection.Open();

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {

                            Item.Name = reader["Name"].ToString();
                            Item.ServingSize =(decimal) reader["DefaultServing"];
                            Item.ServingUnit = reader["ServingUnit"].ToString();

                        }
                    }
                 return Item;
                }
            }
        }

        public List<string> GetAllFoodItemsNames()
        {
            List<string> foodNames = new List<string>();

            using (SqlConnection connection = new SqlConnection(_helper.GetconnectionString()))
            {
                string query = "SELECT Name FROM FoodItems";
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    connection.Open();
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            foodNames.Add(reader["Name"].ToString()!);
                        }
                    }
                }
            }

            return foodNames;
        }


    }
}
