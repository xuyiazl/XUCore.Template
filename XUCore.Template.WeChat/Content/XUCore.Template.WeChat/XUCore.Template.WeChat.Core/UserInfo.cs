namespace XUCore.Template.WeChat.Core
{
    public interface IUserInfo : IUser, IScoped
    {
        long UserId { get; }
        string Name { get; }
        bool IsAuthenticated { get; }
        Task LoginAsync(long id, string userName, string name, string nickName, CancellationToken cancellationToken);
        Task LoginOutAsync(CancellationToken cancellationToken);
    }

    public class UserInfo : User, IUserInfo
    {
        private const string _userId = "_user_id_";
        private const string _userName = "_user_username_";
        private const string _name = "_user_name_";
        private const string _nickName = "_user_nickName_";

        private readonly string scheme = CookieAuthenticationDefaults.AuthenticationScheme;

        private readonly IHttpContextAccessor httpContextAccessor;

        public UserInfo(IServiceProvider serviceProvider) : base(serviceProvider)
        {
            httpContextAccessor = serviceProvider.GetRequiredService<IHttpContextAccessor>();
        }

        public async Task LoginAsync(long id, string userName, string name, string nickName, CancellationToken cancellationToken)
        {
            var claims = new List<Claim> {
                new Claim(_userId, id.ToString()),
                new Claim(_userName,userName),
                new Claim(_name, name),
                new Claim(_nickName, nickName)
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

        public override string Id => Identity?.GetValue<string>(_userId);

        public long UserId => GetId<long>();

        public override string UserName => Identity?.GetValue<string>(_userName);

        public override string NickName => Identity?.GetValue<string>(_nickName);

        public string Name => Identity?.GetValue<string>(_name);


    }
}

