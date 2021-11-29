using Nigel.Application.Interfaces;
using Nigel.Domain.Core.Enums;
using Nigel.Domain.Customer;
using Nigel.Infrastructure.Access;

namespace Nigel.Web.Pages.Admin.Sys.Admin
{
    [Authorize]
    [AccessControl(AccessKey = "sys-customer")]
    public class CustomerModel : PageModel
    {
        private readonly ICustomerAppService customerAppService;
        private readonly IAdminUserService adminUserService;

        public CustomerModel(ICustomerAppService customerAppService, IAdminUserService adminUserService)
        {
            this.customerAppService = customerAppService;
            this.adminUserService = adminUserService;
        }

        public void OnGet() { }

        public async Task<IActionResult> OnGetPageListAsync(int limit, int offset, string field, string search, string sort, string order, Status status)
        {
            var paged = await customerAppService.GetPagedAsync(new CustomerQueryPaged
            {
                CurrentPage = offset / limit + 1,
                Status = status,
                Search = search,
                Field = field,
                Order = order,
                PageSize = limit,
                Sort = sort
            });

            if (!adminUserService.IsCanAccess("sys-customer-mobile"))
            {
                paged.Items.ForEach(c => c.Mobile = c.Mobile.EncryptPhone());
            }

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
            var res = await customerAppService.UpdateAsync(id, field, value);

            if (res > 0)
                return new Result(StateCode.Success, "", "更新成功");
            else
                return new Result(StateCode.Fail, "", "更新成功");
        }

        public async Task<IActionResult> OnPutBatchStatusAsync([FromForm] long[] ids, [FromForm] Status status)
        {
            var res = await customerAppService.UpdateAsync(status, ids);

            if (res > 0)
                return new Result(StateCode.Success, "", "操作成功");
            else
                return new Result(StateCode.Fail, "", "操作失败");
        }

        public async Task<IActionResult> OnDeleteBatchDeleteAsync([FromForm] long[] ids)
        {
            var res = await customerAppService.DeleteAsync(ids);

            if (res > 0)
                return new Result(StateCode.Success, "", "永久删除成功");
            else
                return new Result(StateCode.Fail, "", "永久删除失败");
        }
    }
}