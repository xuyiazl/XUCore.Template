using XUCore.Template.Razor.Applaction.Login;

namespace XUCore.Template.Razor.Web.Pages
{
    public class IndexModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;
        private readonly ILoginAppService loginAppService;

        public IndexModel(ILogger<IndexModel> logger, ILoginAppService loginAppService)
        {
            _logger = logger;
            this.loginAppService = loginAppService;
        }

        public async void OnGetAsync(CancellationToken cancellationToken)
        {
            var command = new DbService.User.User.UserLoginCommand
            {
                Account = "admin",
                Password = "123456"
            };
            var r = await loginAppService.LoginAsync(command, cancellationToken);
        }
    }
}