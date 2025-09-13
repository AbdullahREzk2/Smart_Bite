using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartBite.BAL.Services
{
    public interface IDailyJobsManager
    {
        Task RunDailyJobsAsync(CancellationToken cancellationToken);

    }
}
