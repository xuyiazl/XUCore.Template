using XUCore.Template.Razor2.Applaction.User;
using XUCore.Template.Razor2.Core;

namespace XUCore.Template.Razor2.Web.Components.Admin
{
    [Authorize]
    [NoAccessControl]
    public class _LayoutAdminHeaderViewComponent : ViewComponent
    {
        private readonly IUserAppService userAppService;
        private readonly IUserInfo userInfo;
        public _LayoutAdminHeaderViewComponent(IUserAppService userAppService, IUserInfo userInfo)
        {
            this.userAppService = userAppService;
            this.userInfo = userInfo;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var user = await userAppService.GetByIdAsync(userInfo.UserId, CancellationToken.None);

            dynamic res = new ExpandoObject();

            res.User = user;

            return View(res);
        }
    }
}
