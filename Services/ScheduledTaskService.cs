using Cronos;
using MovieAPI_dotnet.Repositories.Movies;

namespace MovieAPI_dotnet.Services
{
    public class ScheduledTaskService : IHostedService, IDisposable
    {
        private readonly ILogger<ScheduledTaskService> _logger;
        private Timer? _timer;
        private CronExpression _cronExpression;
        private DateTimeOffset? _nextRun;
        private readonly IServiceProvider _serviceProvider;

        public ScheduledTaskService(ILogger<ScheduledTaskService> logger, IServiceProvider serviceProvider)
        {
            _logger = logger;
            _cronExpression = CronExpression.Parse("@every_second");
            _serviceProvider = serviceProvider;
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
        private async void DoWork(object state)
        {
            _logger.LogInformation("Scheduled Task is running at: {time}", DateTimeOffset.Now);

            // Add your scheduled task logic here
            using (var scope = _serviceProvider.CreateScope())
            {
                var movieRepo = scope.ServiceProvider.GetRequiredService<IMovieRepository>();
                var movies = await movieRepo.GetMoviesAsync();

                foreach (var movie in movies)
                {
                    if (DateTime.Now >= movie.AiringDate)
                        _logger.LogInformation("Airing");
                    else
                        _logger.LogInformation("Not Airing");
                }
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
