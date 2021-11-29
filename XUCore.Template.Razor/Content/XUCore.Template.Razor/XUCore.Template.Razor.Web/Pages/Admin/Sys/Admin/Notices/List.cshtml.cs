using Nigel.Application.Interfaces;
using Nigel.Domain.Core.Enums;
using Nigel.Domain.Sys.Notice;
using Nigel.Infrastructure.Access;

namespace Nigel.Web.Pages.Admin.Sys.Admin.Notices
{
    [Authorize]
    [AccessControl(AccessKey = "sys-notice-list")]
    public class ListModel : PageModel
    {
        private readonly INoticeAppService noticeAppService;
        private readonly IAdminUserService adminUserService;
        public ListModel(INoticeAppService noticeAppService, IAdminUserService adminUserService)
        {
            this.noticeAppService = noticeAppService;
            this.adminUserService = adminUserService;
        }

        public void OnGet()
        {

        }

        public async Task<IActionResult> OnGetPageListAsync(int limit, int offset, string field, string search, string sort, string order, Status status)
        {
            var paged = await noticeAppService.GetPagedAsync(new NoticeQueryPagedToAdmin
            {
                CurrentPage = offset / limit + 1,
                PageSize = limit,
                Status = status,
                Field = field,
                Order = order,
                Search = search,
                Sort = sort
            });

            return new JsonResult(new
            {
                error = 0,
                total = paged.TotalCount,
                pages = paged.TotalPages,
                number = paged.CurrentPage,
                rows = paged.Items
            });
        }

        public async Task<IActionResult> OnPutUpdateFieldAsync(long id, string field, string value)
        {
            var res = await noticeAppService.UpdateAsync(id, field, value);

            if (res > 0)
                return new Result(StateCode.Success, "", "更新成功");
            else
                return new Result(StateCode.Fail, "", "更新成功");
        }

        public async Task<IActionResult> OnPutBatchStatusAsync([FromForm] long[] ids, [FromForm] Status status)
        {
            var res = await noticeAppService.UpdateAsync(status, ids);

            if (res > 0)
                return new Result(StateCode.Success, "", "操作成功");
            else
                return new Result(StateCode.Fail, "", "操作失败");
        }

        public async Task<IActionResult> OnDeleteBatchDeleteAsync([FromForm] long[] ids)
        {
            var res = await noticeAppService.DeleteAsync(ids);

            if (res > 0)
                return new Result(StateCode.Success, "", "永久删除成功");
            else
                return new Result(StateCode.Fail, "", "永久删除失败");
        }
    }
}