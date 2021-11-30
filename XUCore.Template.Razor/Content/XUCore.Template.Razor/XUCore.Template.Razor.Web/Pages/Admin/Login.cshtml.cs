
using XUCore.Template.Razor.Applaction.Login;
using XUCore.Template.Razor.DbService.User.User;

namespace XUCore.Template.Razor.Web.Pages.Admin
{
    public class LoginModel : PageModel
    {
        private readonly ILoginAppService loginAppService;
        private readonly IUserInfo userInfo;
        public LoginModel(ILoginAppService loginAppService, IUserInfo userInfo)
        {
            this.loginAppService = loginAppService;
            this.userInfo = userInfo;
        }

        [BindProperty]
        public string Account { get; set; }

        [BindProperty]
        public string Password { get; set; }

        [BindProperty(SupportsGet = true)]
        public string ReturnUrl { get; set; }

        public IActionResult OnGet()
        {
            if (userInfo.IsAuthenticated)
                return RedirectToPage("/admin/index");

            return Page();
        }

        public async Task<IActionResult> OnPostLoginAsync(CancellationToken cancellationToken)
        {
            try
            {
                var command = new UserLoginCommand { Account = Account, Password = Password };

                if (!command.IsVaild())
                {
                    ModelState.AddModelError("All", command.GetErrors(""));

                    return Page();
                }

                await loginAppService.LoginAsync(command, cancellationToken);

                if (string.IsNullOrEmpty(ReturnUrl))
                    return RedirectToPage("/admin/index");

                return Redirect(ReturnUrl);
            }
            catch (ValidationException ex)
            {
                var message = ex.Failures.Select(c => c.Value.Join("")).Join("");

                ModelState.AddModelError("All", message);

                return Page();
            }
        }

        public async Task<IActionResult> OnGetLoginOutAsync(CancellationToken cancellationToken)
        {
            await loginAppService.LoginOutAsync(cancellationToken);

            return RedirectToPage("/admin/login");
        }
    }
}