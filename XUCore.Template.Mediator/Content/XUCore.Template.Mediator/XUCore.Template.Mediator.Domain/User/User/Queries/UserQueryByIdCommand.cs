namespace XUCore.Template.Mediator.Domain;

/// <summary>
/// 查询一条用户记录
/// </summary>
public class UserQueryByIdCommand : Command<Result<UserDto>>
{
    /// <summary>
    /// Id
    /// </summary>
    [Required]
    public long Id { get; set; }
}


internal class UserQueryByIdCommandValidator : CommandValidator<UserQueryByIdCommand>
{
    public UserQueryByIdCommandValidator()
    {
        RuleFor(x => x.Id).NotEmpty().WithName("Id不可为空");
    }
}

internal class UserQueryByIdCommandHandler : CommandHandler<UserQueryByIdCommand, Result<UserDto>>
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