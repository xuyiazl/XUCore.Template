using XUCore.Template.WeChat.Applaction.User;
using XUCore.Template.WeChat.DbService.Auth.Role;

namespace XUCore.Template.WeChat.Web.Pages.Admin.Sys.Admin.Role
{
    [Authorize]
    [AccessControl(AccessKey = "sys-role-list")]
    public class ListModel : PageModel
    {
        private readonly IRoleAppService roleAppService;
        public ListModel(IRoleAppService roleAppService)
        {
            this.roleAppService = roleAppService;
        }

        public async Task<IActionResult> OnGetPageListAsync(int limit, int offset, string field, string search, string sort, string order, Status status)
        {
            var paged = await roleAppService.GetPageAsync(new RoleQueryPagedCommand
            {
                CurrentPage = offset / limit + 1,
                PageSize = limit,
                Status = status,
                Keyword = search,
                OrderBy = $"{sort} {order}"
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
            var res = await roleAppService.UpdateFieldAsync(id, field, value);

            if (res > 0)
                return new Result(StateCode.Success, "", "更新成功");
            else
                return new Result(StateCode.Fail, "", "更新成功");
        }

        public async Task<IActionResult> OnPutBatchStatusAsync([FromForm] long[] ids, [FromForm] Status status)
        {
            if (ids.Contains(1))
                return new Result(StateCode.Fail, "", "操作失败，超级管理员禁止操作");

            var res = await roleAppService.UpdateStatusAsync(ids, status);

            if (res > 0)
                return new Result(StateCode.Success, "", "操作成功");
            else
                return new Result(StateCode.Fail, "", "操作失败");
        }

        public async Task<IActionResult> OnDeleteBatchDeleteAsync([FromForm] long[] ids)
        {
            if (ids.Contains(1))
                return new Result(StateCode.Fail, "", "删除失败，超级管理员禁止删除");

            var res = await roleAppService.DeleteAsync(ids);

            if (res > 0)
                return new Result(StateCode.Success, "", "永久删除成功");
            else
                return new Result(StateCode.Fail, "", "永久删除失败");
        }
    }
}

