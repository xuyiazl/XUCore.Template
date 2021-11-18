using XUCore.Template.EasyFreeSql.Core;
using XUCore.Template.EasyFreeSql.Core.Enums;
using XUCore.Template.EasyFreeSql.Persistence.Entities.User;

namespace XUCore.Template.EasyFreeSql.Applaction.User.User
{
    /// <summary>
    /// 用户管理
    /// </summary>
    [ApiExplorerSettings(GroupName = ApiGroup.Admin)]
    [DynamicWebApi]
    public class UserAppService : IUserAppService, IDynamicWebApi
    {
        /// <summary>
        /// 数据库工作单元
        /// </summary>
        protected readonly FreeSqlUnitOfWorkManager unitOfWork;
        /// <summary>
        /// 仓储
        /// </summary>
        protected readonly IBaseRepository<UserEntity> repo;
        /// <summary>
        /// mapper
        /// </summary>
        protected readonly IMapper mapper;
        /// <summary>
        /// 用户登录信息
        /// </summary>
        protected readonly IUserInfo user;
        /// <summary>
        /// 用户管理
        /// </summary>
        /// <param name="serviceProvider"></param>
        public UserAppService(IServiceProvider serviceProvider)
        {
            this.unitOfWork = serviceProvider.GetRequiredService<FreeSqlUnitOfWorkManager>();
            this.repo = unitOfWork.Orm.GetRepository<UserEntity>();
            this.mapper = serviceProvider.GetRequiredService<IMapper>();
            this.user = serviceProvider.GetRequiredService<IUserInfo>();
        }

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
        [AllowAnonymous]
        public async Task<Result<long>> CreateInitAccountAsync(CancellationToken cancellationToken = default)
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

            command.IsVaild();

            return await CreateAsync(command, cancellationToken);
        }

        #region [ 账号管理 ]

        /// <summary>
        /// 创建用户账号
        /// </summary>
        /// <param name="request"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task<Result<long>> CreateAsync([Required][FromBody] UserCreateCommand request, CancellationToken cancellationToken = default)
        {
            var entity = mapper.Map<UserCreateCommand, UserEntity>(request);

            //角色操作
            if (request.Roles != null && request.Roles.Length > 0)
            {
                //转换角色对象 并写入
                entity.UserRoles = Array.ConvertAll(request.Roles, roleid => new UserRoleEntity
                {
                    RoleId = roleid,
                    UserId = entity.Id
                });
            }

            var res = await repo.InsertAsync(entity, cancellationToken);

            if (res != null)
            {
                await unitOfWork.Orm.Insert<UserRoleEntity>(entity.UserRoles).ExecuteAffrowsAsync(cancellationToken);

                return RestFull.Success(data: res.Id);
            }

            return RestFull.Fail(data: 0L);
        }
        /// <summary>
        /// 更新账号信息
        /// </summary>
        /// <param name="request"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task<Result<int>> UpdateAsync([Required][FromBody] UserUpdateInfoCommand request, CancellationToken cancellationToken = default)
        {
            var entity = await repo.Select.WhereDynamic(request.Id).ToOneAsync<UserEntity>(cancellationToken);

            if (entity == null)
                return RestFull.Fail(data: 0);

            entity = mapper.Map(request, entity);

            var res = await repo.UpdateAsync(entity, cancellationToken);


            if (res > 0)
                return RestFull.Success(data: res);
            else
                return RestFull.Fail(data: res);
        }
        /// <summary>
        /// 更新密码
        /// </summary>
        /// <param name="request"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task<Result<int>> UpdatePasswordAsync([Required][FromBody] UserUpdatePasswordCommand request, CancellationToken cancellationToken = default)
        {
            var entity = await repo.Select.WhereDynamic(request.Id).ToOneAsync<UserEntity>(cancellationToken);

            request.NewPassword = Encrypt.Md5By32(request.NewPassword);
            request.OldPassword = Encrypt.Md5By32(request.OldPassword);

            if (!entity.Password.Equals(request.OldPassword))
                Failure.Error("旧密码错误");

            var res = await unitOfWork.Orm.Update<UserEntity>(request.Id).Set(c => new UserEntity { Password = request.NewPassword }).ExecuteAffrowsAsync(cancellationToken);

            if (res > 0)
                return RestFull.Success(data: res);
            else
                return RestFull.Fail(data: res);
        }
        /// <summary>
        /// 更新指定字段内容
        /// </summary>
        /// <param name="id"></param>
        /// <param name="field"></param>
        /// <param name="value"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task<Result<int>> UpdateFieldAsync([Required][FromQuery] long id, [Required][FromQuery] string field, [FromQuery] string value, CancellationToken cancellationToken = default)
        {
            var entity = await repo.Select.WhereDynamic(id).ToOneAsync(cancellationToken);

            if (entity.IsNull())
                Failure.Error("没有找到该记录");

            switch (field.ToLower())
            {
                case "name":
                    entity.Name = value;
                    break;
                case "position":
                    entity.Position = value;
                    break;
                case "location":
                    entity.Location = value;
                    break;
                case "company":
                    entity.Company = value;
                    break;
                case "picture":
                    entity.Picture = value;
                    break;
            }

            var res = await repo.UpdateAsync(entity, cancellationToken);

            if (res > 0)
                return RestFull.Success(data: res);
            else
                return RestFull.Fail(data: res);
        }
        /// <summary>
        /// 更新状态
        /// </summary>
        /// <param name="ids"></param>
        /// <param name="enabled"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task<Result<int>> UpdateEnabledAsync([Required][FromQuery] long[] ids, [Required][FromQuery] bool enabled, CancellationToken cancellationToken = default)
        {
            var list = await repo.Select.Where(c => ids.Contains(c.Id)).ToListAsync<UserEntity>(cancellationToken);

            list.ForEach(c => c.Enabled = enabled);

            var res = await repo.UpdateAsync(list, cancellationToken);

            if (res > 0)
                return RestFull.Success(data: res);
            else
                return RestFull.Fail(data: res);
        }
        /// <summary>
        /// 删除账号（物理删除）
        /// </summary>
        /// <param name="ids"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task<Result<int>> DeleteAsync([Required][FromQuery] long[] ids, CancellationToken cancellationToken = default)
        {
            var res = await unitOfWork.Orm.Delete<UserEntity>(ids).ExecuteAffrowsAsync(cancellationToken);

            if (res > 0)
            {
                //删除关联的导航
                await unitOfWork.Orm.Delete<UserLoginRecordEntity>().Where(c => ids.Contains(c.UserId)).ExecuteAffrowsAsync(cancellationToken);
                //删除用户关联的角色
                await unitOfWork.Orm.Delete<UserRoleEntity>().Where(c => ids.Contains(c.UserId)).ExecuteAffrowsAsync(cancellationToken);

                return RestFull.Success(data: res);
            }
            else
                return RestFull.Fail(data: res);
        }
        /// <summary>
        /// 获取账号信息
        /// </summary>
        /// <param name="id"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [HttpGet("{id}")]
        public async Task<Result<UserDto>> GetAsync([Required] long id, CancellationToken cancellationToken = default)
        {
            var res = await repo.Select.WhereDynamic(id).ToOneAsync<UserDto>(cancellationToken);

            return RestFull.Success(data: res);
        }
        /// <summary>
        /// 获取账号信息（根据账号或手机号码）
        /// </summary>
        /// <param name="accountMode"></param>
        /// <param name="account"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task<Result<UserDto>> GetAccountAsync([Required] AccountMode accountMode, [Required] string account, CancellationToken cancellationToken = default)
        {
            var user = default(UserDto);

            switch (accountMode)
            {
                case AccountMode.UserName:
                    user = await repo.Select.Where(c => c.UserName.Equals(account)).ToOneAsync<UserDto>(cancellationToken);
                    break;
                case AccountMode.Mobile:
                    user = await repo.Select.Where(c => c.Mobile.Equals(account)).ToOneAsync<UserDto>(cancellationToken);
                    break;
            }

            return RestFull.Success(data: user);
        }
        /// <summary>
        /// 检查账号或者手机号是否存在
        /// </summary>
        /// <param name="accountMode"></param>
        /// <param name="account"></param>
        /// <param name="notId"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task<Result<bool>> GetAnyAsync([Required] AccountMode accountMode, [Required] string account, [Required] long notId, CancellationToken cancellationToken = default)
        {
            var res = true;
            if (notId > 0)
            {
                switch (accountMode)
                {
                    case AccountMode.UserName:
                        res = await repo.Select.AnyAsync(c => c.Id != notId && c.UserName == account, cancellationToken);
                        break;
                    case AccountMode.Mobile:
                        res = await repo.Select.AnyAsync(c => c.Id != notId && c.Mobile == account, cancellationToken);
                        break;
                }
            }
            else
            {
                switch (accountMode)
                {
                    case AccountMode.UserName:
                        res = await repo.Select.AnyAsync(c => c.UserName == account, cancellationToken);
                        break;
                    case AccountMode.Mobile:
                        res = await repo.Select.AnyAsync(c => c.Mobile == account, cancellationToken);
                        break;
                }
            }

            return RestFull.Success(data: res);
        }
        /// <summary>
        /// 获取账号分页
        /// </summary>
        /// <param name="request"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task<Result<PagedModel<UserDto>>> GetPageAsync([Required][FromQuery] UserQueryPagedCommand request, CancellationToken cancellationToken = default)
        {
            var res = await repo.Select
                  .Where(c => c.Enabled == request.Enabled)
                  .WhereIf(request.Keyword.NotEmpty(), c =>
                               c.Name.Contains(request.Keyword) ||
                               c.Mobile.Contains(request.Keyword) ||
                               c.UserName.Contains(request.Keyword))
                  .OrderBy(c => c.Id)
                  .ToPagedListAsync<UserEntity, UserDto>(request.CurrentPage, request.PageSize, cancellationToken);

            return RestFull.Success(data: res.ToModel());
        }

        #endregion

        #region [ 账号&角色 关联操作 ]

        /// <summary>
        /// 账号关联角色
        /// </summary>
        /// <param name="request"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task<Result<int>> CreateRelevanceRoleAsync([Required][FromBody] UserRelevanceRoleCommand request, CancellationToken cancellationToken = default)
        {
            //先清空用户的角色，确保没有冗余的数据
            await unitOfWork.Orm.Delete<UserRoleEntity>().Where(c => c.UserId == request.UserId).ExecuteAffrowsAsync(cancellationToken);

            var userRoles = Array.ConvertAll(request.RoleIds, roleid => new UserRoleEntity
            {
                RoleId = roleid,
                UserId = request.UserId
            });

            var res = 0;
            //添加角色
            if (userRoles.Length > 0)
                res = await unitOfWork.Orm.Insert(userRoles).ExecuteAffrowsAsync(cancellationToken);

            if (res > 0)
                return RestFull.Success(data: res);
            else
                return RestFull.Fail(data: res);
        }
        /// <summary>
        /// 获取账号关联的角色id集合
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task<Result<List<long>>> GetRelevanceRoleKeysAsync([Required] long userId, CancellationToken cancellationToken = default)
        {
            var res = await unitOfWork.Orm.Select<UserRoleEntity>().Where(c => c.UserId == userId).OrderBy(c => c.RoleId).ToListAsync(c => c.RoleId, cancellationToken);

            return RestFull.Success(data: res);
        }

        #endregion

        #region [ 登录记录 ]

        /// <summary>
        /// 获取最近登录记录
        /// </summary>
        /// <param name="request"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task<Result<List<UserLoginRecordDto>>> GetRecordListAsync([Required][FromQuery] UserLoginRecordQueryCommand request, CancellationToken cancellationToken = default)
        {
            var res = await unitOfWork.Orm.Select<UserLoginRecordEntity>()
                   .Where(c => c.UserId == request.userId)
                   .OrderByDescending(c => c.CreatedAt)
                   .Take(request.Limit)
                   .ToListAsync(c => new UserLoginRecordDto
                   {
                       UserId = c.UserId,
                       Id = c.Id,
                       LoginIp = c.LoginIp,
                       LoginTime = c.CreatedAt.Value,
                       LoginWay = c.LoginWay,
                       Mobile = c.User.Mobile,
                       Name = c.User.Name,
                       Picture = c.User.Picture,
                       UserName = c.User.UserName
                   }, cancellationToken);

            return RestFull.Success(data: res);
        }
        /// <summary>
        /// 获取所有登录记录分页
        /// </summary>
        /// <param name="request"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task<Result<PagedModel<UserLoginRecordDto>>> GetRecordPageAsync([Required][FromQuery] UserLoginRecordQueryPagedCommand request, CancellationToken cancellationToken = default)
        {
            var res = await unitOfWork.Orm
                  .Select<UserLoginRecordEntity>()
                  .WhereIf(request.Keyword.NotEmpty(), c => c.User.Name.Contains(request.Keyword) || c.User.Mobile.Contains(request.Keyword) || c.User.UserName.Contains(request.Keyword))
                  .OrderByDescending(c => c.CreatedAt)
                  .ToPagedListAsync(request.CurrentPage, request.PageSize, c => new UserLoginRecordDto
                  {
                      UserId = c.UserId,
                      Id = c.Id,
                      LoginIp = c.LoginIp,
                      LoginTime = c.CreatedAt.Value,
                      LoginWay = c.LoginWay,
                      Mobile = c.User.Mobile,
                      Name = c.User.Name,
                      Picture = c.User.Picture,
                      UserName = c.User.UserName
                  }, cancellationToken);

            return RestFull.Success(data: res.ToModel());
        }

        #endregion
    }
}
