using XUCore.Template.Razor.Core;
using XUCore.Template.Razor.Persistence.Entities;
using XUCore.Template.Razor.Persistence.Enums;

namespace XUCore.Template.Razor.DbService.Article
{
    public class ArticleService : FreeSqlCurdService<long, ArticleEntity, ArticleDto, ArticleCreateCommand, ArticleUpdateCommand, ArticleQueryCommand, ArticleQueryPagedCommand>,
        IArticleService
    {
        public ArticleService(IServiceProvider serviceProvider, FreeSqlUnitOfWorkManager muowm, IMapper mapper, IUserInfo user) : base(muowm, mapper, user)
        {

        }

        public override async Task<ArticleEntity> CreateAsync(ArticleCreateCommand request, CancellationToken cancellationToken)
        {
            var entity = mapper.Map<ArticleCreateCommand, ArticleEntity>(request);

            var res = await repo.InsertAsync(entity, cancellationToken);

            if (res != null)
            {
                //保存关联标签
                if (request.TagIds != null && request.TagIds.Length > 0)
                {
                    entity.TagNavs = Array.ConvertAll(request.TagIds, key => new ArticleTagEntity
                    {
                        ArticleId = entity.Id,
                        TagId = key
                    });
                }

                await freeSql.Insert<ArticleTagEntity>(entity.TagNavs).ExecuteAffrowsAsync(cancellationToken);

                CreatedAction?.Invoke(entity);

                return res;
            }

            return default;
        }

        public override async Task<int> UpdateAsync(ArticleUpdateCommand request, CancellationToken cancellationToken)
        {
            var entity = await repo.Select.WhereDynamic(request.Id).ToOneAsync(cancellationToken);

            if (entity == null)
                return 0;

            entity = mapper.Map(request, entity);

            var res = await repo.UpdateAsync(entity, cancellationToken);

            if (res > 0)
            {
                //保存关联标签
                if (request.TagIds != null && request.TagIds.Length > 0)
                {
                    entity.TagNavs = Array.ConvertAll(request.TagIds, key => new ArticleTagEntity
                    {
                        ArticleId = entity.Id,
                        TagId = key
                    });
                }

                //先清空导航集合，确保没有冗余信息
                await freeSql.Delete<ArticleTagEntity>().Where(c => c.ArticleId == request.Id).ExecuteAffrowsAsync(cancellationToken);

                await freeSql.Insert<ArticleTagEntity>(entity.TagNavs).ExecuteAffrowsAsync(cancellationToken);

                UpdatedAction?.Invoke(entity);
            }
            return res;
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
                case "title":
                    entity.Title = value;
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
            var list = await repo.Select.Where(c => ids.Contains(c.Id)).ToListAsync<ArticleEntity>(cancellationToken);

            list.ForEach(c => c.Status = status);

            return await repo.UpdateAsync(list, cancellationToken);
        }

        public override async Task<int> DeleteAsync(long[] ids, CancellationToken cancellationToken)
        {
            var res = await freeSql.Delete<ArticleEntity>(ids).ExecuteAffrowsAsync(cancellationToken);

            if (res > 0)
            {
                await freeSql.Delete<ArticleTagEntity>().Where(c => ids.Contains(c.ArticleId)).ExecuteAffrowsAsync(cancellationToken);

                DeletedAction?.Invoke(ids);
            }

            return res;
        }

        public override async Task<ArticleDto> GetByIdAsync(long id, CancellationToken cancellationToken)
        {
            var res = await repo.Select.WhereDynamic(id).Include(c => c.Category).IncludeMany(c => c.TagNavs).ToOneAsync(cancellationToken);

            var dto = mapper.Map<ArticleEntity, ArticleDto>(res);

            var navIds = res.TagNavs.Select(t => t.TagId).ToList();

            dto.TagIds = navIds;
            dto.Tags = await freeSql.Select<TagEntity>().Where(c => navIds.Contains(c.Id)).ToListAsync<TagSimpleDto>(cancellationToken);

            return dto;
        }

        public override async Task<IList<ArticleDto>> GetListAsync(ArticleQueryCommand request, CancellationToken cancellationToken)
        {
            var select = repo.Select
                   .Include(c => c.Category)
                   .WhereIf(request.Status != Status.Default, c => c.Status == request.Status)
                   .WhereIf(request.Keyword.NotEmpty(), c => c.Title.Contains(request.Keyword))
                   .OrderByDescending(c => c.Weight)
                   .OrderBy(c => c.CreatedAt);

            if (request.Limit > 0)
                select = select.Take(request.Limit);

            var res = await select.ToListAsync(cancellationToken: cancellationToken);

            var dto = mapper.Map<IList<ArticleEntity>, IList<ArticleDto>>(res);

            return dto;
        }

        public override async Task<PagedModel<ArticleDto>> GetPagedListAsync(ArticleQueryPagedCommand request, CancellationToken cancellationToken)
        {
            var res = await repo.Select
                   .Include(c => c.Category)
                   .WhereIf(request.Status != Status.Default, (c) => c.Status == request.Status)
                   .WhereIf(request.Keyword.NotEmpty(), (c) => c.Title.Contains(request.Keyword))
                   .OrderBy(request.OrderBy.NotEmpty(), request.OrderBy)
                   .OrderByDescending((c) => c.CreatedAt)
                   .Count(out long count)
                   .Page(request.CurrentPage, request.PageSize)
                   .ToListAsync(cancellationToken: cancellationToken);

            var dto = mapper.Map<IList<ArticleEntity>, IList<ArticleDto>>(res);

            return new PagedModel<ArticleDto>(dto, count, request.CurrentPage, request.PageSize);
        }
    }
}
