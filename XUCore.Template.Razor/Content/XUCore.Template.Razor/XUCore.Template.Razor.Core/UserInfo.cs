namespace XUCore.Template.Razor.Core
{
    public interface IUserInfo : IUser, IScoped
    {
        bool IsAuthenticated { get; }
        Task LoginAsync(long id, string userName, string name, CancellationToken cancellationToken);
        Task LoginOutAsync(CancellationToken cancellationToken);
    }

    public class UserInfo : User, IUserInfo
    {
        private const string userId = "_userid";
        private const string userName = "_username";
        private const string nickName = "_nickname";
        private readonly string scheme = CookieAuthenticationDefaults.AuthenticationScheme;
        private readonly IHttpContextAccessor httpContextAccessor;
        public UserInfo(IServiceProvider serviceProvider) : base(serviceProvider)
        {
            httpContextAccessor = serviceProvider.GetRequiredService<IHttpContextAccessor>();
        }

        public async Task LoginAsync(long id, string userName, string name, CancellationToken cancellationToken)
        {
            var claims = new List<Claim> {
                new Claim(userId, id.ToString()),
                new Claim(userName,userName),
                new Claim(nickName, name)
            };

            await httpContextAccessor.HttpContext.SignInAsync(scheme,
                    new ClaimsPrincipal(
                        new ClaimsIdentity(claims, scheme)
                    ),
                    new AuthenticationProperties
                    {
                        IsPersistent = true,
                        AllowRefresh = true,
                        ExpiresUtc = DateTimeOffset.UtcNow.AddDays(7)
                    }
                );
        }

        public async Task LoginOutAsync(CancellationToken cancellationToken)
        {
            if (IsAuthenticated)
                await httpContextAccessor.HttpContext?.SignOutAsync(scheme);
        }

        public bool IsAuthenticated => Identity.IsAuthenticated;

        public override string Id => Identity?.GetValue<string>(userId);

        public override string UserName => Identity?.GetValue<string>(userName);

        public override string NickName => Identity?.GetValue<string>(nickName);
    }
}
