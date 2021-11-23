namespace XUCore.Template.Ddd.Domain.Auth.Menu
{
    public class MenuDeleteEvent : Event
    {
        public string Id { get; set; }
        public MenuDeleteEvent(string id)
        {
            Id = id;
            AggregateId = id;
        }

        /// <summary>
        /// 事件通知操作
        /// </summary>
        public class Handler : NotificationEventHandler<MenuDeleteEvent>
        {
            public Handler()
            {
            }
            /// <summary>
            /// 接受消息处理删除后的业务
            /// </summary>
            /// <param name="notification"></param>
            /// <param name="cancellationToken"></param>
            /// <returns></returns>
            public override async Task Handle(MenuDeleteEvent notification, CancellationToken cancellationToken)
            {
                await Task.CompletedTask;

            }
        }
    }
}
