namespace XUCore.Template.EasyFreeSql.Backstage.Quartz
{
    /// <summary>
    /// Quartz Job Demo
    /// </summary>
    [StartNow]
    [TriggerCron("0 0/5 * * * ?")]
    public class JobDemo3 : IJob
    {
        private readonly ILogger<JobDemo3> _logger;
        private readonly IJobManager _jobManager;

        public JobDemo3(IServiceProvider serviceProvider)
        {
            _logger = serviceProvider.GetRequiredService<ILogger<JobDemo3>>();
            _jobManager = serviceProvider.GetRequiredService<IJobManager>();
        }

        public async Task Execute(IJobExecutionContext context)
        {
            _logger.LogInformation("{0} JobDemo3", DateTimeOffset.Now);

            await _jobManager.AddAsync<JobDemo4>(CronCommon.SecondInterval(2), "111111");
        }
    }
}
