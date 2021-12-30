namespace XUCore.Template.Mediator.Domain;

/// <summary>
/// 登录命令
/// </summary>
public class UserLoginCommand : Command<Result<LoginTokenDto>>
{
    /// <summary>
    /// 登录账号
    /// </summary>
    [Required]
    public string Account { get; set; }
    /// <summary>
    /// 密码
    /// </summary>
    [Required]
    public string Password { get; set; }

}

public class UserLoginCommandValidator : CommandValidator<UserLoginCommand>
{
    public UserLoginCommandValidator()
    {
        RuleFor(x => x.Account).NotEmpty().WithName("登录账号");
        RuleFor(x => x.Password).NotEmpty().WithName("登录密码");
    }
}

public class UserLoginCommandHandler : CommandHandler<UserLoginCommand, Result<LoginTokenDto>>
{
    protected readonly FreeSqlUnitOfWorkManager db;
    protected readonly IUserInfo user;

    public UserLoginCommandHandler(FreeSqlUnitOfWorkManager db, IMediator mediator, IMapper mapper, IUserInfo user) : base(mediator, mapper)
    {
        this.db = db;
        this.user = user;
    }

    public override async Task<Result<LoginTokenDto>> Handle(UserLoginCommand request, CancellationToken cancellationToken)
    {
        var repo = db.Orm.GetRepository<UserEntity>();

        var userEntity = default(UserEntity);

        request.Password = Encrypt.Md5By32(request.Password);

        var loginWay = "";

        if (!Valid.IsMobileNumberSimple(request.Account))
        {
            userEntity = await repo.Select.Where(c => c.UserName.Equals(request.Account)).ToOneAsync(cancellationToken);
            if (userEntity == null)
                Failure.Error("账号不存在");

            loginWay = "UserName";
        }
        else
        {
            userEntity = await repo.Select.Where(c => c.Mobile.Equals(request.Account)).ToOneAsync(cancellationToken);
            if (userEntity == null)
                Failure.Error("手机号码不存在");

            loginWay = "Mobile";
        }

        if (!userEntity.Password.Equals(request.Password))
            Failure.Error("密码错误");
        if (userEntity.Enabled == false)
            Failure.Error("您的帐号禁止登录,请与用户联系!");


        userEntity.LoginCount += 1;
        userEntity.LoginLastTime = DateTime.Now;
        userEntity.LoginLastIp = Web.IP;

        await db.Orm.Update<UserEntity>(userEntity.Id).Set(c => new UserEntity()
        {
            LoginCount = userEntity.LoginCount,
            LoginLastTime = userEntity.LoginLastTime,
            LoginLastIp = userEntity.LoginLastIp
        }).ExecuteAffrowsAsync(cancellationToken);

        await db.Orm.Insert(new UserLoginRecordEntity
        {
            UserId = user.GetId<long>(),
            LoginIp = userEntity.LoginLastIp,
            LoginWay = loginWay
        }).ExecuteAffrowsAsync(cancellationToken);

        // 生成 token
        var accessToken = JWTEncryption.Encrypt(new Dictionary<string, object>
                {
                    { ClaimAttributes.UserId , userEntity.Id },
                    { ClaimAttributes.UserName ,userEntity.UserName }
                });

        // 生成 刷新token
        var refreshToken = JWTEncryption.GenerateRefreshToken(accessToken);

        if (Web.HttpContext != null)
        {
            // 设置 Swagger 自动登录
            Web.HttpContext.SigninToSwagger(accessToken);
            // 设置刷新 token
            Web.HttpContext.Response.Headers["x-access-token"] = refreshToken;
        }

        user.SetToken(userEntity.Id.ToString(), accessToken);

        return RestFull.Success(data: new LoginTokenDto
        {
            Token = accessToken
        });
    }
}