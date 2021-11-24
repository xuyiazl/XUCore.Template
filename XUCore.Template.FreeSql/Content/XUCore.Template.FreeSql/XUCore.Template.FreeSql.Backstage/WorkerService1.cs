namespace XUCore.Template.FreeSql.Backstage
{
    public class WorkerService1 : IHostedService
    {
        private readonly ILogger<WorkerService1> _logger;

        public WorkerService1(IServiceProvider serviceProvider)
        {
            _logger = serviceProvider.GetRequiredService<ILogger<WorkerService1>>();
        }

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("Worker start at: {time}", DateTimeOffset.Now);

            await Task.CompletedTask;
        }

        public async Task StopAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("Worker stop at: {time}", DateTimeOffset.Now);

            await Task.CompletedTask;
        }
    }
}
