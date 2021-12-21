namespace XUCore.Template.Mediator.Backstage.Quartz;

/// <summary>
/// Quartz Job Demo
/// </summary>
[JobIgnore]
public class JobDemo4 : IJob
{
    private readonly ILogger<JobDemo4> _logger;
    private readonly IJobManager _jobManager;

    public JobDemo4(IServiceProvider serviceProvider)
    {
        _logger = serviceProvider.GetRequiredService<ILogger<JobDemo4>>();
        _jobManager = serviceProvider.GetRequiredService<IJobManager>();
    }

    public async Task Execute(IJobExecutionContext context)
    {
        JobDataMap dataMap = context.MergedJobDataMap;

        string id = dataMap.GetString("Id").SafeString();

        _logger.LogInformation("{0} JobDemo4 , 参数 {1}", DateTimeOffset.Now, id);

        await _jobManager.RemoveAsync<JobDemo4>(id);
    }
}