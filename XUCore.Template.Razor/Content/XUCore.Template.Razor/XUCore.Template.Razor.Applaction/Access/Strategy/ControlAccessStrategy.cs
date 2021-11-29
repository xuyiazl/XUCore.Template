using XUCore.Template.Razor.Applaction.Login;
using XUCore.Template.Razor.Core;

namespace XUCore.Template.Razor.Applaction
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

        public bool IsControlCanAccess(string accessKey) => loginAppService.GetPermissionExistsAsync(userInfo.GetId<long>(), accessKey).Result;
    }
}
