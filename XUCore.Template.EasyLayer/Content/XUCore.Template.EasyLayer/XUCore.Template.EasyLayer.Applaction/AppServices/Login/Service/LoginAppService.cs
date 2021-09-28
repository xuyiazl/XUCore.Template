﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Threading;
using System.Threading.Tasks;
using XUCore.Ddd.Domain;
using XUCore.Helpers;
using XUCore.NetCore;
using XUCore.NetCore.Authorization.JwtBearer;
using XUCore.NetCore.DynamicWebApi;
using XUCore.NetCore.Swagger;
using XUCore.Serializer;
using XUCore.Template.EasyLayer.Core;
using XUCore.Template.EasyLayer.DbService.Admin.AdminUser;
using XUCore.Template.EasyLayer.DbService.Admin.Permission;

namespace XUCore.Template.EasyLayer.Applaction.Login
{
    /// <summary>
    /// 管理员登录接口
    /// </summary>
    [ApiExplorerSettings(GroupName = ApiGroup.Admin)]
    [DynamicWebApi]
    public class LoginAppService : ILoginAppService, IDynamicWebApi
    {
        private readonly IPermissionService permissionService;
        private readonly IAdminUserService adminUserService;
        private readonly IUserInfo user;

        public LoginAppService(IServiceProvider serviceProvider)
        {
            this.permissionService = serviceProvider.GetService<IPermissionService>();
            this.adminUserService = serviceProvider.GetService<IAdminUserService>();
            this.user = serviceProvider.GetService<IUserInfo>();
        }

        #region [ 登录 ]

        /// <summary>
        /// 管理员登录
        /// </summary>
        /// <param name="request"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [HttpPost("/api/[controller]")]
        [AllowAnonymous]
        public async Task<Result<LoginTokenDto>> LoginAsync([Required][FromBody] AdminUserLoginCommand request, CancellationToken cancellationToken)
        {
            var userDto = await adminUserService.LoginAsync(request, cancellationToken);

            // 生成 token
            var accessToken = JWTEncryption.Encrypt(new Dictionary<string, object>
            {
                { ClaimAttributes.UserId , userDto.Id },
                { ClaimAttributes.UserName ,userDto.UserName }
            });

            // 生成 刷新token
            var refreshToken = JWTEncryption.GenerateRefreshToken(accessToken);

            // 设置 Swagger 自动登录
            Web.HttpContext.SigninToSwagger(accessToken);
            // 设置刷新 token
            Web.HttpContext.Response.Headers["x-access-token"] = refreshToken;

            user.SetToken(userDto.Id.ToString(), accessToken);

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
            return RestFull.Success(data: new
            {
                user.Id,
                user.UserName
            }.ToJson());
        }
        /// <summary>
        /// 退出登录
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [HttpPost("/api/[controller]/Out")]
        public async Task LoginOutAsync(CancellationToken cancellationToken)
        {
            user.RemoveToken();

            await Task.CompletedTask;
        }

        #endregion

        #region [ 登录后的权限获取 ]

        /// <summary>
        /// 查询是否有权限
        /// </summary>
        /// <param name="adminId"></param>
        /// <param name="onlyCode"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [HttpGet("/api/[controller]/Permission/Exists")]
        public async Task<Result<bool>> GetPermissionExistsAsync([Required] long adminId, [Required] string onlyCode, CancellationToken cancellationToken = default)
        {
            var res = await permissionService.ExistsAsync(adminId, onlyCode, cancellationToken);

            return RestFull.Success(data: res);
        }
        /// <summary>
        /// 查询权限导航
        /// </summary>
        /// <param name="adminId"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [HttpGet("/api/[controller]/Permission/Menu")]
        public async Task<Result<IList<PermissionMenuTreeDto>>> GetPermissionMenusAsync([Required] long adminId, CancellationToken cancellationToken = default)
        {
            var res = await permissionService.GetMenusAsync(adminId, cancellationToken);

            return RestFull.Success(data: res);
        }
        /// <summary>
        /// 查询权限导航（快捷导航）
        /// </summary>
        /// <param name="adminId"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [HttpGet("/api/[controller]/Permission/Express")]
        public async Task<Result<IList<PermissionMenuDto>>> GetPermissionMenuExpressAsync([Required] long adminId, CancellationToken cancellationToken = default)
        {
            var res = await permissionService.GetMenuExpressAsync(adminId, cancellationToken);

            return RestFull.Success(data: res);
        }

        #endregion
    }
}
