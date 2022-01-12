using XUCore.Template.WeChat.Core;
using XUCore.Template.WeChat.Persistence.Enums;
using XUCore.Template.WeChat.Persistence.Entities.User;

namespace XUCore.Template.WeChat.DbService.User.User
{
    public class UserService : IUserService
    {
        protected readonly FreeSqlUnitOfWorkManager unitOfWork;
        protected readonly IBaseRepository<UserEntity> repo;
        protected readonly IMapper mapper;
        protected readonly IUserInfo user;

        public UserService(IServiceProvider serviceProvider)
        {
            this.unitOfWork = serviceProvider.GetRequiredService<FreeSqlUnitOfWorkManager>();
            this.repo = unitOfWork.Orm.GetRepository<UserEntity>();
            this.mapper = serviceProvider.GetRequiredService<IMapper>();
            this.user = serviceProvider.GetRequiredService<IUserInfo>();
        }

        public async Task<UserEntity> CreateAsync(UserCreateCommand request, CancellationToken cancellationToken)
        {
            var entity = mapper.Map<UserCreateCommand, UserEntity>(request);

            var res = await repo.InsertAsync(entity, cancellationToken);

            if (res != null)
            {
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

                await unitOfWork.Orm.Insert<UserRoleEntity>(entity.UserRoles).ExecuteAffrowsAsync(cancellationToken);

                return res;
            }

            return default;
        }

        public async Task<int> UpdateAsync(UserUpdateInfoCommand request, CancellationToken cancellationToken)
        {
            var entity = await repo.Select.WhereDynamic(request.Id).ToOneAsync(cancellationToken);

            if (entity == null)
                return 0;

            entity = mapper.Map(request, entity);

            var res = await repo.UpdateAsync(entity, cancellationToken);

            return res;
        }

        public async Task<int> UpdateAsync(UserUpdatePasswordCommand request, CancellationToken cancellationToken)
        {
            var entity = await repo.Select.WhereDynamic(request.Id).ToOneAsync<UserEntity>(cancellationToken);

            request.NewPassword = Encrypt.Md5By32(request.NewPassword);
            request.OldPassword = Encrypt.Md5By32(request.OldPassword);

            if (!entity.Password.Equals(request.OldPassword))
                Failure.Error("旧密码错误");

            return await unitOfWork.Orm.Update<UserEntity>(request.Id).Set(c => new UserEntity { Password = request.NewPassword }).ExecuteAffrowsAsync(cancellationToken);
        }

        public async Task<int> UpdateAsync(long id, string field, string value, CancellationToken cancellationToken)
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
                case "mobile":
                    entity.Mobile = value;
                    break;
                case "username":
                    entity.UserName = value;
                    break;
                case "password":
                    entity.Password = Encrypt.Md5By32(value);
                    break;
            }

            return await repo.UpdateAsync(entity, cancellationToken);
        }

        public async Task<int> UpdateAsync(long[] ids, Status status, CancellationToken cancellationToken)
        {
            var list = await repo.Select.Where(c => ids.Contains(c.Id)).ToListAsync<UserEntity>(cancellationToken);

            list.ForEach(c => c.Status = status);

            return await repo.UpdateAsync(list, cancellationToken);
        }

        public async Task<int> DeleteAsync(long[] ids, CancellationToken cancellationToken)
        {
            var res = await unitOfWork.Orm.Delete<UserEntity>(ids).ExecuteAffrowsAsync(cancellationToken);

            if (res > 0)
            {
                //删除关联的导航
                await unitOfWork.Orm.Delete<UserLoginRecordEntity>().Where(c => ids.Contains(c.UserId)).ExecuteAffrowsAsync(cancellationToken);
                //删除用户关联的角色
                await unitOfWork.Orm.Delete<UserRoleEntity>().Where(c => ids.Contains(c.UserId)).ExecuteAffrowsAsync(cancellationToken);
            }

            return res;
        }

        public async Task<UserDto> LoginAsync(UserLoginCommand request, CancellationToken cancellationToken)
        {
            var user = default(UserEntity);

            request.Password = Encrypt.Md5By32(request.Password);

            var loginWay = "";

            if (!Valid.IsMobileNumberSimple(request.Account))
            {
                user = await repo.Select.Where(c => c.UserName.Equals(request.Account)).ToOneAsync(cancellationToken);
                if (user == null)
                    Failure.Error("账号不存在");

                loginWay = "UserName";
            }
            else
            {
                user = await repo.Select.Where(c => c.Mobile.Equals(request.Account)).ToOneAsync(cancellationToken);
                if (user == null)
                    Failure.Error("手机号码不存在");

                loginWay = "Mobile";
            }

            if (!user.Password.Equals(request.Password))
                Failure.Error("密码错误");
            if (user.Status != Status.Show)
                Failure.Error("您的帐号禁止登录,请与用户联系!");


            user.LoginCount += 1;
            user.LoginLastTime = DateTime.Now;
            user.LoginLastIp = Web.IP;

            await unitOfWork.Orm
                .Update<UserEntity>(user.Id)
                .Set(c => new UserEntity()
                {
                    LoginCount = user.LoginCount,
                    LoginLastTime = user.LoginLastTime,
                    LoginLastIp = user.LoginLastIp
                })
                .ExecuteAffrowsAsync(cancellationToken);

            await unitOfWork.Orm
                .Insert(new UserLoginRecordEntity
                {
                    UserId = user.Id,
                    LoginIp = user.LoginLastIp,
                    LoginWay = loginWay
                })
                .ExecuteAffrowsAsync(cancellationToken);

            return mapper.Map<UserDto>(user);
        }

        public async Task<bool> AnyByAccountAsync(AccountMode accountMode, string account, long notId, CancellationToken cancellationToken)
        {
            if (notId > 0)
            {
                switch (accountMode)
                {
                    case AccountMode.UserName:
                        return await repo.Select.AnyAsync(c => c.Id != notId && c.UserName == account, cancellationToken);
                    case AccountMode.Mobile:
                        return await repo.Select.AnyAsync(c => c.Id != notId && c.Mobile == account, cancellationToken);
                }
            }
            else
            {
                switch (accountMode)
                {
                    case AccountMode.UserName:
                        return await repo.Select.AnyAsync(c => c.UserName == account, cancellationToken);
                    case AccountMode.Mobile:
                        return await repo.Select.AnyAsync(c => c.Mobile == account, cancellationToken);
                }
            }

            return false;
        }

        public async Task<UserDto> GetByAccountAsync(AccountMode accountMode, string account, CancellationToken cancellationToken)
        {
            return accountMode switch
            {
                AccountMode.UserName => await repo.Select.Where(c => c.UserName.Equals(account)).ToOneAsync<UserDto>(cancellationToken),
                AccountMode.Mobile => await repo.Select.Where(c => c.Mobile.Equals(account)).ToOneAsync<UserDto>(cancellationToken),
                _ => null,
            };
        }

        public async Task<UserDto> GetByIdAsync(long id, CancellationToken cancellationToken)
        {
            return await repo.Select.WhereDynamic(id).ToOneAsync<UserDto>(cancellationToken);
        }

        public async Task<IList<UserSimpleDto>> GetListAsync(CancellationToken cancellationToken)
        {
            var select = repo.Select
                .Where(c => c.Status == Status.Show)
                .OrderByDescending(c => c.CreatedAt);

            var res = await select.ToListAsync<UserSimpleDto>(cancellationToken);

            return res;
        }

        public async Task<IList<UserDto>> GetListAsync(UserQueryCommand request, CancellationToken cancellationToken)
        {
            var select = repo.Select
                .WhereIf(request.Status != Status.Default, c => c.Status == request.Status)
                .WhereIf(request.Keyword.NotEmpty(), c =>
                             c.Name.Contains(request.Keyword) ||
                             c.Mobile.Contains(request.Keyword) ||
                             c.UserName.Contains(request.Keyword))
                .OrderBy(c => c.Id);

            if (request.Limit > 0)
                select = select.Take(request.Limit);

            var res = await select.ToListAsync<UserDto>(cancellationToken);

            return res;
        }

        public async Task<PagedModel<UserDto>> GetPagedListAsync(UserQueryPagedCommand request, CancellationToken cancellationToken)
        {
            var res = await repo.Select
                .WhereIf(request.Status != Status.Default, c => c.Status == request.Status)
                .WhereIf(request.Keyword.NotEmpty(), c =>
                             c.Name.Contains(request.Keyword) ||
                             c.Mobile.Contains(request.Keyword) ||
                             c.UserName.Contains(request.Keyword))
                .OrderBy(request.OrderBy)
                .ToPagedListAsync<UserEntity, UserDto>(request.CurrentPage, request.PageSize, cancellationToken);

            return res.ToModel();
        }

        public async Task<int> CreateRelevanceRoleAsync(UserRelevanceRoleCommand request, CancellationToken cancellationToken)
        {
            //先清空用户的角色，确保没有冗余的数据
            await unitOfWork.Orm.Delete<UserRoleEntity>().Where(c => c.UserId == request.UserId).ExecuteAffrowsAsync(cancellationToken);

            var userRoles = Array.ConvertAll(request.RoleIds, roleid => new UserRoleEntity
            {
                RoleId = roleid,
                UserId = request.UserId
            });

            //添加角色
            if (userRoles.Length > 0)
                return await unitOfWork.Orm.Insert(userRoles).ExecuteAffrowsAsync(cancellationToken);

            return 1;
        }

        public async Task<IList<long>> GetRoleKeysAsync(long userId, CancellationToken cancellationToken)
        {
            return await unitOfWork.Orm.Select<UserRoleEntity>().Where(c => c.UserId == userId).OrderBy(c => c.RoleId).ToListAsync(c => c.RoleId, cancellationToken);
        }

        public async Task<IList<UserLoginRecordDto>> GetRecordListAsync(UserLoginRecordQueryCommand request, CancellationToken cancellationToken)
        {
            var res = await unitOfWork.Orm.Select<UserLoginRecordEntity, UserEntity>()
                .LeftJoin((record, user) => record.UserId == user.Id)
                .Where((record, user) => record.UserId == request.UserId)
                .OrderByDescending((record, user) => record.CreatedAt)
                .Take(request.Limit)
                .ToListAsync((record, user) => new UserLoginRecordDto
                {
                    UserId = record.UserId,
                    Id = record.Id,
                    LoginIp = record.LoginIp,
                    LoginTime = record.CreatedAt.Value,
                    LoginWay = record.LoginWay,
                    Mobile = user.Mobile,
                    Name = user.Name,
                    Picture = user.Picture,
                    UserName = user.UserName
                }, cancellationToken);

            return res;
        }

        public async Task<PagedModel<UserLoginRecordDto>> GetRecordPagedListAsync(UserLoginRecordQueryPagedCommand request, CancellationToken cancellationToken)
        {
            var res = await unitOfWork.Orm
                .Select<UserLoginRecordEntity, UserEntity>()
                .LeftJoin((record, user) => record.UserId == user.Id)
                .WhereIf(request.Keyword.NotEmpty(), (record, user) => user.Name.Contains(request.Keyword) || user.Mobile.Contains(request.Keyword) || user.UserName.Contains(request.Keyword))
                .OrderByDescending((record, user) => record.CreatedAt)
                .Count(out long count)
                .Page(request.CurrentPage, request.PageSize)
                .ToListAsync((record, user) => new UserLoginRecordDto
                {
                    UserId = record.UserId,
                    Id = record.Id,
                    LoginIp = record.LoginIp,
                    LoginTime = record.CreatedAt.Value,
                    LoginWay = record.LoginWay,
                    Mobile = user.Mobile,
                    Name = user.Name,
                    Picture = user.Picture,
                    UserName = user.UserName
                }, cancellationToken);

            var model = new PagedModel<UserLoginRecordDto>(res, count, request.CurrentPage, request.PageSize);

            return model;
        }
    }
}
