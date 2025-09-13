using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Build.Framework;
using NuGet.Common;
using SmartBite.DAL.Services;
using Microsoft.Extensions.Logging;


namespace SmartBite.BAL.Services
{
    public class DailyJobsManager: IDailyJobsManager
    {

        private readonly IDailyJobsRepository _repository;
        private readonly ILogger<DailyJobsManager> _logger;

        public DailyJobsManager(IDailyJobsRepository repository, ILogger<DailyJobsManager> logger)
        {
            _repository = repository;
            _logger = logger;
        }


        public async Task RunDailyJobsAsync(CancellationToken cancellationToken)
        {
            await _repository.RunUpdateDailyCalorieAsync(cancellationToken);
            _logger.LogInformation("✅ sp_UpdateDailyCalorie executed at {time}", DateTime.Now);

            await _repository.RunUpdateDailyLimitsAsync(cancellationToken);
            _logger.LogInformation("✅ sp_UpdateDailyLimits executed at {time}", DateTime.Now);
        }


    }
}
