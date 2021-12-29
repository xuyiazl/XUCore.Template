namespace XUCore.Template.Mediator2.Domain;

/// <summary>
/// 密码修改命令
/// </summary>
public class UserUpdatePasswordCommand : Command<Result<int>>
{
    /// <summary>
    /// id
    /// </summary>
    [Required]
    public long Id { get; set; }
    /// <summary>
    /// 旧密码
    /// </summary>
    [Required]
    public string OldPassword { get; set; }
    /// <summary>
    /// 新密码
    /// </summary>
    [Required]
    public string NewPassword { get; set; }
}

internal class UserUpdatePasswordCommandValidator : CommandValidator<UserUpdatePasswordCommand>
{
    public UserUpdatePasswordCommandValidator()
    {
        RuleFor(x => x.Id).NotEmpty().WithName("Id不可为空");

        RuleFor(x => x.OldPassword).NotEmpty().MaximumLength(30).WithName("旧密码");
        RuleFor(x => x.NewPassword).NotEmpty().MaximumLength(30).WithName("新密码").NotEqual(c => c.OldPassword).WithName("新密码不能和旧密码相同");
    }
}

internal class UserUpdatePasswordCommandHandler : CommandHandler<UserUpdatePasswordCommand, Result<int>>
{
    protected readonly FreeSqlUnitOfWorkManager db;
    protected readonly IUserInfo user;

    public UserUpdatePasswordCommandHandler(FreeSqlUnitOfWorkManager db, IMediator mediator, IMapper mapper, IUserInfo user) : base(mediator, mapper)
    {
        this.db = db;
        this.user = user;
    }

    public override async Task<Result<int>> Handle(UserUpdatePasswordCommand request, CancellationToken cancellationToken)
    {
        var repo = db.Orm.GetRepository<UserEntity>();

        var entity = await repo.Select.WhereDynamic(request.Id).ToOneAsync<UserEntity>(cancellationToken);

        request.NewPassword = Encrypt.Md5By32(request.NewPassword);
        request.OldPassword = Encrypt.Md5By32(request.OldPassword);

        if (!entity.Password.Equals(request.OldPassword))
            Failure.Error("旧密码错误");

        var res = await db.Orm
            .Update<UserEntity>(request.Id)
            .Set(c => new UserEntity { Password = request.NewPassword })
            .ExecuteAffrowsAsync(cancellationToken);

        if (res > 0)
            return RestFull.Success(data: res);
        else
            return RestFull.Fail(data: res);
    }
}