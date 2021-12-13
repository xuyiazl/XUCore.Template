using XUCore.Template.Razor2.Applaction.Login;
using XUCore.Template.Razor2.Core;

namespace XUCore.Template.Razor2.Applaction
{
    /// <summary>
    /// CSHTML控件权限验证
    /// </summary>
    public class ControlAccessStrategy : IControlAccessStrategy
    {
        private readonly ILoginAppService loginAppService;
        private readonly IUserInfo userInfo;

        public ControlAccessStrategy(ILoginAppService loginAppService, IUserInfo userInfo)
        {
            this.loginAppService = loginAppService;
            this.userInfo = userInfo;
        }

        public bool IsControlCanAccess(string accessKey) => loginAppService.GetPermissionExistsAsync(userInfo.UserId, accessKey).Result;
    }
}
