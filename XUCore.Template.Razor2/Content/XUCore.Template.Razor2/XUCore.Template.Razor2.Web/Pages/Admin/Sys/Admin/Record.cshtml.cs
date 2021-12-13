using XUCore.Template.Razor2.Applaction.User;
using XUCore.Template.Razor2.DbService.User.User;

namespace XUCore.Template.Razor2.Web.Pages.Admin.Sys.Admin
{
    [Authorize]
    [AccessControl(AccessKey = "sys-loginrecord")]
    public class RecordModel : PageModel
    {
        private readonly IUserAppService userAppService;
        public RecordModel(IUserAppService userAppService)
        {
            this.userAppService = userAppService;
        }

        public async Task<IActionResult> OnGetRecordListAsync(int limit, int offset, string field, string search, string sort, string order)
        {
            var paged = await userAppService.GetRecordPageAsync(new UserLoginRecordQueryPagedCommand
            {
                CurrentPage = offset / limit + 1,
                PageSize = limit,
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
    }
}