using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;

namespace SmartBite.DAL.Services
{
    public class DailyJobsRepository: IDailyJobsRepository
    {
        private readonly dbHelper _helper;

        public DailyJobsRepository(dbHelper helper)
        {
            _helper = helper;
        }
        

        public async Task RunUpdateDailyCalorieAsync(CancellationToken cancellationToken)
        {
            using (var connection = new SqlConnection(_helper.GetconnectionString()))
            {
                await connection.OpenAsync(cancellationToken);
                using (var cmd = new SqlCommand("sp_UpdateDailyCalorie", connection))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    await cmd.ExecuteNonQueryAsync(cancellationToken);
                }
            }
        }

        public async Task RunUpdateDailyLimitsAsync(CancellationToken cancellationToken)
        {
            using (var connection = new SqlConnection(_helper.GetconnectionString()))
            {
                await connection.OpenAsync(cancellationToken);
                using (var cmd = new SqlCommand("sp_UpdateDailyLimits", connection))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    await cmd.ExecuteNonQueryAsync(cancellationToken);
                }
            }
        }



    }
}
