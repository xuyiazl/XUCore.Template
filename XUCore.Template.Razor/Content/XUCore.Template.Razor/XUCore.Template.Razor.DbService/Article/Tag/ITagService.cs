using XUCore.Template.Razor.Persistence.Entities;
using XUCore.Template.Razor.Persistence.Enums;

namespace XUCore.Template.Razor.DbService.Article
{
    public interface ITagService : ICurdService<long, TagEntity, TagDto, TagCreateCommand, TagUpdateCommand, TagQueryCommand, TagQueryPagedCommand>, IScoped
    {
        /// <summary>
        /// 更新部分字段
        /// </summary>
        /// <param name="id"></param>
        /// <param name="field"></param>
        /// <param name="value"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<int> UpdateAsync(long id, string field, string value, CancellationToken cancellationToken);
        /// <summary>
        /// 更新状态
        /// </summary>
        /// <param name="ids"></param>
        /// <param name="status"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<int> UpdateStatusAsync(long[] ids, Status status, CancellationToken cancellationToken);
    }
}