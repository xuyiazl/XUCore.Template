using XUCore.Template.Razor.Applaction.User;

namespace XUCore.Template.Razor.Web.Components.Admin
{
    [Authorize]
    [NoAccessControl]
    public class _LayoutAdminHeaderViewComponent : ViewComponent
    {
        private readonly IUserInfo userInfo;
        private readonly IUserAppService userAppService;
        public _LayoutAdminHeaderViewComponent(IUserInfo userInfo, IUserAppService userAppService)
        {
            this.userInfo = userInfo;
            this.userAppService = userAppService;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var user = await userAppService.GetAsync(userInfo.GetId<long>(), CancellationToken.None);

            dynamic res = new ExpandoObject();

            res.Admin = user;

            return View(res);
        }
    }
}
