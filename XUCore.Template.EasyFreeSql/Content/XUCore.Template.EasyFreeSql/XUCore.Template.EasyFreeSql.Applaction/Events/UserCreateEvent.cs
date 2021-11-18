using XUCore.Template.EasyFreeSql.Persistence.Entities.User;

namespace XUCore.Template.EasyFreeSql.Applaction.Events
{
    /// <summary>
    /// 用户账号创建事件
    /// </summary>
    public class UserCreateEvent : INotification
    {
        /// <summary>
        /// 用户Id
        /// </summary>
        public long Id { get; set; }
        /// <summary>
        /// 用户实体
        /// </summary>
        public UserEntity User { get; set; }
        /// <summary>
        /// 用户账号创建事件
        /// </summary>
        /// <param name="id"></param>
        /// <param name="user"></param>
        public UserCreateEvent(long id, UserEntity user)
        {
            Id = id;
            User = user;
        }

        /// <summary>
        /// 事件通知操作
        /// </summary>
        public class Handler : INotificationHandler<UserCreateEvent>
        {
            /// <summary>
            /// 事件通知操作
            /// </summary>
            public Handler()
            {

            }
            /// <summary>
            /// 接受消息处理创建后的业务
            /// </summary>
            /// <param name="notification"></param>
            /// <param name="cancellationToken"></param>
            /// <returns></returns>
            public async Task Handle(UserCreateEvent notification, CancellationToken cancellationToken)
            {
                await Task.CompletedTask;
            }
        }
    }
}
