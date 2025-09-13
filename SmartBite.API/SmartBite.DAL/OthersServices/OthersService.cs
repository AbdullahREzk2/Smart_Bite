using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Data.SqlClient;

namespace SmartBite.DAL.OthersServices
{
    public class OthersService : IOthersService
    {
        private readonly dbHelper _helper;

        public OthersService(dbHelper helper)
        {
            _helper = helper;
        }
        public bool AddRate(int UserID, int Rate)
        {
            int rowsAffected;

            using (SqlConnection connection = new SqlConnection(_helper.GetconnectionString()))
            {
                string query = "UPDATE Users SET Rate = @Rate WHERE UserID = @UserID";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@Rate", Rate);
                    command.Parameters.AddWithValue("@UserID", UserID);

                    connection.Open();
                    rowsAffected = command.ExecuteNonQuery();
                }
            }

            return rowsAffected > 0;
        }






    }
}
