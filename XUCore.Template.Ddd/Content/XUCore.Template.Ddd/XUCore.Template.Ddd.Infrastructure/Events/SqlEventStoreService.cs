using XUCore.Template.Ddd.Domain.Core;
using XUCore.Template.Ddd.Domain.Core.Events;

namespace XUCore.Template.Ddd.Infrastructure.Events
{
    /// <summary>
    /// 事件存储服务类
    /// </summary>
    public class SqlEventStoreService : IEventStoreService
    {
        private readonly IDefaultDbRepository db;
        private readonly IServiceProvider serviceProvider;
        public SqlEventStoreService(IServiceProvider serviceProvider, IDefaultDbRepository db)
        {
            this.db = db;
            this.serviceProvider = serviceProvider;
        }

        /// <summary>
        /// 保存事件模型统一方法
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="theEvent"></param>
        public void Save<T>(T theEvent) where T : Event
        {
            var serializedData = theEvent.ToJson();

            var auth = serviceProvider.GetService<IUserInfo>();

            var storedEvent = new StoredEvent(
                theEvent,
                serializedData,
                auth.Id.SafeString());

            db.Add(storedEvent);
        }
    }
}
