using XUCore.Template.Ddd.Applaction.Common;
using XUCore.Template.Ddd.Domain.Auth.Permission;
using XUCore.Template.Ddd.Domain.Core;
using XUCore.Template.Ddd.Domain.User.LoginRecord;
using XUCore.Template.Ddd.Domain.User.User;

namespace XUCore.Template.Ddd.Applaction.AppServices.Login
{
    /// <summary>
    /// 用户登录接口
    /// </summary>
    [ApiExplorerSettings(GroupName = ApiGroup.User)]
    [DynamicWebApi]
    public class LoginAppService : AppService, ILoginAppService, IDynamicWebApi
    {
        private readonly IUserInfo userInfo;

        public LoginAppService(IMediatorHandler bus, IUserInfo userInfo) : base(bus)
        {
            this.userInfo = userInfo;
        }

        #region [ 登录 ]
        /// <summary>
        /// 创建初始账号
        /// </summary>
        /// <remarks>
        /// 初始账号密码：
        ///     <para>username : admin</para>
        ///     <para>password : admin</para>
        /// </remarks>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [HttpPost]
        [AllowAnonymous]
        public async Task<Result<int>> CreateInitAccountAsync(CancellationToken cancellationToken = default)
        {
            var command = new UserCreateCommand
            {
                UserName = "admin",
                Password = "admin",
                Company = "",
                Location = "",
                Mobile = "13500000000",
                Name = "admin",
                Position = ""
            };
            var res = await bus.SendCommand(command, cancellationToken);

            if (res > 0)
                return RestFull.Success(data: res);
            else
                return RestFull.Fail(data: res);
        }

        /// <summary>
        /// 用户登录
        /// </summary>
        /// <param name="command"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [HttpPost("/api/[controller]")]
        [AllowAnonymous]
        public async Task<Result<LoginTokenDto>> Login([FromBody] UserLoginCommand command, CancellationToken cancellationToken)
        {
            var user = await bus.SendCommand(command, cancellationToken);

            // 生成 token
            var accessToken = JWTEncryption.Encrypt(new Dictionary<string, object>
            {
                { ClaimAttributes.UserId , user.Id },
                { ClaimAttributes.UserName  ,user.UserName }
            });

            // 生成 刷新token
            var refreshToken = JWTEncryption.GenerateRefreshToken(accessToken);

            userInfo.SetToken(user.Id, accessToken);

            // 设置 Swagger 自动登录
            Web.HttpContext.SigninToSwagger(accessToken);
            // 设置刷新 token
            Web.HttpContext.Response.Headers["x-access-token"] = refreshToken;

            return RestFull.Success(data: new LoginTokenDto
            {
                Token = accessToken
            });
        }
        /// <summary>
        /// 验证Token
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<Result<string>> VerifyTokenAsync(CancellationToken cancellationToken)
        {
            await Task.CompletedTask;

            return RestFull.Success(SubCode.Success, data: new { userInfo.Id, userInfo.UserName }.ToJson());
        }
        /// <summary>
        /// 退出登录
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [HttpPost("/api/[controller]/Out")]
        public async Task LoginOutAsync(CancellationToken cancellationToken)
        {
            userInfo.RemoveToken();

            await Task.CompletedTask;
        }

        #endregion

        #region [ 登录后的权限获取 ]

        /// <summary>
        /// 查询是否有权限
        /// </summary>
        /// <param name="command"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [HttpGet("Exists")]
        public async Task<Result<bool>> GetPermissionAsync([FromQuery] PermissionQueryExists command, CancellationToken cancellationToken = default)
        {
            var res = await bus.SendCommand(command, cancellationToken);

            return RestFull.Success(data: res);
        }
        /// <summary>
        /// 查询权限导航
        /// </summary>
        /// <param name="command"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [HttpGet("Menu")]
        public async Task<Result<IList<PermissionMenuTreeDto>>> GetPermissionAsync([FromQuery] PermissionQueryMenu command, CancellationToken cancellationToken = default)
        {
            var res = await bus.SendCommand(command, cancellationToken);

            return RestFull.Success(data: res);
        }
        /// <summary>
        /// 查询权限导航（快捷导航）
        /// </summary>
        /// <param name="command"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [HttpGet("Express")]
        public async Task<Result<IList<PermissionMenuDto>>> GetPermissionAsync([FromQuery] PermissionQueryMenuExpress command, CancellationToken cancellationToken = default)
        {
            var res = await bus.SendCommand(command, cancellationToken);

            return RestFull.Success(data: res);
        }

        #endregion

        #region [ 登录记录 ]

        /// <summary>
        /// 获取最近登录记录
        /// </summary>
        /// <param name="command"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [HttpGet("List")]
        public async Task<Result<IList<UserLoginRecordDto>>> GetRecordAsync([FromQuery] UserLoginRecordQueryList command, CancellationToken cancellationToken = default)
        {
            var res = await bus.SendCommand(command, cancellationToken);

            return RestFull.Success(data: res);
        }
        /// <summary>
        /// 获取所有登录记录分页
        /// </summary>
        /// <param name="command"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [HttpGet("Page")]
        public async Task<Result<PagedModel<UserLoginRecordDto>>> GetRecordAsync([FromQuery] UserLoginRecordQueryPaged command, CancellationToken cancellationToken = default)
        {
            var res = await bus.SendCommand(command, cancellationToken);

            return RestFull.Success(data: res);
        }

        #endregion
    }
}
