using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Data.SqlClient;
using SmartBite.DAL.Models;

namespace SmartBite.DAL.Medication
{
    public class MedicationService: IMedicationService
    {
        private readonly dbHelper _helper;

        public MedicationService(dbHelper helper)
        {
            _helper = helper;
        }

        public bool AddMedicine(int UserId, MedicineModel medicine)
        {
            object result;
            using (SqlConnection connection = new SqlConnection(_helper.GetconnectionString()))
            {
                string query = @"INSERT INTO Medicines (Name, Type, Time, User_ID) 
                                 VALUES (@Name, @Type, @Time, @UserID);
                                 SELECT SCOPE_IDENTITY();";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@Name", medicine.Name);
                    command.Parameters.AddWithValue("@Type", medicine.Type);
                    command.Parameters.AddWithValue("@Time", medicine.Time);
                    command.Parameters.AddWithValue("@UserID", UserId);

                    connection.Open();
                    result = command.ExecuteScalar();
                }
            }

            return result != null && int.TryParse(result.ToString(), out int id) && id > 0;
        }

        public bool DeleteMedicine(int UserId,int MedicineID)
        {
            int rowsAffected;
            using (SqlConnection connection = new SqlConnection(_helper.GetconnectionString()))
            {
                string query = @"DELETE FROM Medicines WHERE MedicineID = @MedicineID AND User_ID = @UserID";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@MedicineID", MedicineID);
                    command.Parameters.AddWithValue("@UserID", UserId);

                    connection.Open();
                    rowsAffected = command.ExecuteNonQuery();

                }
            }
        
            return rowsAffected > 0?true:false;
        }

        public bool UpdateMedicine(int UserId, MedicineModel medicine)
        {
            int rowsAffected;
            using (SqlConnection connection = new SqlConnection(_helper.GetconnectionString()))
            {
                string query = @"UPDATE Medicines 
                                 SET Name = @Name, 
                                 Type = @Type, 
                                 Time = @Time
                                 WHERE MedicineID = @MedicineID AND User_ID = @UserID";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@Name", medicine.Name);
                    command.Parameters.AddWithValue("@Type", medicine.Type);
                    command.Parameters.AddWithValue("@Time", medicine.Time);
                    command.Parameters.AddWithValue("@MedicineID", medicine.MedicineID);
                    command.Parameters.AddWithValue("@UserID", UserId);

                     connection.Open();
                     rowsAffected = command.ExecuteNonQuery();

                }
            }
                    return rowsAffected > 0 ?true:false;
        }

        public List<MedicineModel> GetUserMedicines(int UserId)
        {
            List<MedicineModel> Medicines = new List<MedicineModel>();
            using (SqlConnection connection = new SqlConnection(_helper.GetconnectionString()))
            {
                string query = @"SELECT MedicineID, Name, Type, Time FROM Medicines WHERE User_ID = @UserID";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@UserID", UserId);


                    connection.Open();

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            Medicines.Add(new MedicineModel
                            {
                                MedicineID =(int)reader["MedicineID"],
                                Name = reader["Name"].ToString(),
                                Type = reader["Type"].ToString(),
                                Time = (TimeSpan)reader["Time"] 
                            });
                        }
                    }
                }
            }
                    return Medicines;

        }



    

    }
}
