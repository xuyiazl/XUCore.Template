using XUCore.Template.Razor.Applaction.User;
using XUCore.Template.Razor.DbService.Auth.Menu;
using XUCore.Template.Razor.DbService.Auth.Role;

namespace XUCore.Template.Razor.Web.Pages.Admin.Sys.Admin.Role
{
    [Authorize]
    [AccessControl(AccessKey = "sys-role-edit")]
    public class UpdateModel : PageModel
    {
        private readonly IRoleAppService roleAppService;
        private readonly IMenuAppService menuAppService;
        public UpdateModel(IRoleAppService roleAppService, IMenuAppService menuAppService)
        {
            this.roleAppService = roleAppService;
            this.menuAppService = menuAppService;
        }

        [BindProperty]
        public RoleDto RoleDto { get; set; }
        public IList<MenuDto> Menus { get; set; }
        public IList<long> MenuKeys { get; set; }

        public async Task OnGetAsync(int id, CancellationToken cancellationToken)
        {
            RoleDto = await roleAppService.GetAsync(id, cancellationToken);

            Menus = await menuAppService.GetListAsync(new MenuQueryCommand { Status = Status.Show }, cancellationToken);

            MenuKeys = await roleAppService.GetRelevanceMenuAsync(id, cancellationToken);
        }

        public async Task<IActionResult> OnPutRoleByUpdateRowAsync(int Id, long[] MenuKeys)
        {
            var command = new RoleUpdateCommand
            {
                Id = Id,
                Name = RoleDto.Name,
                Status = RoleDto.Status,
                MenuIds = MenuKeys
            };

            if (!command.IsVaild())
                return new Result(StateCode.Fail, "", command.GetErrors("</br>"));

            var res = await roleAppService.UpdateAsync(command);

            if (res > 0)
                return new Result(StateCode.Success, "", "修改成功");
            else
                return new Result(StateCode.Fail, "", "修改失败");
        }
    }
}