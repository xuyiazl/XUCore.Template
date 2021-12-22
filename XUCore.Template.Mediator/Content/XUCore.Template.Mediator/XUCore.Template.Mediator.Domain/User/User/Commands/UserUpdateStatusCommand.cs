namespace XUCore.Template.Mediator.Domain;

/// <summary>
/// 角色更新状态命令
/// </summary>
public class UserUpdateStatusCommand : Command<Result<int>>
{
    /// <summary>
    /// id
    /// </summary>
    [Required]
    public long[] Ids { get; set; }
    /// <summary>
    /// 修改的值
    /// </summary>
    public bool Enabled { get; set; }
}


internal class UserUpdateStatusCommandValidator : CommandValidator<UserUpdateStatusCommand>
{
    public UserUpdateStatusCommandValidator()
    {
        RuleFor(x => x.Ids).NotEmpty().WithName("Id不可为空");
    }
}

internal class UserUpdateStatusCommandHandler : CommandHandler<UserUpdateStatusCommand, Result<int>>
{
    protected readonly FreeSqlUnitOfWorkManager db;
    protected readonly IUserInfo user;

    public UserUpdateStatusCommandHandler(FreeSqlUnitOfWorkManager db, IMediator mediator, IMapper mapper, IUserInfo user) : base(mediator, mapper)
    {
        this.db = db;
        this.user = user;
    }

    public override async Task<Result<int>> Handle(UserUpdateStatusCommand request, CancellationToken cancellationToken)
    {
        var repo = db.Orm.GetRepository<UserEntity>();

        var list = await repo.Select.Where(c => request.Ids.Contains(c.Id)).ToListAsync<UserEntity>(cancellationToken);

        list.ForEach(c => c.Enabled = request.Enabled);

        var res = await repo.UpdateAsync(list, cancellationToken);

        if (res > 0)
            return RestFull.Success(data: res);
        else
            return RestFull.Fail(data: res);
    }
}