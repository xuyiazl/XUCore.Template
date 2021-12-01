using XUCore.Template.Razor.Applaction.Article;
using XUCore.Template.Razor.DbService.Article;

namespace XUCore.Template.Razor.Web.Pages.Admin.Articles.Category
{
    [Authorize]
    //[AccessControl(AccessKey = "base-category-list")]
    public class ListModel : PageModel
    {
        private readonly ICategoryAppService categoryAppService;

        public ListModel(ICategoryAppService categoryAppService)
        {
            this.categoryAppService = categoryAppService;
        }

        public async Task<IActionResult> OnGetPageListAsync(int limit, int offset, string code, string field, string search, string sort, string order, Status status)
        {
            var paged = await categoryAppService.GetPagedListAsync(new CategoryQueryPagedCommand
            {
                CurrentPage = offset / limit + 1,
                Keyword = search,
                PageSize = limit,
                OrderBy = $"{sort} {order}",
                Status = status
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
            var res = await categoryAppService.UpdateFieldAsync(id, field, value);

            if (res > 0)
                return new Result(StateCode.Success, "", "更新成功");
            else
                return new Result(StateCode.Fail, "", "更新成功");
        }

        public async Task<IActionResult> OnPutBatchStatusAsync([FromForm] long[] ids, [FromForm] Status status)
        {
            var res = await categoryAppService.UpdateStatusAsync(ids, status);

            if (res > 0)
                return new Result(StateCode.Success, "", "操作成功");
            else
                return new Result(StateCode.Fail, "", "操作失败");
        }

        public async Task<IActionResult> OnDeleteBatchDeleteAsync([FromForm] long[] ids)
        {
            var res = await categoryAppService.DeleteAsync(ids);

            if (res > 0)
                return new Result(StateCode.Success, "", "永久删除成功");
            else
                return new Result(StateCode.Fail, "", "永久删除失败");
        }
    }
}