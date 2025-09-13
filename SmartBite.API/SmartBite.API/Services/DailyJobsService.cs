using System;
using SmartBite.BAL.Services;

public class DailyJobsService : BackgroundService
{
    private readonly IServiceScopeFactory _scopeFactory;
    private readonly ILogger<DailyJobsService> _logger;
    private readonly IConfiguration _configuration;

    public DailyJobsService(IServiceScopeFactory scopeFactory, IConfiguration configuration, ILogger<DailyJobsService> logger)
    {
        _scopeFactory = scopeFactory;
        _configuration = configuration;
        _logger = logger;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        var runHour = _configuration.GetValue<int>("DailyJobSettings:RunHour");
        var runMinute = _configuration.GetValue<int>("DailyJobSettings:RunMinute");
        var egyptTimeZone = TimeZoneInfo.FindSystemTimeZoneById("Egypt Standard Time");

        while (!stoppingToken.IsCancellationRequested)
        {
            var nowUtc = DateTime.UtcNow;
            var nowEgypt = TimeZoneInfo.ConvertTimeFromUtc(nowUtc, egyptTimeZone);

            var scheduledEgyptToday = nowEgypt.Date.AddHours(runHour).AddMinutes(runMinute);
            var scheduledUtc = TimeZoneInfo.ConvertTimeToUtc(scheduledEgyptToday, egyptTimeZone);

            var delay = scheduledUtc - nowUtc;

            if (delay < TimeSpan.Zero)
            {
                // missed today’s time — schedule for tomorrow
                scheduledEgyptToday = scheduledEgyptToday.AddDays(1);
                scheduledUtc = TimeZoneInfo.ConvertTimeToUtc(scheduledEgyptToday, egyptTimeZone);
                delay = scheduledUtc - nowUtc;
            }

            _logger.LogInformation("⏰ Next job scheduled at: {time} Egypt Time (in {delay})", scheduledEgyptToday, delay);

            try
            {
                await Task.Delay(delay, stoppingToken);
            }
            catch (TaskCanceledException)
            {
                break;
            }

            try
            {
                using var scope = _scopeFactory.CreateScope();
                var manager = scope.ServiceProvider.GetRequiredService<IDailyJobsManager>();
                await manager.RunDailyJobsAsync(stoppingToken);
                _logger.LogInformation("✅ Job executed successfully at: {time} (Egypt time)", scheduledEgyptToday);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "❌ Error executing job");
            }
        }
    }

}