namespace XUCore.Template.Mediator2.Backstage.Quartz;

/// <summary>
/// Quartz Job Demo
/// </summary>
[TriggerCron("0/5 * * * * ? *")]
public class JobDemo1 : IJob
{
    private readonly ILogger<JobDemo1> _logger;
    public JobDemo1(IServiceProvider serviceProvider)
    {
        _logger = serviceProvider.GetRequiredService<ILogger<JobDemo1>>();
    }

    public async Task Execute(IJobExecutionContext context)
    {
        _logger.LogInformation("{0} JobDemo1", DateTimeOffset.Now);

        await Task.CompletedTask;
    }
}