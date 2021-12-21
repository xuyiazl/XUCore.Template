namespace XUCore.Template.Mediator.Applaction;

/// <summary>
/// Mediator 消息中介发布请求和通知
/// </summary>
public class MediatorMemoryHandler : IMediatorHandler
{
    private readonly IMediator mediator;

    public MediatorMemoryHandler(IMediator mediator)
    {
        this.mediator = mediator;
    }

    /// <summary>
    /// 发布事件通知
    /// </summary>
    /// <typeparam name="TNotification"></typeparam>
    /// <param name="event">通知事件</param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public Task PublishEvent<TNotification>(TNotification @event, CancellationToken cancellationToken = default(CancellationToken)) where TNotification : Event
    {
        // MediatR中介者模式中的第二种方法，发布/订阅模式
        return mediator.Publish(@event);
    }

    /// <summary>
    /// 发送命令请求
    /// </summary>
    /// <typeparam name="TResponse"></typeparam>
    /// <param name="command">命令</param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public Task<TResponse> SendCommand<TResponse>(Command<TResponse> command, CancellationToken cancellationToken = default(CancellationToken))
    {
        return mediator.Send(command, cancellationToken);
    }
}

