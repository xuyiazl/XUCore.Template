using Nigel.Application.Interfaces;
using Nigel.Domain.Sys.AdminMenu;
using Nigel.Domain.Sys.AdminRole;

namespace Nigel.Web.Pages.Admin.Sys.Admin.Role
{
    [Authorize]
    [AccessControl(AccessKey = "sys-role-add")]
    public class AddModel : PageModel
    {
        private readonly IAdminUserAppService adminUserAppService;
        public AddModel(IAdminUserAppService adminUserAppService)
        {
            this.adminUserAppService = adminUserAppService;
        }

        [BindProperty]
        public AdminRoleDto Role { get; set; }
        public IList<AdminMenuDto> Menus { get; set; }

        public async Task OnGetAsync()
        {
            Role = new AdminRoleDto
            {
                Status = Domain.Core.Enums.Status.Show
            };
            Menus = await adminUserAppService.GetMenuByTreeAsync();
        }

        public async Task<IActionResult> OnPostRoleByAddAsync(long[] MenuKeys)
        {
            AdminRoleCreateCommand command = new AdminRoleCreateCommand();

            command.Name = Role.Name;
            command.Status = Role.Status;
            command.MenuIds = MenuKeys;

            if (!command.IsVaild())
                return new Result(StateCode.Fail, "", command.GetErrors("</br>"));

            var res = await adminUserAppService.CreateRoleAsync(command);

            if (res > 0)
                return new Result(StateCode.Success, "", "添加成功");
            else
                return new Result(StateCode.Fail, "", "添加失败");
        }
    }
}