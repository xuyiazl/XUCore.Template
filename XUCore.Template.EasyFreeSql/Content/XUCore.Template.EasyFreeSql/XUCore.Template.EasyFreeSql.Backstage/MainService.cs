namespace XUCore.Template.EasyFreeSql.Backstage
{
    /// <summary>
    /// HostdService
    /// </summary>
    public class MainService : IHostedService
    {
        /// <summary>
        /// HostdService
        /// </summary>
        /// <param name="serviceProvider"></param>
        public MainService(IServiceProvider serviceProvider)
        {

        }
        /// <summary>
        /// 启动服务
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task StartAsync(CancellationToken cancellationToken)
        {
            await Task.CompletedTask;
        }
        /// <summary>
        /// 停止服务
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task StopAsync(CancellationToken cancellationToken)
        {
            await Task.CompletedTask;
        }
    }
}
