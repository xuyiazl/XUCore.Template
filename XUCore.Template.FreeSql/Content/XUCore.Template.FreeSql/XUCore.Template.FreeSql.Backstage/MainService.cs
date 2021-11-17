namespace XUCore.Template.FreeSql.Backstage
{
    public class MainService : IHostedService
    {
        private readonly ILogger logger;
        private readonly IServiceProvider serviceProvider;
        public MainService(ILogger<MainService> logger, IServiceProvider serviceProvider)
        {
            this.logger = logger;
            this.serviceProvider = serviceProvider;
        }

        public async Task StartAsync(CancellationToken cancellationToken)
        {

        }

        public async Task StopAsync(CancellationToken cancellationToken)
        {

        }
    }
}
