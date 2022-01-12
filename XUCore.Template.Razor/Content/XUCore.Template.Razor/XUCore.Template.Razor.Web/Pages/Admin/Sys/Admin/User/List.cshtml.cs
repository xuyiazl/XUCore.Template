using XUCore.Template.Razor.Applaction.User;
using XUCore.Template.Razor.DbService.Auth.Role;
using XUCore.Template.Razor.DbService.User.User;

namespace XUCore.Template.Razor.Web.Pages.Admin.Sys.Admin.User
{
    [Authorize]
    [AccessControl(AccessKey = "sys-admin-list")]
    public class ListModel : PageModel
    {
        private readonly IUserAppService userAppService;
        private readonly IRoleAppService roleAppService;

        public ListModel(IUserAppService userAppService, IRoleAppService roleAppService)
        {
            this.userAppService = userAppService;
            this.roleAppService = roleAppService;
        }

        public IList<RoleDto> Roles { get; set; }

        public async Task OnGetAsync(CancellationToken cancellationToken)
        {
            Roles = await roleAppService.GetListAsync(new RoleQueryCommand { Status = Status.Show }, cancellationToken);
        }

        public async Task<IActionResult> OnGetPageListAsync(int limit, int offset, string field, string search, string sort, string order, Status status)
        {
            var paged = await userAppService.GetPageAsync(new UserQueryPagedCommand
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

        public async Task<IActionResult> OnGetAccreditListAsync(long id)
        {
            var roles = await userAppService.GetRelevanceRoleKeysAsync(id);

            return new JsonResult(roles);
        }

        public async Task<IActionResult> OnPutUpdateAsync(long id, string field, string value)
        {
            value = value.SafeString().Trim();
            field = field.ToLower();
            if (field.Equals("username"))
            {
                var res = await userAppService.GetAnyAsync(AccountMode.UserName, value, id);
                if (res)
                    return new Result(StateCode.Fail, "", "更新失败，账号已存在");
            }
            if (field.Equals("mobile"))
            {
                var res = await userAppService.GetAnyAsync(AccountMode.Mobile, value, id);
                if (res)
                    return new Result(StateCode.Fail, "", "更新失败，手机号码已存在");
            }
            {
                var res = await userAppService.UpdateAsync(id, field, value);

                if (res > 0)
                    return new Result(StateCode.Success, "", "更新成功");
                else
                    return new Result(StateCode.Fail, "", "更新失败");
            }
        }

        public async Task<IActionResult> OnPutAccreditAsync(long id, long[] roles)
        {
            var res = await userAppService.CreateRelevanceRoleAsync(new UserRelevanceRoleCommand
            {
                UserId = id,
                RoleIds = roles
            });

            if (res > 0)
                return new Result(StateCode.Success, "", "授权成功");
            else
                return new Result(StateCode.Fail, "", "授权失败");
        }
        public async Task<IActionResult> OnPutBatchStatusAsync([FromForm] long[] ids, [FromForm] Status status)
        {
            var res = await userAppService.UpdateAsync(ids, status);

            if (res > 0)
                return new Result(StateCode.Success, "", "操作成功");
            else
                return new Result(StateCode.Fail, "", "操作失败");
        }

        public async Task<IActionResult> OnDeleteBatchDeleteAsync([FromForm] long[] ids)
        {
            var res = await userAppService.DeleteAsync(ids);

            if (res > 0)
                return new Result(StateCode.Success, "", "永久删除成功");
            else
                return new Result(StateCode.Fail, "", "永久删除失败");
        }
    }
}