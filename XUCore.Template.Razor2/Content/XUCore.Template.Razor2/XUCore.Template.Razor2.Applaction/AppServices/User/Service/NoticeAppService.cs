using XUCore.Template.Razor2.DbService.Notice;
using XUCore.Template.Razor2.Persistence.Enums;

namespace XUCore.Template.Razor2.Applaction.User
{
    /// <summary>
    /// 公告板管理
    /// </summary>
    public class NoticeAppService : INoticeAppService
    {
        private readonly INoticeService noticeService;

        public NoticeAppService(IServiceProvider serviceProvider)
        {
            this.noticeService = serviceProvider.GetRequiredService<INoticeService>();
        }

        /// <summary>
        /// 创建公告板
        /// </summary>
        /// <param name="request"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task<long> CreateAsync(NoticeCreateCommand request, CancellationToken cancellationToken = default)
        {
            var res = await noticeService.CreateAsync(request, cancellationToken);

            if (res != null)
                return res.Id;
            else
                return 0L;
        }
        /// <summary>
        /// 更新公告板信息
        /// </summary>
        /// <param name="request"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task<int> UpdateAsync(NoticeUpdateCommand request, CancellationToken cancellationToken = default)
        {
            return await noticeService.UpdateAsync(request, cancellationToken);
        }
        /// <summary>
        /// 更新公告板指定字段内容
        /// </summary>
        /// <param name="id"></param>
        /// <param name="field"></param>
        /// <param name="value"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task<int> UpdateFieldAsync(long id, string field, string value, CancellationToken cancellationToken = default)
        {
            return await noticeService.UpdateAsync(id, field, value, cancellationToken);
        }
        /// <summary>
        /// 更新状态
        /// </summary>
        /// <param name="ids"></param>
        /// <param name="status"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task<int> UpdateStatusAsync(long[] ids, Status status, CancellationToken cancellationToken = default)
        {
            return await noticeService.UpdateStatusAsync(ids, status, cancellationToken);
        }
        /// <summary>
        /// 删除公告板（物理删除）
        /// </summary>
        /// <param name="ids"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task<int> DeleteAsync(long[] ids, CancellationToken cancellationToken = default)
        {
            return await noticeService.DeleteAsync(ids, cancellationToken);
        }
        /// <summary>
        /// 获取公告板信息
        /// </summary>
        /// <param name="id"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task<NoticeDto> GetByIdAsync(long id, CancellationToken cancellationToken = default)
        {
            return await noticeService.GetByIdAsync(id, cancellationToken);
        }
        /// <summary>
        /// 获取公告板列表
        /// </summary>
        /// <param name="request"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task<IList<NoticeDto>> GetListAsync(NoticeQueryCommand request, CancellationToken cancellationToken = default)
        {
            return await noticeService.GetListAsync(request, cancellationToken);
        }
        /// <summary>
        /// 获取公告板分页
        /// </summary>
        /// <param name="request"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task<PagedModel<NoticeDto>> GetPageAsync(NoticeQueryPagedCommand request, CancellationToken cancellationToken = default)
        {
            return await noticeService.GetPagedListAsync(request, cancellationToken);
        }
        /// <summary>
        /// 公告等级
        /// </summary>
        /// <param name="checkId"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task<IList<SelectListItem>> GetLevelSelectItemsCheck(long checkId, CancellationToken cancellationToken = default)
        {
            await Task.CompletedTask;

            var levels = new List<(long Id, string Name)> {
                (1,"普通"),
                (2,"一般"),
                (3,"重要"),
                (4,"紧急"),
            };
            return levels.ForEach(item =>
            {
                if (checkId == item.Id)
                    return new SelectListItem { Value = item.Id.SafeString(), Text = item.Name, Selected = true };
                else
                    return new SelectListItem { Value = item.Id.SafeString(), Text = item.Name };
            });
        }
    }
}
