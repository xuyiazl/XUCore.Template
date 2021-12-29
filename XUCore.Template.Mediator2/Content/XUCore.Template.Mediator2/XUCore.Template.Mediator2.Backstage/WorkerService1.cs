namespace XUCore.Template.Mediator2.Backstage;

/// <summary>
/// WorkerService
/// </summary>
public class WorkerService1 : IHostedService
{
    private readonly ILogger<WorkerService1> _logger;
    /// <summary>
    /// WorkerService
    /// </summary>
    /// <param name="serviceProvider"></param>
    public WorkerService1(IServiceProvider serviceProvider)
    {
        _logger = serviceProvider.GetRequiredService<ILogger<WorkerService1>>();
    }
    /// <summary>
    /// 启动服务
    /// </summary>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public async Task StartAsync(CancellationToken cancellationToken)
    {
        _logger.LogInformation("Worker start at: {time}", DateTimeOffset.Now);

        await Task.CompletedTask;
    }
    /// <summary>
    /// 停止服务
    /// </summary>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public async Task StopAsync(CancellationToken cancellationToken)
    {
        _logger.LogInformation("Worker stop at: {time}", DateTimeOffset.Now);

        await Task.CompletedTask;
    }
}
