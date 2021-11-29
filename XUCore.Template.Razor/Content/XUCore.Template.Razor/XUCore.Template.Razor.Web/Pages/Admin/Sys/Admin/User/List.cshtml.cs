using Nigel.Application.Interfaces;
using Nigel.Domain.Core.Enums;
using Nigel.Domain.Sys.AdminRole;
using Nigel.Domain.Sys.AdminUser;

namespace Nigel.Web.Pages.Admin.Sys.Admin.User
{
    [Authorize]
    [AccessControl(AccessKey = "sys-admin-list")]
    public class ListModel : PageModel
    {
        private readonly IAdminUserAppService adminUserAppService;

        public ListModel(IAdminUserAppService adminUserAppService)
        {
            this.adminUserAppService = adminUserAppService;
        }

        public IList<AdminRoleDto> Roles { get; set; }

        public async Task OnGetAsync()
        {
            Roles = await adminUserAppService.GetRoleAllAsync();
        }

        public async Task<IActionResult> OnGetPageListAsync(int limit, int offset, string field, string search, string sort, string order, Status status)
        {
            var paged = await adminUserAppService.GetPagedAsync(new AdminUserQueryPaged
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

        public async Task<IActionResult> OnGetAccreditListAsync(long id)
        {
            var roles = await adminUserAppService.GetRelevanceRoleIdsAsync(id);

            return new JsonResult(roles);
        }

        public async Task<IActionResult> OnPutUpdateFieldAsync(long id, string field, string value)
        {
            value = value.Trim();
            field = field.ToLower();
            if (field.Equals("username"))
            {
                var res = await adminUserAppService.AnyAsync(AccountMode.UserName, value, id);
                if (res)
                    return new Result(StateCode.Fail, "", "更新失败，账号已存在");
            }
            if (field.Equals("mobile"))
            {
                var res = await adminUserAppService.AnyAsync(AccountMode.Mobile, value, id);
                if (res)
                    return new Result(StateCode.Fail, "", "更新失败，手机号码已存在");
            }

            if (field.Equals("password"))
                value = Encrypt.Md5By32(value);
            {
                var res = await adminUserAppService.UpdateAsync(id, field, value);

                if (res > 0)
                    return new Result(StateCode.Success, "", "更新成功");
                else
                    return new Result(StateCode.Fail, "", "更新失败");
            }
        }

        public async Task<IActionResult> OnPutAccreditAsync(long id, long[] Roles)
        {
            var res = await adminUserAppService.RelevanceRoleAsync(id, Roles);

            if (res > 0)
                return new Result(StateCode.Success, "", "授权成功");
            else
                return new Result(StateCode.Fail, "", "授权失败");
        }
        public async Task<IActionResult> OnPutBatchStatusAsync([FromForm] long[] ids, [FromForm] Status status)
        {
            var res = await adminUserAppService.UpdateAsync(status, ids);

            if (res > 0)
                return new Result(StateCode.Success, "", "操作成功");
            else
                return new Result(StateCode.Fail, "", "操作失败");
        }

        public async Task<IActionResult> OnDeleteBatchDeleteAsync([FromForm] long[] ids)
        {
            var res = await adminUserAppService.DeleteAsync(ids);

            if (res > 0)
                return new Result(StateCode.Success, "", "永久删除成功");
            else
                return new Result(StateCode.Fail, "", "永久删除失败");
        }
    }
}