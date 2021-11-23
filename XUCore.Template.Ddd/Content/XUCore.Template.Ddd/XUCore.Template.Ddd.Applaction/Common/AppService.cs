namespace XUCore.Template.Ddd.Applaction.Common
{
    public class AppService
    {
        // 中介者 总线
        public readonly IMediatorHandler bus;

        public AppService(IMediatorHandler bus)
        {
            this.bus ??= bus;
        }
    }
}
