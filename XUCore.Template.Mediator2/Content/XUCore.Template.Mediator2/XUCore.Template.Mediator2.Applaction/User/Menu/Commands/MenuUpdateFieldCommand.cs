namespace XUCore.Template.Mediator2.Applaction;

/// <summary>
/// 菜单管理
/// </summary>
[ApiExplorerSettings(GroupName = ApiGroup.Admin)]
[DynamicWebApi(Name = "Menu")]
public class MenuUpdateFieldCommand : Command<Result<int>>, IDynamicWebApi
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

    /// <summary>
    /// 更新导航指定字段内容
    /// </summary>
    /// <param name="mediator"></param>
    /// <param name="request"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public async Task<Result<int>> UpdateFieldAsync([FromServices] IMediator mediator, [Required][FromBody] MenuUpdateFieldCommand request, CancellationToken cancellationToken = default)
        => await mediator.Send(request, cancellationToken);
}

public class MenuUpdateFieldCommandValidator : CommandValidator<MenuUpdateFieldCommand>
{
    public MenuUpdateFieldCommandValidator()
    {
        RuleFor(x => x.Id).NotEmpty().WithName("Id不可为空");
        RuleFor(x => x.Field).NotEmpty().MaximumLength(20).WithName("指定的字段名");
    }
}

public class MenuUpdateFieldCommandHandler : CommandHandler<MenuUpdateFieldCommand, Result<int>>
{
    protected readonly FreeSqlUnitOfWorkManager db;
    protected readonly IUserInfo user;

    public MenuUpdateFieldCommandHandler(FreeSqlUnitOfWorkManager db, IMediator mediator, IMapper mapper, IUserInfo user) : base(mediator, mapper)
    {
        this.db = db;
        this.user = user;
    }

    public override async Task<Result<int>> Handle(MenuUpdateFieldCommand request, CancellationToken cancellationToken)
    {
        var repo = db.Orm.GetRepository<MenuEntity>();

        var entity = await repo.Select.WhereDynamic(request.Id).ToOneAsync(cancellationToken);

        if (entity.IsNull())
            Failure.Error("没有找到该记录");

        switch (request.Field.ToLower())
        {
            case "name":
                entity.Name = request.Value;
                break;
            case "icon":
                entity.Icon = request.Value;
                break;
            case "url":
                entity.Url = request.Value;
                break;
            case "onlycode":
                entity.OnlyCode = request.Value;
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
