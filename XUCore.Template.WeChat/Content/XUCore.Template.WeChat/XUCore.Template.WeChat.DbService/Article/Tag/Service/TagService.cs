using XUCore.Template.WeChat.Core;
using XUCore.Template.WeChat.Persistence.Entities;
using XUCore.Template.WeChat.Persistence.Enums;

namespace XUCore.Template.WeChat.DbService.Article
{
    public class TagService : FreeSqlCurdService<long, TagEntity, TagDto, TagCreateCommand, TagUpdateCommand, TagQueryCommand, TagQueryPagedCommand>,
        ITagService
    {
        public TagService(IServiceProvider serviceProvider, FreeSqlUnitOfWorkManager muowm, IMapper mapper, IUserInfo user) : base(muowm, mapper, user)
        {

        }
        /// <summary>
        /// 更新部分字段
        /// </summary>
        /// <param name="id"></param>
        /// <param name="field"></param>
        /// <param name="value"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
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
                case "weight":
                    entity.Weight = value.ToInt();
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
        public async Task<int> UpdateStatusAsync(long[] ids, Status status, CancellationToken cancellationToken)
        {
            var list = await repo.Select.Where(c => ids.Contains(c.Id)).ToListAsync<TagEntity>(cancellationToken);

            list.ForEach(c => c.Status = status);

            return await repo.UpdateAsync(list, cancellationToken);
        }

        public override async Task<int> DeleteAsync(long[] ids, CancellationToken cancellationToken)
        {
            var res = await freeSql.Delete<TagEntity>(ids).ExecuteAffrowsAsync(cancellationToken);

            if (res > 0)
            {
                await freeSql.Delete<ArticleTagEntity>().Where(c => ids.Contains(c.TagId)).ExecuteAffrowsAsync(cancellationToken);

                DeletedAction?.Invoke(ids);
            }

            return res;
        }

        /// <summary>
        /// 检查名字是否重复
        /// </summary>
        /// <param name="name"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task<bool> AnyAsync(string name, CancellationToken cancellationToken)
            => await repo.Select.AnyAsync(c => c.Name == name, cancellationToken);

        public override async Task<TagDto> GetByIdAsync(long id, CancellationToken cancellationToken)
        {
            return await repo.Select.WhereDynamic(id).ToOneAsync<TagDto>(cancellationToken);
        }

        public override async Task<IList<TagDto>> GetListAsync(TagQueryCommand request, CancellationToken cancellationToken)
        {
            var select = repo.Select
                   .WhereIf(request.Status != Status.Default, c => c.Status == request.Status)
                   .WhereIf(request.Keyword.NotEmpty(), c => c.Name.Contains(request.Keyword))
                   .OrderByDescending(c => c.Weight)
                   .OrderBy(c => c.CreatedAt);

            if (request.Limit > 0)
                select = select.Take(request.Limit);

            var res = await select.ToListAsync<TagDto>(cancellationToken);

            return res;
        }

        public override async Task<PagedModel<TagDto>> GetPagedListAsync(TagQueryPagedCommand request, CancellationToken cancellationToken)
        {
            var res = await repo.Select
                   .WhereIf(request.Status != Status.Default, (c) => c.Status == request.Status)
                   .WhereIf(request.Keyword.NotEmpty(), (c) => c.Name.Contains(request.Keyword))
                   .OrderBy(request.OrderBy.NotEmpty(), request.OrderBy)
                   .OrderByDescending((c) => c.CreatedAt)
                   .ToPagedListAsync<TagEntity, TagDto>(request.CurrentPage, request.PageSize, cancellationToken);

            return res.ToModel();
        }
    }
}

