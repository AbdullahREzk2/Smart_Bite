using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Data.SqlClient;
using SmartBite.DAL.Models;

namespace SmartBite.DAL.Disease
{
    public class DiseaseService : IDiseaseService
    {
        private readonly dbHelper _helper;

        public DiseaseService(dbHelper helper)
        {
            _helper = helper;
        }


        public string GetUserDisease(int userId)
        {
            string disease = string.Empty;

            using (SqlConnection connection = new SqlConnection(_helper.GetconnectionString()))
            {

                string query = @"select Diseases.Name from 
                                 Users inner join UserDiseases on Users.UserID = UserDiseases.User_ID
                                 inner join Diseases on UserDiseases.Disease_ID = Diseases.DiseaseID
                                 where Users.UserID = @UserID;";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@UserID", userId);

                    connection.Open();

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.Read() && reader["Name"] != DBNull.Value)
                        {
                            disease = reader["Name"].ToString()!;
                        }
                    }

                }
            }

            return disease;
        }


        public bool AddUserDisease(int UserId, string DiseaseName)
        {
            int rowsAffected;

            using (SqlConnection connection = new SqlConnection(_helper.GetconnectionString()))
            {
                string query = @"INSERT INTO UserDiseases (User_ID, Disease_ID)
                                SELECT @UserID, DiseaseID
                                FROM Diseases
                                WHERE Name = @Disease;";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@UserID", UserId);
                    command.Parameters.AddWithValue("@Disease", DiseaseName);

                    connection.Open();

                    rowsAffected = command.ExecuteNonQuery();
                }
            }

            return rowsAffected > 0 ? true : false;
        }


        public bool UpdateUserDisease(int UserId, string NewDiseaseName)
        {
            int rowsAffected;

            using (SqlConnection connection = new SqlConnection(_helper.GetconnectionString()))
            {
                string query = @"UPDATE UserDiseases
                                SET Disease_ID = (SELECT DiseaseID FROM Diseases WHERE Name = @NewDiseaseName)
                                WHERE User_ID = @UserID;";


                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@UserID", UserId);
                    command.Parameters.AddWithValue("@NewDiseaseName", NewDiseaseName);

                    connection.Open();

                    rowsAffected = command.ExecuteNonQuery();
                }
            }

            return rowsAffected > 0 ? true : false;
        }


        public bool DeleteUserDisease(int UserId)
        {
            int rowsAffected;

            using (SqlConnection connection = new SqlConnection(_helper.GetconnectionString()))
            {
                string query = @"DELETE FROM UserDiseases
                                 WHERE User_ID = @UserID;";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@UserID", UserId);

                    connection.Open();
                    rowsAffected = command.ExecuteNonQuery();
                }
            }

            return rowsAffected > 0 ? true : false;
        }


        public bool IsUserHasDisease(int userId)
        {
            int count;

            using (SqlConnection connection = new SqlConnection(_helper.GetconnectionString()))
            {

                string query = @"SELECT COUNT(*) FROM UserDiseases WHERE User_ID = @UserID;";


                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@UserID", userId);

                    connection.Open();

                    count = (int)command.ExecuteScalar();
                }
            }

            return count > 0;
        }

        public bool UpdateUserInsolin(int UserID , int Value)
        {
            int RowsAffected;
            using (SqlConnection connection = new SqlConnection(_helper.GetconnectionString()))
            {
                string Query = "update Users set TakeInsolin = @Value where UserID = @UserID;";

                using (SqlCommand command=new SqlCommand(Query, connection))
                {
                    command.Parameters.AddWithValue("@UserID", UserID);
                    command.Parameters.AddWithValue("@Value", Value);

                    connection.Open();
                    RowsAffected = command.ExecuteNonQuery();

                }
            }
                    return RowsAffected > 0;
        }



    }
}
