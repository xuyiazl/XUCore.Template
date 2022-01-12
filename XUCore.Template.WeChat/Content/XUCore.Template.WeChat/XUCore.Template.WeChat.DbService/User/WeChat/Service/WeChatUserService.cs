using XUCore.Template.WeChat.Core;
using XUCore.Template.WeChat.Persistence.Enums;
using XUCore.Template.WeChat.Persistence.Entities.User;

namespace XUCore.Template.WeChat.DbService.User.WeChatUser
{
    public class WeChatUserService : IWeChatUserService
    {
        protected readonly FreeSqlUnitOfWorkManager unitOfWork;
        protected readonly IBaseRepository<WeChatUserEntity> repo;
        protected readonly IMapper mapper;
        protected readonly IUserInfo user;

        public WeChatUserService(IServiceProvider serviceProvider)
        {
            this.unitOfWork = serviceProvider.GetRequiredService<FreeSqlUnitOfWorkManager>();
            this.repo = unitOfWork.Orm.GetRepository<WeChatUserEntity>();
            this.mapper = serviceProvider.GetRequiredService<IMapper>();
            this.user = serviceProvider.GetRequiredService<IUserInfo>();
        }

        public async Task<WeChatUserEntity> CreateAsync(WeChatUserCreateCommand request, CancellationToken cancellationToken)
        {
            var entity = await repo.Select.Where(c => c.OpenId == request.OpenId).ToOneAsync(cancellationToken);

            if (entity == null)
            {
                entity = mapper.Map<WeChatUserCreateCommand, WeChatUserEntity>(request);

                var res = await repo.InsertAsync(entity, cancellationToken);

                if (res != null)
                {
                    return res;
                }
            }
            else
            {
                if (!entity.NickName.Equals(request.NickName) || !entity.Headimgurl.Equals(request.Headimgurl))
                {
                    entity.NickName = request.NickName;
                    entity.Headimgurl = request.Headimgurl;

                    entity.City = request.City;
                    entity.Province = request.Province;
                    entity.Country = request.Country;

                    await repo.UpdateAsync(entity, cancellationToken);
                }
            }

            return entity;
        }

        public async Task<int> UpdateAsync(WeChatUserUpdateInfoCommand request, CancellationToken cancellationToken)
        {
            var entity = await repo.Select.WhereDynamic(request.Id).ToOneAsync(cancellationToken);

            if (entity == null)
                return 0;

            entity = mapper.Map(request, entity);

            var res = await repo.UpdateAsync(entity, cancellationToken);

            return res;
        }

        public async Task<int> UpdateAsync(WeChatUserUpdatePasswordCommand request, CancellationToken cancellationToken)
        {
            var entity = await repo.Select.WhereDynamic(request.Id).ToOneAsync<WeChatUserEntity>(cancellationToken);

            request.NewPassword = Encrypt.Md5By32(request.NewPassword);
            request.OldPassword = Encrypt.Md5By32(request.OldPassword);

            if (!entity.Password.Equals(request.OldPassword))
                Failure.Error("旧密码错误");

            return await unitOfWork.Orm.Update<WeChatUserEntity>(request.Id).Set(c => new WeChatUserEntity { Password = request.NewPassword }).ExecuteAffrowsAsync(cancellationToken);
        }

        public async Task<int> UpdateAsync(long id, string field, string value, CancellationToken cancellationToken)
        {
            var entity = await repo.Select.WhereDynamic(id).ToOneAsync(cancellationToken);

            if (entity.IsNull())
                Failure.Error("没有找到该记录");

            switch (field.ToLower())
            {
                case "nickname":
                    entity.NickName = value;
                    break;
                case "city":
                    entity.City = value;
                    break;
                case "province":
                    entity.Province = value;
                    break;
                case "country":
                    entity.Country = value;
                    break;
                case "headimgurl":
                    entity.Headimgurl = value;
                    break;
                case "sex":
                    entity.Sex = value.ToInt();
                    break;
                case "userid":
                    entity.UserId = value.ToInt();
                    break;
            }

            return await repo.UpdateAsync(entity, cancellationToken);
        }

        public async Task<int> UpdateAsync(long[] ids, Status status, CancellationToken cancellationToken)
        {
            var list = await repo.Select.Where(c => ids.Contains(c.Id)).ToListAsync<WeChatUserEntity>(cancellationToken);

            list.ForEach(c => c.Status = status);

            return await repo.UpdateAsync(list, cancellationToken);
        }

        public async Task<int> DeleteAsync(long[] ids, CancellationToken cancellationToken)
        {
            var res = await unitOfWork.Orm.Delete<WeChatUserEntity>(ids).ExecuteAffrowsAsync(cancellationToken);

            return res;
        }

        public async Task<WeChatUserDto> LoginAsync(WeChatUserLoginCommand request, CancellationToken cancellationToken)
        {
            var user = default(WeChatUserEntity);

            request.Password = Encrypt.Md5By32(request.Password);

            user = await repo.Select.Where(c => c.Mobile.Equals(request.Mobile)).ToOneAsync(cancellationToken);
            if (user == null)
                Failure.Error("手机号码不存在");

            if (!user.Password.Equals(request.Password))
                Failure.Error("密码错误");
            if (user.Status != Status.Show)
                Failure.Error("您的帐号禁止登录,请与用户联系!");

            return mapper.Map<WeChatUserDto>(user);
        }

        public async Task<WeChatUserDto> GetByIdAsync(long id, CancellationToken cancellationToken)
        {
            return await repo.Select.WhereDynamic(id).ToOneAsync<WeChatUserDto>(cancellationToken);
        }

        public async Task<WeChatUserDto> GetByOpenIdAsync(string opendId, CancellationToken cancellationToken)
        {
            var user = await repo.Select.Where(c => c.OpenId == opendId).ToOneAsync(cancellationToken);

            return mapper.Map<WeChatUserDto>(user);
        }

        public async Task<IList<WeChatUserDto>> GetListAsync(WeChatUserQueryCommand request, CancellationToken cancellationToken)
        {
            var select = repo.Select
                  .Include(c => c.User)
                  .WhereIf(request.Status != Status.Default, c => c.Status == request.Status)
                  .WhereIf(request.Keyword.NotEmpty(), c => c.NickName.Contains(request.Keyword) || c.Mobile.Contains(request.Keyword))
                  .OrderBy(c => c.CreatedAt);

            if (request.Limit > 0)
                select = select.Take(request.Limit);

            var res = await select.ToListAsync(cancellationToken: cancellationToken);

            var dto = mapper.Map<IList<WeChatUserEntity>, IList<WeChatUserDto>>(res);

            return dto;
        }

        public async Task<PagedModel<WeChatUserDto>> GetPagedListAsync(WeChatUserQueryPagedCommand request, CancellationToken cancellationToken)
        {
            var res = await repo.Select
                   .Include(c => c.User)
                   .WhereIf(request.Status != Status.Default, (c) => c.Status == request.Status)
                   .WhereIf(request.Keyword.NotEmpty(), c => c.NickName.Contains(request.Keyword) || c.Mobile.Contains(request.Keyword))
                   .OrderByDescending((c) => c.CreatedAt)
                   .Count(out long count)
                   .Page(request.CurrentPage, request.PageSize)
                   .ToListAsync(cancellationToken: cancellationToken);

            var dto = mapper.Map<IList<WeChatUserEntity>, IList<WeChatUserDto>>(res);

            return new PagedModel<WeChatUserDto>(dto, count, request.CurrentPage, request.PageSize);
        }
    }
}

