namespace XUCore.Template.Mediator.Domain;

/// <summary>
/// 退出登录命令
/// </summary>
public class UserLoginOutCommand : Command<Result<bool>>
{

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

    public UserLoginOutCommandHandler(FreeSqlUnitOfWorkManager db, IMediatorHandler bus, IMapper mapper, IUserInfo user) : base(bus, mapper)
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