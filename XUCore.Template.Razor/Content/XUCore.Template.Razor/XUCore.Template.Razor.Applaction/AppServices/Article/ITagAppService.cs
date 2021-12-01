﻿using XUCore.Template.Razor.DbService.Article;
using XUCore.Template.Razor.Persistence.Enums;

namespace XUCore.Template.Razor.Applaction.Article
{
    /// <summary>
    /// 标签管理
    /// </summary>
    public interface ITagAppService : IScoped
    {
        /// <summary>
        /// 创建标签
        /// </summary>
        /// <param name="request"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<long> CreateAsync(TagCreateCommand request, CancellationToken cancellationToken = default);
        /// <summary>
        /// 更新标签信息
        /// </summary>
        /// <param name="request"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<int> UpdateAsync(TagUpdateCommand request, CancellationToken cancellationToken = default);
        /// <summary>
        /// 更新标签指定字段内容
        /// </summary>
        /// <param name="id"></param>
        /// <param name="field"></param>
        /// <param name="value"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<int> UpdateFieldAsync(long id, string field, string value, CancellationToken cancellationToken = default);
        /// <summary>
        /// 更新状态
        /// </summary>
        /// <param name="ids"></param>
        /// <param name="status"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<int> UpdateStatusAsync(long[] ids, Status status, CancellationToken cancellationToken = default);
        /// <summary>
        /// 删除标签（物理删除）
        /// </summary>
        /// <param name="ids"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<int> DeleteAsync(long[] ids, CancellationToken cancellationToken = default);
        /// <summary>
        /// 获取标签信息
        /// </summary>
        /// <param name="id"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<TagDto> GetByIdAsync(long id, CancellationToken cancellationToken = default);
        /// <summary>
        /// 获取标签列表
        /// </summary>
        /// <param name="request"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<IList<TagDto>> GetListAsync(TagQueryCommand request, CancellationToken cancellationToken = default);
        /// <summary>
        /// 获取标签分页
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<PagedModel<TagDto>> GetPagedListAsync(TagQueryPagedCommand request, CancellationToken cancellationToken = default);
    }
}
