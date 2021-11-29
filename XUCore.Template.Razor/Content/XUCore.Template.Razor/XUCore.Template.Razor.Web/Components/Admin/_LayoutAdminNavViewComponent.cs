
using XUCore.Template.Razor.Applaction.Login;
using XUCore.Template.Razor.Applaction.User;

namespace XUCore.Template.Razor.Web.Components.Admin
{
    [Authorize]
    [NoAccessControl]
    public class _LayoutAdminNavViewComponent : ViewComponent
    {
        private readonly IUserInfo userInfo;
        private readonly IUserAppService userAppService;
        private readonly ILoginAppService loginAppService;

        public _LayoutAdminNavViewComponent(IUserInfo userInfo, IUserAppService userAppService, ILoginAppService loginAppService)
        {
            this.userInfo = userInfo;
            this.userAppService = userAppService;
            this.loginAppService = loginAppService;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var menus = await loginAppService.GetPermissionMenusAsync(userInfo.GetId<long>(), CancellationToken.None);

            var topMenus = await loginAppService.GetPermissionMenuExpressAsync(userInfo.GetId<long>(), CancellationToken.None);

            var admin = await userAppService.GetAsync(userInfo.GetId<long>(), CancellationToken.None);

            dynamic res = new ExpandoObject();

            res.Menus = menus;
            res.TopMenus = topMenus;
            res.Admin = admin;

            return View(res);
        }

    }
}
