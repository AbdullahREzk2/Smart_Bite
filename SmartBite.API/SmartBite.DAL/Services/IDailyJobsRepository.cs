using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartBite.DAL.Services
{
    public interface IDailyJobsRepository
    {
        Task RunUpdateDailyCalorieAsync(CancellationToken cancellationToken);
        Task RunUpdateDailyLimitsAsync(CancellationToken cancellationToken);
    }
}
