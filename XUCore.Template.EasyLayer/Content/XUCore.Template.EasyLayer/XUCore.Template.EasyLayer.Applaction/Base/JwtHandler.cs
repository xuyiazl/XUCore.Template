﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Threading;
using System.Threading.Tasks;
using XUCore.Helpers;
using XUCore.NetCore.Authorization;
using XUCore.NetCore.Authorization.JwtBearer;
using XUCore.Template.EasyLayer.Core;
using XUCore.Template.EasyLayer.DbService.Admin.Permission;

namespace XUCore.Template.EasyLayer.Applaction
{
    /// <summary>
    /// JWT 授权自定义处理程序
    /// </summary>
    public class JwtHandler : AppAuthorizeHandler
    {
        private readonly IServiceProvider serviceProvider;
        private readonly IUserInfo user;
        private readonly IPermissionService permissionService;
        public JwtHandler(IServiceProvider serviceProvider)
        {
            this.serviceProvider = serviceProvider;
            this.user = serviceProvider.GetService<IUserInfo>();
            this.permissionService = serviceProvider.GetService<IPermissionService>();
        }
        /// <summary>
        /// 重写 Handler 添加自动刷新收取逻辑
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public override async Task HandleAsync(AuthorizationHandlerContext context)
        {
            string url = context.GetCurrentHttpContext().Request.GetRefererUrlAddress();
            if (url.Contains("xx.com")) //if (url.Contains("localhost"))
            {
                var isAuthenticated = context.User.Identity.IsAuthenticated;
                var pendingRequirements = context.PendingRequirements;
                foreach (var requirement in pendingRequirements)
                {
                    // 授权成功
                    context.Succeed(requirement);
                }
            }
            else
            {
#if !DEBUG
                // 验证登录保存的token，如果不一致则是被其他人踢掉，或者退出登录了，需要重新登录
                var token = JWTEncryption.GetJwtBearerToken(context.GetCurrentHttpContext());

                if (!authService.VaildLoginToken(token))
                    context.Fail();
#endif

                // 自动刷新 token
                if (JWTEncryption.AutoRefreshToken(context, context.GetCurrentHttpContext()))
                    await AuthorizeHandleAsync(context);
                else
                    context.Fail();    // 授权失败
            }
        }

        /// <summary>
        /// 验证管道
        /// </summary>
        /// <param name="context"></param>
        /// <param name="httpContext"></param>
        /// <returns></returns>
        public override async Task<bool> PipelineAsync(AuthorizationHandlerContext context, DefaultHttpContext httpContext)
        {
            // 获取权限特性
            var securityDefineAttribute = httpContext.GetMetadata<SecurityDefineAttribute>();
            if (securityDefineAttribute == null) return true;

            // 检查授权
            return await permissionService.ExistsAsync(user.GetId<long>(), securityDefineAttribute.ResourceId, CancellationToken.None);
        }
    }
}
