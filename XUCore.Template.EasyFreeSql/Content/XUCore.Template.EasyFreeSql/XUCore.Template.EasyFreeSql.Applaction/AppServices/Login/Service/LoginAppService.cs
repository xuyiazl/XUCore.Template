using AutoMapper;
using FreeSql;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Threading;
using System.Threading.Tasks;
using XUCore.Ddd.Domain;
using XUCore.Ddd.Domain.Exceptions;
using XUCore.Helpers;
using XUCore.NetCore;
using XUCore.NetCore.Authorization.JwtBearer;
using XUCore.NetCore.DynamicWebApi;
using XUCore.NetCore.FreeSql.Curd;
using XUCore.NetCore.Swagger;
using XUCore.Serializer;
using XUCore.Template.EasyFreeSql.Applaction.User.Permission;
using XUCore.Template.EasyFreeSql.Applaction.User.User;
using XUCore.Template.EasyFreeSql.Core;
using XUCore.Template.EasyFreeSql.Persistence.Entities.User;

namespace XUCore.Template.EasyFreeSql.Applaction.Login
{
    /// <summary>
    /// 用户登录接口
    /// </summary>
    [ApiExplorerSettings(GroupName = ApiGroup.Admin)]
    [DynamicWebApi]
    public class LoginAppService : ILoginAppService, IDynamicWebApi
    {
        protected readonly FreeSqlUnitOfWorkManager unitOfWork;
        protected readonly IBaseRepository<UserEntity> repo;
        protected readonly IMapper mapper;
        protected readonly IUserInfo user;
        protected readonly IPermissionService permissionService;
        public LoginAppService(IServiceProvider serviceProvider)
        {
            this.unitOfWork = serviceProvider.GetService<FreeSqlUnitOfWorkManager>();
            this.repo = unitOfWork.Orm.GetRepository<UserEntity>();
            this.mapper = serviceProvider.GetService<IMapper>();
            this.user = serviceProvider.GetService<IUserInfo>();
            this.permissionService = serviceProvider.GetService<IPermissionService>();
        }

        #region [ 登录 ]

        /// <summary>
        /// 用户登录
        /// </summary>
        /// <param name="request"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [AllowAnonymous]
        public async Task<Result<LoginTokenDto>> PostAsync([Required][FromBody] UserLoginCommand request, CancellationToken cancellationToken)
        {
            var userEntity = default(UserEntity);

            request.Password = Encrypt.Md5By32(request.Password);

            var loginWay = "";

            if (!Valid.IsMobileNumberSimple(request.Account))
            {
                userEntity = await repo.Select.Where(c => c.UserName.Equals(request.Account)).ToOneAsync(cancellationToken);
                if (userEntity == null)
                    Failure.Error("账号不存在");

                loginWay = "UserName";
            }
            else
            {
                userEntity = await repo.Select.Where(c => c.Mobile.Equals(request.Account)).ToOneAsync(cancellationToken);
                if (userEntity == null)
                    Failure.Error("手机号码不存在");

                loginWay = "Mobile";
            }

            if (!userEntity.Password.Equals(request.Password))
                Failure.Error("密码错误");
            if (userEntity.Enabled == false)
                Failure.Error("您的帐号禁止登录,请与用户联系!");


            userEntity.LoginCount += 1;
            userEntity.LoginLastTime = DateTime.Now;
            userEntity.LoginLastIp = Web.IP;

            await unitOfWork.Orm.Update<UserEntity>(userEntity.Id).Set(c => new UserEntity()
            {
                LoginCount = userEntity.LoginCount,
                LoginLastTime = userEntity.LoginLastTime,
                LoginLastIp = userEntity.LoginLastIp
            })
                .ExecuteAffrowsAsync(cancellationToken);

            await unitOfWork.Orm.Insert(new UserLoginRecordEntity
            {
                UserId = user.GetId<long>(),
                LoginIp = userEntity.LoginLastIp,
                LoginWay = loginWay
            }).ExecuteAffrowsAsync(cancellationToken);

            // 生成 token
            var accessToken = JWTEncryption.Encrypt(new Dictionary<string, object>
            {
                { ClaimAttributes.UserId , userEntity.Id },
                { ClaimAttributes.UserName ,userEntity.UserName }
            });

            // 生成 刷新token
            var refreshToken = JWTEncryption.GenerateRefreshToken(accessToken);

            if (Web.HttpContext != null)
            {
                // 设置 Swagger 自动登录
                Web.HttpContext.SigninToSwagger(accessToken);
                // 设置刷新 token
                Web.HttpContext.Response.Headers["x-access-token"] = refreshToken;
            }

            user.SetToken(userEntity.Id.ToString(), accessToken);

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
        public async Task PostOutAsync(CancellationToken cancellationToken)
        {
            user.RemoveToken();

            await Task.CompletedTask;
        }

        #endregion

        #region [ 登录后的权限获取 ]

        /// <summary>
        /// 查询是否有权限
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="onlyCode"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task<Result<bool>> GetPermissionExistsAsync([Required] long userId, [Required] string onlyCode, CancellationToken cancellationToken = default)
        {
            var res = await permissionService.ExistsAsync(userId, onlyCode, cancellationToken);

            return RestFull.Success(data: res);
        }
        /// <summary>
        /// 查询权限导航
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task<Result<IList<PermissionMenuTreeDto>>> GetPermissionMenusAsync([Required] long userId, CancellationToken cancellationToken = default)
        {
            var res = await permissionService.GetMenusAsync(userId, cancellationToken);

            return RestFull.Success(data: res);
        }
        /// <summary>
        /// 查询权限导航（快捷导航）
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task<Result<IList<PermissionMenuDto>>> GetPermissionMenuExpressAsync([Required] long userId, CancellationToken cancellationToken = default)
        {
            var res = await permissionService.GetMenuExpressAsync(userId, cancellationToken);

            return RestFull.Success(data: res);
        }

        #endregion
    }
}
