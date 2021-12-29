namespace XUCore.Template.Mediator2.Backstage.Quartz;

/// <summary>
/// Quartz Job Demo
/// </summary>
public class JobDemo2 : EasyQuartzJob, IJob
{
    private readonly IConfiguration _configuration;

    private readonly ILogger<JobDemo2> _logger;
    public JobDemo2(IServiceProvider serviceProvider)
    {
        _logger = serviceProvider.GetRequiredService<ILogger<JobDemo2>>();
        _configuration = serviceProvider.GetRequiredService<IConfiguration>();
    }

    public override string Cron => _configuration["Test2JobCron"];

    public async Task Execute(IJobExecutionContext context)
    {
        _logger.LogInformation("{0} JobDemo2", DateTimeOffset.Now);

        await Task.CompletedTask;
    }
}