using Nigel.Application.Interfaces;
using Nigel.Domain.Core.Enums;
using Nigel.Domain.Sys.Base.Category;

namespace Nigel.Web.Pages.Admin.Sys.Base.Category
{
    [Authorize]
    [AccessControl(AccessKey = "base-category-list")]
    public class ListModel : PageModel
    {
        private readonly IBasicsAppService basicsAppService;

        public IList<string> CodeList { get; set; }

        public ListModel(IBasicsAppService basicsAppService)
        {
            this.basicsAppService = basicsAppService;
        }

        public void OnGet()
        {
            CodeList = basicsAppService.GetCategoryDistinctByCode();
        }

        public async Task<IActionResult> OnGetPageListAsync(int limit, int offset, string code, string field, string search, string sort, string order, Status status)
        {
            var paged = await basicsAppService.GetCategoryPagedAsync(new CategoryQueryPaged
            {
                CurrentPage = offset / limit + 1,
                Code = code,
                Field = field,
                Order = order,
                PageSize = limit,
                Search = search,
                Sort = sort,
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
            var res = await basicsAppService.UpdateCategoryAsync(id, field, value);

            if (res > 0)
                return new Result(StateCode.Success, "", "更新成功");
            else
                return new Result(StateCode.Fail, "", "更新成功");
        }

        public async Task<IActionResult> OnPutBatchStatusAsync([FromForm] long[] ids, [FromForm] Status status)
        {
            var res = await basicsAppService.UpdateCategoryAsync(status, ids);

            if (res > 0)
                return new Result(StateCode.Success, "", "操作成功");
            else
                return new Result(StateCode.Fail, "", "操作失败");
        }

        public async Task<IActionResult> OnDeleteBatchDeleteAsync([FromForm] long[] ids)
        {
            var res = await basicsAppService.DeleteCategoryAsync(ids);

            if (res > 0)
                return new Result(StateCode.Success, "", "永久删除成功");
            else
                return new Result(StateCode.Fail, "", "永久删除失败");
        }
    }
}