using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Data.SqlClient;
using SmartBite.DAL.Models;

namespace SmartBite.DAL.Authantication
{
    public class AuthanticationService : IAuthanticationService
    {
        private readonly dbHelper _helper;

        public AuthanticationService(dbHelper helper)
        {
            _helper = helper;
        }

        public bool DeleteProfile(int ID)
        {
            using (SqlConnection connection = new SqlConnection(_helper.GetconnectionString()))
            {
                string Query = "delete from Users where UserID=@id";
                using(SqlCommand command=new SqlCommand(Query, connection))
                {
                    command.Parameters.AddWithValue("@id", ID);

                    connection.Open();

                    int rowsAffected= command.ExecuteNonQuery();
                    return rowsAffected>0?true:false;
                }
            }
        }

        public bool FindUser(string Email)
        {
            int Finded = 0;

            using (SqlConnection connection=new SqlConnection(_helper.GetconnectionString()))
            {
                string Query = "Select count(*) from Users where Email = @email";
                using(SqlCommand command = new SqlCommand(Query, connection))
                {
                    command.Parameters.AddWithValue("@email", Email);
                    connection.Open();

                    Finded = (int)command.ExecuteScalar();

                }
            }

            return Finded > 0 ? true : false;
        }

        public string GetName(int UserID)
        {
            string Name = "";

            using (SqlConnection connection = new SqlConnection(_helper.GetconnectionString()))
            {
                string query = "SELECT Name FROM Users WHERE UserID = @UserID;";
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@UserID", UserID);
                    connection.Open();

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            Name = reader["Name"]?.ToString() ?? "";
                        }
                    }
                }
            }

            return Name;
        }

        public TinyPersonModel Login(string email, string password)
        {
            using (SqlConnection connection = new SqlConnection(_helper.GetconnectionString()))
            {
                string query = "SELECT UserId, Name, Email FROM Users WHERE Email = @email AND Password = @password";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@email", email);
                    command.Parameters.AddWithValue("@password", password);

                    connection.Open();
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            return new TinyPersonModel
                            {
                                UserID = reader.GetInt32(0),
                                Name = reader.GetString(1),
                                Email = reader.GetString(2)
                            };
                        }
                    }
                }
            }

            
            return null!;
        }


        public TinyPersonModel SignUP(string name, string email, string password)
        {
            using (SqlConnection connection = new SqlConnection(_helper.GetconnectionString()))
            {
                string query = @"INSERT INTO Users (Name, Email, Password)
                                 OUTPUT INSERTED.UserId, INSERTED.Name, INSERTED.Email
                                 VALUES (@name, @email, @password)";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@name", name);
                    command.Parameters.AddWithValue("@email", email);
                    command.Parameters.AddWithValue("@password", password);

                    connection.Open();

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            return new TinyPersonModel
                            {
                                UserID = reader.GetInt32(0),
                                Name = reader.GetString(1),
                                Email = reader.GetString(2)
                            };
                        }
                    }
                }
            }

            return null!; // Insertion failed
        }



        public bool UpdatePassword(string Email, string NewPassword)
        {
            using(SqlConnection connection=new SqlConnection(_helper.GetconnectionString()))
            {
                string Query = "update Users set Password=@pass where Email=@email";
                using(SqlCommand command = new SqlCommand(Query, connection))
                {
                    command.Parameters.AddWithValue("@pass", NewPassword);
                    command.Parameters.AddWithValue("@email", Email);

                    connection.Open ();
                    int rowsAffected=command.ExecuteNonQuery();
                    return rowsAffected>0?true : false;
                }
            }
        }



    }
}
