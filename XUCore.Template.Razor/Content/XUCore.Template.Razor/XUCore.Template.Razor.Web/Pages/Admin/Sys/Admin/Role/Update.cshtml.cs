using Nigel.Application.Interfaces;
using Nigel.Domain.Sys.AdminMenu;
using Nigel.Domain.Sys.AdminRole;

namespace Nigel.Web.Pages.Admin.Sys.Admin.Role
{
    [Authorize]
    [AccessControl(AccessKey = "sys-role-edit")]
    public class UpdateModel : PageModel
    {
        private readonly IAdminUserAppService adminUserAppService;
        public UpdateModel(IAdminUserAppService adminUserAppService)
        {
            this.adminUserAppService = adminUserAppService;
        }

        [BindProperty]
        public AdminRoleDto Role { get; set; }
        public IList<AdminMenuDto> Menus { get; set; }
        public IList<long> MenuKeys { get; set; }

        public async Task OnGetAsync(int id)
        {
            Menus = await adminUserAppService.GetMenuByTreeAsync();

            Role = await adminUserAppService.GetRoleByIdAsync(id);

            MenuKeys = await adminUserAppService.GetRoleRelevanceMenuIdsAsync(id);
        }

        public async Task<IActionResult> OnPutRoleByUpdateRowAsync(int Id, long[] MenuKeys)
        {
            AdminRoleUpdateCommand command = new AdminRoleUpdateCommand();

            command.Id = Id;
            command.Name = Role.Name;
            command.Status = Role.Status;
            command.MenuIds = MenuKeys;

            if (!command.IsVaild())
                return new Result(StateCode.Fail, "", command.GetErrors("</br>"));

            var res = await adminUserAppService.UpdateRoleAsync(command);

            if (res > 0)
                return new Result(StateCode.Success, "", "修改成功");
            else
                return new Result(StateCode.Fail, "", "修改失败");
        }
    }
}