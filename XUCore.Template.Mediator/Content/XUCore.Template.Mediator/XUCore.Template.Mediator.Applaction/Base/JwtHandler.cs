namespace XUCore.Template.Mediator.Applaction;

/// <summary>
/// JWT 授权自定义处理程序
/// </summary>
public class JwtHandler : AppAuthorizeHandler
{
    private readonly IServiceProvider serviceProvider;
    private readonly IUserInfo user;
    private readonly IMediator mediator;
    /// <summary>
    /// JWT 授权自定义处理程序
    /// </summary>ws
    /// <param name="serviceProvider"></param>
    public JwtHandler(IServiceProvider serviceProvider)
    {
        this.serviceProvider = serviceProvider;
        this.user = serviceProvider.GetRequiredService<IUserInfo>();
        this.mediator = serviceProvider.GetRequiredService<IMediator>();
    }
    /// <summary>
    /// 重写 Handler 添加自动刷新收取逻辑
    /// </summary>
    /// <param name="context"></param>
    /// <returns></returns>
    public override async Task HandleAsync(AuthorizationHandlerContext context)
    {
        string url = context.GetCurrentHttpContext().Request.GetRefererUrlAddress();
        if (url.Contains("xx.com")) //if (url.Contains("localhost"))
        {
            //var isAuthenticated = context.User.Identity.IsAuthenticated;
            var pendingRequirements = context.PendingRequirements;
            foreach (var requirement in pendingRequirements)
            {
                // 授权成功
                context.Succeed(requirement);
            }
        }
        else
        {

            // 验证登录保存的token，如果不一致则是被其他人踢掉，或者退出登录了，需要重新登录
            var token = JWTEncryption.GetJwtBearerToken(context.GetCurrentHttpContext());

            if (!user.VaildToken(token))
                context.Fail();


            // 自动刷新 token
            if (JWTEncryption.AutoRefreshToken(context, context.GetCurrentHttpContext()))
                await AuthorizeHandleAsync(context);
            else
                context.Fail();    // 授权失败
        }
    }

    /// <summary>
    /// 验证管道
    /// </summary>
    /// <param name="context"></param>
    /// <param name="httpContext"></param>
    /// <returns></returns>
    public override async Task<bool> PipelineAsync(AuthorizationHandlerContext context, DefaultHttpContext httpContext)
    {
        // 获取权限特性
        var securityDefineAttribute = httpContext.GetMetadata<SecurityDefineAttribute>();
        if (securityDefineAttribute == null) return true;

        // 检查授权
        var res = await mediator.Send(new PermissionExistCommand { UserId = user.GetId<long>() }, CancellationToken.None);

        return res.Data;
    }
}
