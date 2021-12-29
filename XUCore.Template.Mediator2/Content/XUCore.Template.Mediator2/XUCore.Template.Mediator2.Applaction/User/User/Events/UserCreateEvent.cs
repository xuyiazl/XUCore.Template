namespace XUCore.Template.Mediator2.Applaction;

/// <summary>
/// 用户账号创建事件
/// </summary>
public class UserCreateEvent : Event
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
}

/// <summary>
/// 事件通知操作
/// </summary>
internal class UserCreateEventHandler : NotificationEventHandler<UserCreateEvent>
{
    /// <summary>
    /// 事件通知操作
    /// </summary>
    public UserCreateEventHandler() { }
    /// <summary>
    /// 接受消息处理创建后的业务
    /// </summary>
    /// <param name="notification"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public override async Task Handle(UserCreateEvent notification, CancellationToken cancellationToken)
    {
        await Task.CompletedTask;
    }
}
