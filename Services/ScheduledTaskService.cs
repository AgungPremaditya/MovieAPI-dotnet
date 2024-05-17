using Cronos;

namespace MovieAPI_dotnet.Services
{
    public class ScheduledTaskService : IHostedService, IDisposable
    {
        private readonly ILogger<ScheduledTaskService> _logger;
        private Timer? _timer;
        private CronExpression _cronExpression;
        private DateTimeOffset? _nextRun;

        public ScheduledTaskService(ILogger<ScheduledTaskService> logger)
        {
            _logger = logger;
            _cronExpression = CronExpression.Parse("@every_second");
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("Scheduled Task Service is starting.");

            ScheduleTask();

            return Task.CompletedTask;
        }
        private void ScheduleTask()
        {
            _nextRun = _cronExpression.GetNextOccurrence(DateTimeOffset.Now, TimeZoneInfo.Local);

            if (_nextRun.HasValue)
            {
                var delay = _nextRun.Value - DateTimeOffset.Now;
                _timer = new Timer(DoWork, null, delay, TimeSpan.FromSeconds(1));
            }
        }
        private void DoWork(object state)
        {
            _logger.LogInformation("Scheduled Task is running at: {time}", DateTimeOffset.Now);

            // Add your scheduled task logic here
            if (DateTime.Now >= DateTime.Today.AddHours(9).AddMinutes(48))
            {
                StopAsync(new CancellationToken());
            }
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("Scheduled Task Service is stopping.");

            _timer?.Change(Timeout.Infinite, 0);

            return Task.CompletedTask;
        }

        public void Dispose()
        {
            _timer?.Dispose();
        }
    }
}
