namespace XUCore.Template.Mediator2.Applaction;

/// <summary>
/// 登录操作
/// </summary>
[ApiExplorerSettings(GroupName = ApiGroup.Admin)]
[DynamicWebApi(Name = "Login")]
public class UserLoginOutCommand : Command<Result<bool>>, IDynamicWebApi
{
    /// <summary>
    /// 退出登录
    /// </summary>
    /// <param name="mediator"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public async Task PostOutAsync([FromServices] IMediator mediator, CancellationToken cancellationToken)
        => await mediator.Send(new UserLoginOutCommand(), cancellationToken);
}

public class UserLoginOutCommandValidator : CommandValidator<UserLoginOutCommand>
{
    public UserLoginOutCommandValidator()
    {

    }
}

public class UserLoginOutCommandHandler : CommandHandler<UserLoginOutCommand, Result<bool>>
{
    protected readonly FreeSqlUnitOfWorkManager db;
    protected readonly IUserInfo user;

    public UserLoginOutCommandHandler(FreeSqlUnitOfWorkManager db, IMediator mediator, IMapper mapper, IUserInfo user) : base(mediator, mapper)
    {
        this.db = db;
        this.user = user;
    }

    public override async Task<Result<bool>> Handle(UserLoginOutCommand request, CancellationToken cancellationToken)
    {
        user.RemoveToken();

        await Task.CompletedTask;

        return RestFull.Success(data: true);
    }
}