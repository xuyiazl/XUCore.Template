using Nigel.Application.Interfaces;
using Nigel.Domain.Core.Enums;

namespace Nigel.Web.Pages.Admin.Sys.Admin.Menu
{
    [Authorize]
    [AccessControl(AccessKey = "sys-menus-list")]
    public class ListModel : PageModel
    {
        private readonly IAdminUserAppService adminUserAppService;

        public ListModel(IAdminUserAppService adminUserAppService)
        {
            this.adminUserAppService = adminUserAppService;
        }

        public void OnGet() { }

        public async Task<IActionResult> OnGetMenuByListAsync()
        {
            var entities = await adminUserAppService.GetMenuByTreeAsync();

            return new JsonResult(new
            {
                error = 0,
                rows = entities
            });
        }

        public async Task<IActionResult> OnPutUpdateFieldAsync(long id, string field, string value)
        {
            var res = await adminUserAppService.UpdateMenuAsync(id, field, value);

            if (res > 0)
                return new Result(StateCode.Success, "", "更新成功");
            else
                return new Result(StateCode.Fail, "", "更新成功");
        }

        public async Task<IActionResult> OnPutBatchStatusAsync([FromForm] long[] ids, [FromForm] Status status)
        {
            var res = await adminUserAppService.UpdateMenuAsync(status, ids);

            if (res > 0)
                return new Result(StateCode.Success, "", "操作成功");
            else
                return new Result(StateCode.Fail, "", "操作失败");
        }

        public async Task<IActionResult> OnDeleteBatchDeleteAsync([FromForm] long[] ids)
        {
            var res = await adminUserAppService.DeleteMenuAsync(ids);

            if (res > 0)
                return new Result(StateCode.Success, "", "永久删除成功");
            else
                return new Result(StateCode.Fail, "", "永久删除失败");
        }
    }
}