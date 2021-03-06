namespace XUCore.Template.Mediator2.Applaction;

/// <summary>
/// 用户管理
/// </summary>
[ApiExplorerSettings(GroupName = ApiGroup.Admin)]
[DynamicWebApi(Name = "User")]
public class UserUpdateInfoCommand : Command<Result<int>>, IMapFrom<UserEntity>, IDynamicWebApi
{
    /// <summary>
    /// id
    /// </summary>
    [Required]
    public long Id { get; set; }
    /// <summary>
    /// 名字
    /// </summary>
    [Required]
    public string Name { get; set; }
    /// <summary>
    /// 位置
    /// </summary>
    [Required]
    public string Location { get; set; }
    /// <summary>
    /// 职位
    /// </summary>
    [Required]
    public string Position { get; set; }
    /// <summary>
    /// 公司
    /// </summary>
    [Required]
    public string Company { get; set; }

    [NonDynamicMethod]
    public void Mapping(Profile profile) =>
            profile.CreateMap<UserUpdateInfoCommand, UserEntity>()
                .ForMember(c => c.Location, c => c.MapFrom(s => s.Location.SafeString()))
                .ForMember(c => c.Position, c => c.MapFrom(s => s.Position.SafeString()))
                .ForMember(c => c.Company, c => c.MapFrom(s => s.Company.SafeString()))
            ;

    /// <summary>
    /// 更新账号信息
    /// </summary>
    /// <param name="mediator"></param>
    /// <param name="request"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public async Task<Result<int>> UpdateAsync([FromServices] IMediator mediator, [Required][FromBody] UserUpdateInfoCommand request, CancellationToken cancellationToken = default)
        => await mediator.Send(request, cancellationToken);
}

public class UserUpdateInfoCommandValidator : CommandValidator<UserUpdateInfoCommand>
{
    public UserUpdateInfoCommandValidator()
    {
        RuleFor(x => x.Id).NotEmpty().WithName("Id不可为空");

        RuleFor(x => x.Name).NotEmpty().MaximumLength(30).WithName("名字");
        RuleFor(x => x.Company).NotEmpty().MaximumLength(30).WithName("公司");
        RuleFor(x => x.Location).NotEmpty().MaximumLength(30).WithName("位置");
        RuleFor(x => x.Position).NotEmpty().MaximumLength(20).WithName("职位");
    }
}

public class UserUpdateInfoCommandHandler : CommandHandler<UserUpdateInfoCommand, Result<int>>
{
    protected readonly FreeSqlUnitOfWorkManager db;
    protected readonly IUserInfo user;

    public UserUpdateInfoCommandHandler(FreeSqlUnitOfWorkManager db, IMediator mediator, IMapper mapper, IUserInfo user) : base(mediator, mapper)
    {
        this.db = db;
        this.user = user;
    }

    public override async Task<Result<int>> Handle(UserUpdateInfoCommand request, CancellationToken cancellationToken)
    {
        var repo = db.Orm.GetRepository<UserEntity>();

        var entity = await repo.Select.WhereDynamic(request.Id).ToOneAsync<UserEntity>(cancellationToken);

        if (entity == null)
            return RestFull.Fail(data: 0);

        entity = mapper.Map(request, entity);

        var res = await repo.UpdateAsync(entity, cancellationToken);


        if (res > 0)
            return RestFull.Success(data: res);
        else
            return RestFull.Fail(data: res);
    }
}