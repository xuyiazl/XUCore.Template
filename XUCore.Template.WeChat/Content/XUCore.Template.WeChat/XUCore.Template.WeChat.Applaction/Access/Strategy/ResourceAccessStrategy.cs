
using XUCore.Template.WeChat.Applaction.Login;
using XUCore.Template.WeChat.Core;

namespace XUCore.Template.WeChat.Applaction
{
    /// <summary>
    /// 资源权限验证，比如Controller控制器的API方法，或者View
    /// </summary>
    public class ResourceAccessStrategy : IResourceAccessStrategy
    {
        private readonly ILoginAppService loginAppService;
        private readonly IUserInfo userInfo;

        public ResourceAccessStrategy(ILoginAppService loginAppService, IUserInfo userInfo)
        {
            this.loginAppService = loginAppService;
            this.userInfo = userInfo;
        }

        public bool IsCanAccess(string accessKey) => loginAppService.GetPermissionExistsAsync(userInfo.UserId, accessKey).Result;

        public string StrategyName { get; } = "Global";

        public IActionResult DisallowedCommonResult => new ContentResult
        {
            Content = "您没有权限进行该操作!!!",
            ContentType = "text/html;charset=utf-8",
            StatusCode = 403
        };

        public IActionResult DisallowedAjaxResult => new Result(StateCode.Fail, "403", "您没有权限进行该操作!!!");
    }
}

