using XUCore.Extensions;

namespace XUCore.Template.EasyFreeSql.Backstage.Quartz
{
    /// <summary>
    /// 测试job
    /// </summary>
    [TriggerCron("0/5 * * * * ? *")]
    public class Test1Job : IJob
    {
        /// <summary>
        /// 执行job
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public Task Execute(IJobExecutionContext context)
        {
            Console.WriteLine($"{DateTime.Now} Test1Job");
            return Task.CompletedTask;
        }
    }

    /// <summary>
    /// 测试job
    /// </summary>
    public class Test2Job : EasyQuartzJob, IJob
    {
        private readonly IConfiguration _configuration;

        /// <summary>
        /// 测试job
        /// </summary>
        public Test2Job(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        /// <summary>
        /// 执行规则
        /// </summary>
        public override string Cron => _configuration["Test2JobCron"];

        /// <summary>
        /// 执行job
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public Task Execute(IJobExecutionContext context)
        {
            Console.WriteLine($"{DateTime.Now} Test2Job");
            return Task.CompletedTask;
        }
    }

    /// <summary>
    /// 测试job
    /// </summary>
    [StartNow]
    [TriggerCron("0 0/5 * * * ?")]
    public class Test3Job : IJob
    {
        private readonly IJobManager _jobManager;

        /// <summary>
        /// 测试job
        /// </summary>
        public Test3Job(IJobManager jobManager)
        {
            _jobManager = jobManager;
        }

        /// <summary>
        /// 执行job
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public async Task Execute(IJobExecutionContext context)
        {
            Console.WriteLine($"{DateTime.Now} Test3Job");
            await _jobManager.AddAsync(typeof(Test4Job), CronCommon.SecondInterval(2), "111111");
        }
    }

    /// <summary>
    /// 测试job
    /// </summary>
    [JobIgnore]
    public class Test4Job : IJob
    {
        private readonly IJobManager _jobManager;

        /// <summary>
        /// 测试job
        /// </summary>
        public Test4Job(IJobManager jobManager)
        {
            _jobManager = jobManager;
        }

        /// <summary>
        /// 执行job
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public Task Execute(IJobExecutionContext context)
        {
            JobDataMap dataMap = context.MergedJobDataMap;

            string id = dataMap.GetString("Id").SafeString();

            Console.WriteLine($"{DateTime.Now} Test4Job,参数{id}");
            //await _jobManager.RemoveJobAsync(typeof(Test4Job), id);
            return Task.CompletedTask;
        }
    }
}
