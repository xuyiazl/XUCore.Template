using XUCore.Template.Razor2.Applaction.Article;
using XUCore.Template.Razor2.DbService.Article;

namespace XUCore.Template.Razor2.Web.Pages.Admin.Articles.Tag
{
    [Authorize]
    [AccessControl(AccessKey = "content-article-list")]
    public class ListModel : PageModel
    {
        private readonly ITagAppService tagAppService;

        public ListModel(ITagAppService tagAppService)
        {
            this.tagAppService = tagAppService;
        }

        public async Task<IActionResult> OnGetPageListAsync(int limit, int offset, string code, string field, string search, string sort, string order, Status status)
        {
            var paged = await tagAppService.GetPagedListAsync(new TagQueryPagedCommand
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

        public async Task<IActionResult> OnPutUpdateAsync(long id, string field, string value)
        {
            var res = await tagAppService.UpdateAsync(id, field, value);

            if (res > 0)
                return new Result(StateCode.Success, "", "更新成功");
            else
                return new Result(StateCode.Fail, "", "更新成功");
        }

        public async Task<IActionResult> OnPutBatchStatusAsync([FromForm] long[] ids, [FromForm] Status status)
        {
            var res = await tagAppService.UpdateAsync(ids, status);

            if (res > 0)
                return new Result(StateCode.Success, "", "操作成功");
            else
                return new Result(StateCode.Fail, "", "操作失败");
        }

        public async Task<IActionResult> OnDeleteBatchDeleteAsync([FromForm] long[] ids)
        {
            var res = await tagAppService.DeleteAsync(ids);

            if (res > 0)
                return new Result(StateCode.Success, "", "永久删除成功");
            else
                return new Result(StateCode.Fail, "", "永久删除失败");
        }
    }
}