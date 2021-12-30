namespace XUCore.Template.Mediator2.Applaction;

/// <summary>
/// 用户管理
/// </summary>
[ApiExplorerSettings(GroupName = ApiGroup.Admin)]
[DynamicWebApi(Name = "User")]
public class UserQueryByIdCommand : Command<Result<UserDto>>, IDynamicWebApi
{
    /// <summary>
    /// Id
    /// </summary>
    [Required]
    public long Id { get; set; }

    /// <summary>
    /// 获取账号信息
    /// </summary>
    /// <param name="mediator"></param>
    /// <param name="id"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    [HttpGet("{id}")]
    public async Task<Result<UserDto>> GetAsync([FromServices] IMediator mediator, [Required] long id, CancellationToken cancellationToken = default)
        => await mediator.Send(new UserQueryByIdCommand { Id = id }, cancellationToken);
}

public class UserQueryByIdCommandValidator : CommandValidator<UserQueryByIdCommand>
{
    public UserQueryByIdCommandValidator()
    {
        RuleFor(x => x.Id).NotEmpty().WithName("Id不可为空");
    }
}

public class UserQueryByIdCommandHandler : CommandHandler<UserQueryByIdCommand, Result<UserDto>>
{
    protected readonly FreeSqlUnitOfWorkManager db;
    protected readonly IUserInfo user;

    public UserQueryByIdCommandHandler(FreeSqlUnitOfWorkManager db, IMediator mediator, IMapper mapper, IUserInfo user) : base(mediator, mapper)
    {
        this.db = db;
        this.user = user;
    }

    public override async Task<Result<UserDto>> Handle(UserQueryByIdCommand request, CancellationToken cancellationToken)
    {
        var repo = db.Orm.GetRepository<UserEntity>();

        var res = await repo.Select.WhereDynamic(request.Id).ToOneAsync<UserDto>(cancellationToken);

        return RestFull.Success(data: res);
    }
}