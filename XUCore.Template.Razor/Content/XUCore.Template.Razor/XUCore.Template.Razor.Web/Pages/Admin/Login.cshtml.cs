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
        public string UserName { get; set; }
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
                await loginAppService.LoginAsync(new UserLoginCommand
                {
                    Account = UserName,
                    Password = Password
                }, cancellationToken);

                if (string.IsNullOrEmpty(ReturnUrl))
                    return RedirectToPage("/admin/index");

                return Redirect(ReturnUrl);
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("All", ex.Message);

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