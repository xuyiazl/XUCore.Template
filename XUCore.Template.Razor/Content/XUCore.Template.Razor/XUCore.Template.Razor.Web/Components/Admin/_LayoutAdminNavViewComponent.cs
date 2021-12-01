using XUCore.Template.Razor.Applaction.Login;
using XUCore.Template.Razor.Applaction.User;

namespace XUCore.Template.Razor.Web.Components.Admin
{
    [Authorize]
    [NoAccessControl]
    public class _LayoutAdminNavViewComponent : ViewComponent
    {
        private readonly IUserAppService userAppService;
        private readonly ILoginAppService loginAppService;
        private readonly IUserInfo userInfo;

        public _LayoutAdminNavViewComponent(IUserAppService userAppService, IUserInfo userInfo, ILoginAppService loginAppService)
        {
            this.userAppService = userAppService;
            this.loginAppService = loginAppService;
            this.userInfo = userInfo;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var menus = await loginAppService.GetPermissionMenusAsync(userInfo.UserId, CancellationToken.None);

            var topMenus = await loginAppService.GetPermissionMenuExpressAsync(userInfo.UserId, CancellationToken.None);

            var user = await userAppService.GetByIdAsync(userInfo.UserId, CancellationToken.None);

            dynamic res = new ExpandoObject();

            res.Menus = menus;
            res.TopMenus = topMenus;
            res.User = user;

            return View(res);
        }

    }
}
