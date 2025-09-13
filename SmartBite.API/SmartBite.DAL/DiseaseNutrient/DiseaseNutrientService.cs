using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Data.SqlClient;
using SmartBite.DAL.Models;

namespace SmartBite.DAL.DiseaseNutrient
{
    public class DiseaseNutrientService : IDiseaseNutrientService
    {
        private readonly dbHelper _helper;

        public DiseaseNutrientService(dbHelper helper)
        {
            _helper = helper;
        }


        public List<NutrientLimitModel> GetDiseaseNutrientLimits(int UserId)
        {
            using (SqlConnection connection = new SqlConnection(_helper.GetconnectionString()))
            {
                string query = @"SELECT Nutrients.Name, DiseaseNutrientLimits.LimitPerDay,
                                 Nutrients.DefaultUnit
                                 FROM DiseaseNutrientLimits inner
                                 JOIN UserDiseases ON DiseaseNutrientLimits.UserDisease_ID = UserDiseases.ID inner
                                 JOIN Nutrients ON DiseaseNutrientLimits.Nutrient_ID = Nutrients.NutrientID
                                 WHERE UserDiseases.User_ID = @UserID;";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@UserID", UserId);

                    List<NutrientLimitModel> limits = new List<NutrientLimitModel>();
                    connection.Open();

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            limits.Add(new NutrientLimitModel
                            {
                                NutrientName = reader["Name"].ToString(),
                                LimitPerDay = (decimal)reader["LimitPerDay"],
                                NutrientUnit = reader["DefaultUnit"].ToString()
                            });
                        }
                    }
                    return limits;
                }
            }
        }

        public void AddDiseaseNutrientLimitsForUser(int userId, List<stNutrientLimitModel> limits)
        {
            foreach (var limit in limits)
            {
                AddDiseaseNutrientLimitForUser(userId, limit);
            }
        }

        private int AddDiseaseNutrientLimitForUser(int userId, stNutrientLimitModel limit)
        {
            int newId = 0;

            using (SqlConnection connection = new SqlConnection(_helper.GetconnectionString()))
            {
                connection.Open();

                string checkQuery = @"SELECT TOP 1 dnl.ID
                                      FROM DiseaseNutrientLimits dnl
                                      INNER JOIN UserDiseases ud ON dnl.UserDisease_ID = ud.ID
                                      INNER JOIN Nutrients n ON dnl.Nutrient_ID = n.NutrientID
                                      WHERE ud.User_ID = @UserID 
                                      AND n.Name = @NutrientName;";

                using (SqlCommand checkCmd = new SqlCommand(checkQuery, connection))
                {
                    checkCmd.Parameters.AddWithValue("@UserID", userId);
                    checkCmd.Parameters.AddWithValue("@NutrientName", limit.NutrientName);

                    object result = checkCmd.ExecuteScalar();
                    if (result != null)
                    {
                        newId = Convert.ToInt32(result);
                        return newId; 
                    }
                }

                // ✅ 2️⃣ Insert only if not exists
                string insertQuery = @"INSERT INTO DiseaseNutrientLimits (Nutrient_ID, LimitPerDay, UserDisease_ID)
                                       OUTPUT INSERTED.ID
                                       SELECT TOP 1 n.NutrientID, @LimitPerDay, ud.ID
                                       FROM UserDiseases ud
                                       INNER JOIN Users u ON u.UserID = ud.User_ID
                                       INNER JOIN Nutrients n ON n.Name = @NutrientName
                                       WHERE u.UserID = @UserID;";

                using (SqlCommand insertCmd = new SqlCommand(insertQuery, connection))
                {
                    insertCmd.Parameters.AddWithValue("@UserID", userId);
                    insertCmd.Parameters.AddWithValue("@NutrientName", limit.NutrientName);
                    insertCmd.Parameters.AddWithValue("@LimitPerDay", limit.LimitPerDay);

                    object insertResult = insertCmd.ExecuteScalar();
                    if (insertResult != null)
                    {
                        newId = Convert.ToInt32(insertResult);
                    }
                }
            }

            return newId;
        }

        public bool DeleteDiseaseNutrientLimitForUser(int UserId)
        {
            int rowsAffected;
            using (SqlConnection connection = new SqlConnection(_helper.GetconnectionString()))
            {
                string query = @"DELETE FROM DiseaseNutrientLimits
                                 WHERE UserDisease_ID IN
                                 (
                                 SELECT ID FROM UserDiseases
                                 WHERE User_ID = @UserID
                                                        );";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.Add("@UserID", SqlDbType.Int).Value = UserId;
                    connection.Open();
                    rowsAffected= command.ExecuteNonQuery();
                }
            }
          
           return rowsAffected>0?true:false;
        }

        public bool UpdateDiseaseNutrientLimitForUser(int UserId, List<stNutrientLimitModel> newLimits)
        {

            bool deleted = DeleteDiseaseNutrientLimitForUser(UserId);
            if (!deleted)
                return false;

            foreach (stNutrientLimitModel limit in newLimits)
            {
                int added = AddDiseaseNutrientLimitForUser(UserId, limit);
                
            }
            return true;

        }

        public List<NutrientLimitModel> GetDiseaseNutrientLimitsFromVariable(int userId)
        {
            List<NutrientLimitModel> limits = new List<NutrientLimitModel>();

            using (SqlConnection connection = new SqlConnection(_helper.GetconnectionString()))
            {

                string query = @"SELECT Nutrients.Name, DailyLimit.VariableLimit,Nutrients.DefaultUnit
                                 FROM DiseaseNutrientLimits inner
                                 JOIN UserDiseases ON DiseaseNutrientLimits.UserDisease_ID = UserDiseases.ID inner
                                 JOIN Nutrients ON DiseaseNutrientLimits.Nutrient_ID = Nutrients.NutrientID
                                 inner join DailyLimit on DailyLimit.LimitID = DiseaseNutrientLimits.ID
                                 WHERE UserDiseases.User_ID = @UserID;";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@UserID", userId);

                    connection.Open();

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            limits.Add(new NutrientLimitModel
                            {
                                NutrientName = reader.GetString(reader.GetOrdinal("Name")),
                                LimitPerDay = reader.IsDBNull(reader.GetOrdinal("VariableLimit")) ? (decimal?)null: reader.GetDecimal(reader.GetOrdinal("VariableLimit")),
                                NutrientUnit = reader.GetString(reader.GetOrdinal("DefaultUnit"))
                            });

                        }
                    }
                }
            }

            foreach (var limit in limits)
            {
                if (limit.LimitPerDay == null)
                {
                    var Locallimit = GetDiseaseNutrienSpecifictLimit(userId, limit.NutrientName!);
                    limit.LimitPerDay = Locallimit.LimitPerDay;
                    limit.NutrientUnit = Locallimit.NutrientUnit;
                }
            }


            return limits;
        }

        private NutrientLimitModel GetDiseaseNutrienSpecifictLimit(int userId, string LimitNutrientName)
        {
            NutrientLimitModel limit = new NutrientLimitModel();

            using (SqlConnection connection = new SqlConnection(_helper.GetconnectionString()))
            {

                string query = @"SELECT DiseaseNutrientLimits.LimitPerDay, Nutrients.DefaultUnit
                                 FROM DiseaseNutrientLimits inner
                                 JOIN UserDiseases ON DiseaseNutrientLimits.UserDisease_ID = UserDiseases.ID inner
                                 JOIN Nutrients ON DiseaseNutrientLimits.Nutrient_ID = Nutrients.NutrientID
                                 inner join DailyLimit on DailyLimit.LimitID = DiseaseNutrientLimits.ID
                                 WHERE UserDiseases.User_ID = @UserID and Nutrients.Name = @Name;";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@UserID", userId);
                    command.Parameters.AddWithValue("@Name", LimitNutrientName);

                    connection.Open();

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            limit.NutrientName = LimitNutrientName;
                            limit.LimitPerDay = reader.GetDecimal(reader.GetOrdinal("LimitPerDay"));
                            limit.NutrientUnit = reader.GetString(reader.GetOrdinal("DefaultUnit"));
                        }
                    }
                }
            }

            return limit;
        }

        public void UpdateDailyLimits(int userId, List<NutrientLimitModel> RemainingLimits)
        {
            foreach (var limit in RemainingLimits)
            {
                UpdateDailyLimit(userId, limit);
            }
        }

        private bool UpdateDailyLimit(int userId, NutrientLimitModel limit)
        {
            int rowsAffected;
            int limitId = GetDiseaseNutrientLimitID(userId, limit.NutrientName!);

            using (SqlConnection connection = new SqlConnection(_helper.GetconnectionString()))
            {
                string query = "UPDATE DailyLimit SET VariableLimit = @VariableLimit WHERE User_ID = @UserId AND LimitID = @LimitId";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@UserId", userId);
                    command.Parameters.AddWithValue("@LimitId", limitId);
                    command.Parameters.AddWithValue("@VariableLimit", limit.LimitPerDay);

                    connection.Open();
                    rowsAffected = command.ExecuteNonQuery();
                }
            }

            return rowsAffected > 0;
        }

        private int GetDiseaseNutrientLimitID(int userId, string nutrientName)
        {
            int LimitID;

            using (SqlConnection connection = new SqlConnection(_helper.GetconnectionString()))
            {

                string query = @"SELECT DiseaseNutrientLimits.ID
                                 FROM DiseaseNutrientLimits 
                                 INNER JOIN UserDiseases ON DiseaseNutrientLimits.UserDisease_ID = UserDiseases.ID
                                 INNER JOIN Nutrients ON DiseaseNutrientLimits.Nutrient_ID = Nutrients.NutrientID
                                 WHERE UserDiseases.User_ID = @UserId AND Nutrients.Name = @NutrientName";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@UserId", userId);
                    command.Parameters.AddWithValue("@NutrientName", nutrientName);

                    connection.Open();

                    object result = command.ExecuteScalar();
                    LimitID = result != null ? Convert.ToInt32(result) : 0;
                }
            }

            return LimitID;
        }

        public void AddDiseaseNutrientLimitsToVariable(int userId, List<stNutrientLimitModel> limits)
        {
            foreach (var limit in limits)
            {
                AddDiseaseNutrientLimitToVariable(userId, limit);
            }
        }

        private bool AddDiseaseNutrientLimitToVariable(int userId, stNutrientLimitModel limit)
        {
            int rowsAffected;

            int limitId = GetDiseaseNutrientLimitID(userId, limit.NutrientName!);

            if (limitId == 0)
            {
                // Parent does not exist → create it
                limitId = AddDiseaseNutrientLimitForUser(userId, limit);
            }

            if (limitId == 0)
            {
                // Still failed → can't insert child
                return false;
            }

            using (SqlConnection connection = new SqlConnection(_helper.GetconnectionString()))
            {
                string query = @"INSERT INTO DailyLimit (LimitID, VariableLimit, User_ID)
                                 VALUES (@LimitID, NULL, @UserID);";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@UserID", userId);
                    command.Parameters.AddWithValue("@LimitID", limitId);

                    connection.Open();
                    rowsAffected = command.ExecuteNonQuery();
                }
            }

            return (rowsAffected > 0);
        }


        private void DeleteDiseaseNutrientLimitsFromVariable(int userId)
        {
            using (SqlConnection connection = new SqlConnection(_helper.GetconnectionString()))
            {
                string query = @"DELETE FROM DailyLimit WHERE User_ID = @UserID;";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.Add("@UserID", SqlDbType.Int).Value = userId;
                    connection.Open();
                    command.ExecuteNonQuery();
                }
            }
        }

        public void UpdateDiseaseNutrientLimitsToVariable(int userId, List<stNutrientLimitModel> newLimits)
        {
            using (SqlConnection connection = new SqlConnection(_helper.GetconnectionString()))
            {
                AddDiseaseNutrientLimitsToVariable(userId, newLimits);
            }
        }




    }
}
