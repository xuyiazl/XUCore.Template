using XUCore.Template.WeChat.Core;
using XUCore.Template.WeChat.Persistence.Enums;
using XUCore.Template.WeChat.Persistence.Entities.User;

namespace XUCore.Template.WeChat.DbService.User.WeChatUser
{
    public class WeChatUserService : FreeSqlCurdService<long, WeChatUserEntity, WeChatUserDto, WeChatUserCreateCommand, WeChatUserUpdateInfoCommand, WeChatUserQueryCommand, WeChatUserQueryPagedCommand>,
        IWeChatUserService
    {
        public WeChatUserService(IServiceProvider serviceProvider, FreeSqlUnitOfWorkManager muowm, IMapper mapper, IUserInfo UserWeChat) : base(muowm, mapper, UserWeChat)
        {

        }

        public override async Task<WeChatUserEntity> CreateAsync(WeChatUserCreateCommand request, CancellationToken cancellationToken)
        {
            var entity = await repo.Select.Where(c => c.OpenId == request.OpenId).ToOneAsync(cancellationToken);

            if (entity == null)
            {
                entity = mapper.Map<WeChatUserCreateCommand, WeChatUserEntity>(request);

                var res = await repo.InsertAsync(entity, cancellationToken);

                if (res != null)
                {
                    CreatedAction?.Invoke(entity);

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

                    UpdatedAction?.Invoke(entity);
                }
            }

            return entity;
        }

        public async Task<int> UpdateAsync(WeChatUserUpdatePasswordCommand request, CancellationToken cancellationToken)
        {
            var entity = await repo.Select.WhereDynamic(request.Id).ToOneAsync<WeChatUserEntity>(cancellationToken);

            request.NewPassword = Encrypt.Md5By32(request.NewPassword);
            request.OldPassword = Encrypt.Md5By32(request.OldPassword);

            if (!entity.Password.Equals(request.OldPassword))
                Failure.Error("旧密码错误");

            return await freeSql.Update<WeChatUserEntity>(request.Id).Set(c => new WeChatUserEntity { Password = request.NewPassword }).ExecuteAffrowsAsync(cancellationToken);
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
        /// <summary>
        /// 更新状态
        /// </summary>
        /// <param name="ids"></param>
        /// <param name="status"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task<int> UpdateAsync(long[] ids, Status status, CancellationToken cancellationToken)
        {
            var list = await repo.Select.Where(c => ids.Contains(c.Id)).ToListAsync<WeChatUserEntity>(cancellationToken);

            list.ForEach(c => c.Status = status);

            return await repo.UpdateAsync(list, cancellationToken);
        }

        public override async Task<int> DeleteAsync(long[] ids, CancellationToken cancellationToken)
        {
            var res = await freeSql.Delete<WeChatUserEntity>(ids).ExecuteAffrowsAsync(cancellationToken);

            if (res > 0)
            {
                DeletedAction?.Invoke(ids);
            }

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

        public async Task<WeChatUserDto> GetByOpenIdAsync(string opendId, CancellationToken cancellationToken)
        {
            var user = await repo.Select.Where(c => c.OpenId == opendId).ToOneAsync(cancellationToken);

            return mapper.Map<WeChatUserDto>(user);
        }

        public override async Task<IList<WeChatUserDto>> GetListAsync(WeChatUserQueryCommand request, CancellationToken cancellationToken)
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

        public override async Task<PagedModel<WeChatUserDto>> GetPagedListAsync(WeChatUserQueryPagedCommand request, CancellationToken cancellationToken)
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

