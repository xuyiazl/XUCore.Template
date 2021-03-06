namespace XUCore.Template.Mediator2.Applaction;

/// <summary>
/// 用户管理
/// </summary>
[ApiExplorerSettings(GroupName = ApiGroup.Admin)]
[DynamicWebApi(Name = "User")]
public class UserCreateCommand : Command<Result<long>>, IMapFrom<UserEntity>, IDynamicWebApi
{
    /// <summary>
    /// 账号
    /// </summary>
    [Required]
    public string UserName { get; set; }
    /// <summary>
    /// 手机号码
    /// </summary>
    [Required]
    public string Mobile { get; set; }
    /// <summary>
    /// 密码
    /// </summary>
    [Required]
    public string Password { get; set; }
    /// <summary>
    /// 名字
    /// </summary>
    [Required]
    public string Name { get; set; }
    /// <summary>
    /// 位置
    /// </summary>
    public string Location { get; set; }
    /// <summary>
    /// 职位
    /// </summary>
    public string Position { get; set; }
    /// <summary>
    /// 公司
    /// </summary>
    public string Company { get; set; }
    /// <summary>
    /// 角色id集合
    /// </summary>
    public long[] Roles { get; set; }

    [NonDynamicMethod]
    public void Mapping(Profile profile) =>
            profile.CreateMap<UserCreateCommand, UserEntity>()
                .ForMember(c => c.Password, c => c.MapFrom(s => Encrypt.Md5By32(s.Password)))
                .ForMember(c => c.LoginCount, c => c.MapFrom(s => 0))
                .ForMember(c => c.LoginLastIp, c => c.MapFrom(s => ""))
                .ForMember(c => c.Picture, c => c.MapFrom(s => ""))
                .ForMember(c => c.Enabled, c => c.MapFrom(s => true))
            ;

    /// <summary>
    /// 创建用户账号
    /// </summary>
    /// <param name="mediator"></param>
    /// <param name="request"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public async Task<Result<long>> CreateAsync([FromServices] IMediator mediator, [Required][FromBody] UserCreateCommand request, CancellationToken cancellationToken = default)
        => await mediator.Send(request, cancellationToken);
}

public class UserCreateCommandValidator : CommandValidator<UserCreateCommand>
{
    public UserCreateCommandValidator()
    {
        RuleFor(x => x.UserName).NotEmpty().MaximumLength(20).WithName("账号")
            .MustAsync(async (account, cancel) =>
            {
                var res = await Web.GetRequiredService<IMediator>()
                .Send(new UserAnyByAccountCommand
                {
                    AccountMode = AccountMode.UserName,
                    Account = account,
                    NotId = 0
                }, cancel);

                return !res.Data;
            })
            .WithMessage(c => $"该账号已存在。");

        RuleFor(x => x.Mobile)
            .NotEmpty()
            .Matches("^[1][3-9]\\d{9}$").WithMessage("手机号格式不正确。")
            .MustAsync(async (account, cancel) =>
            {
                var res = await Web.GetRequiredService<IMediator>()
                .Send(new UserAnyByAccountCommand
                {
                    AccountMode = AccountMode.Mobile,
                    Account = account,
                    NotId = 0
                }, cancel);

                return !res.Data;
            })
            .WithMessage(c => $"该手机号码已存在。");

        RuleFor(x => x.Password).NotEmpty().MaximumLength(30).WithName("密码");
        RuleFor(x => x.Name).NotEmpty().MaximumLength(20).WithName("名字");
    }
}

public class UserCreateCommandHandler : CommandHandler<UserCreateCommand, Result<long>>
{
    protected readonly FreeSqlUnitOfWorkManager db;
    protected readonly IUserInfo user;

    public UserCreateCommandHandler(FreeSqlUnitOfWorkManager db, IMediator mediator, IMapper mapper, IUserInfo user) : base(mediator, mapper)
    {
        this.db = db;
        this.user = user;
    }

    public override async Task<Result<long>> Handle(UserCreateCommand request, CancellationToken cancellationToken)
    {
        var repo = db.Orm.GetRepository<UserEntity>();

        var entity = mapper.Map<UserCreateCommand, UserEntity>(request);

        //角色操作
        if (request.Roles != null && request.Roles.Length > 0)
        {
            //转换角色对象 并写入
            entity.UserRoles = Array.ConvertAll(request.Roles, roleid => new UserRoleEntity
            {
                RoleId = roleid,
                UserId = entity.Id
            });
        }

        var res = await repo.InsertAsync(entity, cancellationToken);

        if (res != null)
        {
            await db.Orm.Insert<UserRoleEntity>(entity.UserRoles).ExecuteAffrowsAsync(cancellationToken);

            await bus.PublishEvent(new UserCreateEvent(entity.Id, entity), cancellationToken);

            return RestFull.Success(data: res.Id);
        }

        return RestFull.Fail(data: 0L);
    }
}
