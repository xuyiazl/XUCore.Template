namespace XUCore.Template.Mediator2.Backstage;

/// <summary>
/// WorkerService
/// </summary>
public class WorkerService2 : BackgroundService
{
    private readonly ILogger<WorkerService2> _logger;
    /// <summary>
    /// WorkerService
    /// </summary>
    /// <param name="serviceProvider"></param>
    public WorkerService2(IServiceProvider serviceProvider)
    {
        _logger = serviceProvider.GetRequiredService<ILogger<WorkerService2>>();
    }
    /// <summary>
    /// 执行任务
    /// </summary>
    /// <param name="stoppingToken"></param>
    /// <returns></returns>
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            _logger.LogInformation("Worker running at: {time}", DateTimeOffset.Now);
            await Task.Delay(1000, stoppingToken);
        }
    }
}
