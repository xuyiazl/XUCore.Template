using Nigel.Application.Interfaces;
using Nigel.Domain.Sys.LoginRecord;

namespace Nigel.Web.Pages.Admin.Sys.Admin
{
    [Authorize]
    [AccessControl(AccessKey = "sys-loginrecord")]
    public class RecordModel : PageModel
    {
        private readonly IAdminUserAppService adminAppService;
        public RecordModel(IAdminUserAppService adminAppService)
        {
            this.adminAppService = adminAppService;
        }

        public void OnGet()
        {

        }

        public async Task<IActionResult> OnGetRecordListAsync(int limit, int offset, string field, string search, string sort, string order)
        {
            var paged = await adminAppService.GetLoginRecordPagedAsync(new LoginRecordQueryPaged
            {
                CurrentPage = offset / limit + 1,
                Field = field,
                Order = order,
                PageSize = limit,
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
    }
}