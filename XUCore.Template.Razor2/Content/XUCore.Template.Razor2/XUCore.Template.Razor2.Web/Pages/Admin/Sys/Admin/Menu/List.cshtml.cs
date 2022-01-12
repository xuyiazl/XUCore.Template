using XUCore.Template.Razor2.Applaction.User;
using XUCore.Template.Razor2.Persistence.Enums;

namespace XUCore.Template.Razor2.Web.Pages.Admin.Sys.Admin.Menu
{
    [Authorize]
    [AccessControl(AccessKey = "sys-menus-list")]
    public class ListModel : PageModel
    {
        private readonly IMenuAppService menuAppService;
        public ListModel(IMenuAppService menuAppService)
        {
            this.menuAppService = menuAppService;
        }

        public async Task<IActionResult> OnGetMenuByListAsync(CancellationToken cancellationToken)
        {
            var entities = await menuAppService.GetListAsync(cancellationToken);

            entities = TreeUtils.AuthMenusTree(entities, true);

            return new JsonResult(new
            {
                error = 0,
                rows = entities
            });
        }

        public async Task<IActionResult> OnPutUpdateAsync(long id, string field, string value)
        {
            var res = await menuAppService.UpdateAsync(id, field, value);

            if (res > 0)
                return new Result(StateCode.Success, "", "更新成功");
            else
                return new Result(StateCode.Fail, "", "更新成功");
        }

        public async Task<IActionResult> OnPutBatchStatusAsync([FromForm] long[] ids, [FromForm] Status status)
        {
            var res = await menuAppService.UpdateAsync(ids, status);

            if (res > 0)
                return new Result(StateCode.Success, "", "操作成功");
            else
                return new Result(StateCode.Fail, "", "操作失败");
        }

        public async Task<IActionResult> OnDeleteBatchDeleteAsync([FromForm] long[] ids)
        {
            var res = await menuAppService.DeleteAsync(ids);

            if (res > 0)
                return new Result(StateCode.Success, "", "永久删除成功");
            else
                return new Result(StateCode.Fail, "", "永久删除失败");
        }
    }
}