namespace XUCore.Template.FreeSql.Backstage
{
    public class WorkerService2 : BackgroundService
    {
        private readonly ILogger<WorkerService2> _logger;

        public WorkerService2(IServiceProvider serviceProvider)
        {
            _logger = serviceProvider.GetRequiredService<ILogger<WorkerService2>>();
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                _logger.LogInformation("Worker running at: {time}", DateTimeOffset.Now);
                await Task.Delay(1000, stoppingToken);
            }
        }
    }
}
