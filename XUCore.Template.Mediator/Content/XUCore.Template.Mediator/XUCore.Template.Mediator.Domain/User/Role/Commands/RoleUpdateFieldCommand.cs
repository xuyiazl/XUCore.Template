namespace XUCore.Template.Mediator.Domain;

/// <summary>
/// 角色更新指定字段命令
/// </summary>
public class RoleUpdateFieldCommand : Command<Result<int>>
{
    /// <summary>
    /// id
    /// </summary>
    [Required]
    public long Id { get; set; }
    /// <summary>
    /// 指定的字段名
    /// </summary>
    [Required]
    public string Field { get; set; }
    /// <summary>
    /// 修改的值
    /// </summary>
    public string Value { get; set; }
}

internal class RoleUpdateFieldCommandValidator : CommandValidator<RoleUpdateFieldCommand>
{
    public RoleUpdateFieldCommandValidator()
    {
        RuleFor(x => x.Id).NotEmpty().WithName("Id不可为空");
        RuleFor(x => x.Field).NotEmpty().MaximumLength(20).WithName("指定的字段名");
    }
}

internal class RoleUpdateFieldCommandHandler : CommandHandler<RoleUpdateFieldCommand, Result<int>>
{
    protected readonly FreeSqlUnitOfWorkManager db;
    protected readonly IUserInfo user;

    public RoleUpdateFieldCommandHandler(FreeSqlUnitOfWorkManager db, IMediatorHandler bus, IMapper mapper, IUserInfo user) : base(bus, mapper)
    {
        this.db = db;
        this.user = user;
    }

    public override async Task<Result<int>> Handle(RoleUpdateFieldCommand request, CancellationToken cancellationToken)
    {
        var repo = db.Orm.GetRepository<RoleEntity>();

        var entity = await repo.Select.WhereDynamic(request.Id).ToOneAsync(cancellationToken);

        if (entity.IsNull())
            Failure.Error("没有找到该记录");

        switch (request.Field.ToLower())
        {
            case "name":
                entity.Name = request.Value;
                break;
            case "sort":
                entity.Sort = request.Value.ToInt();
                break;
        }

        var res = await repo.UpdateAsync(entity, cancellationToken);

        if (res > 0)
            return RestFull.Success(data: res);
        else
            return RestFull.Fail(data: res);
    }
}