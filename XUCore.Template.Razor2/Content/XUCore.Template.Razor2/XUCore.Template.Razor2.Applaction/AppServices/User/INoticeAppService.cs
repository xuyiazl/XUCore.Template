using XUCore.Template.Razor2.DbService.Notice;
using XUCore.Template.Razor2.Persistence.Enums;

namespace XUCore.Template.Razor2.Applaction.User
{
    /// <summary>
    /// 公告板管理
    /// </summary>
    public interface INoticeAppService : IScoped
    {

        /// <summary>
        /// 创建公告板
        /// </summary>
        /// <param name="request"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<long> CreateAsync(NoticeCreateCommand request, CancellationToken cancellationToken = default);
        /// <summary>
        /// 更新公告板信息
        /// </summary>
        /// <param name="request"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<int> UpdateAsync(NoticeUpdateCommand request, CancellationToken cancellationToken = default);
        /// <summary>
        /// 更新公告板指定字段内容
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
        /// 删除公告板（物理删除）
        /// </summary>
        /// <param name="ids"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<int> DeleteAsync(long[] ids, CancellationToken cancellationToken = default);
        /// <summary>
        /// 获取公告板信息
        /// </summary>
        /// <param name="id"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<NoticeDto> GetByIdAsync(long id, CancellationToken cancellationToken = default);
        /// <summary>
        /// 获取公告板列表
        /// </summary>
        /// <param name="request"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<IList<NoticeDto>> GetListAsync(NoticeQueryCommand request, CancellationToken cancellationToken = default);
        /// <summary>
        /// 获取公告板分页
        /// </summary>
        /// <param name="request"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<PagedModel<NoticeDto>> GetPageAsync(NoticeQueryPagedCommand request, CancellationToken cancellationToken = default);

        /// <summary>
        /// 公告等级
        /// </summary>
        /// <param name="checkId"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<IList<SelectListItem>> GetLevelSelectItemsCheck(long checkId, CancellationToken cancellationToken = default);
    }
}
