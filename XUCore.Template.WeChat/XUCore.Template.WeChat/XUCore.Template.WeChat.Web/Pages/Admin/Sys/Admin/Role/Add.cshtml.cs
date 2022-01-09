using XUCore.Template.WeChat.Applaction.User;
using XUCore.Template.WeChat.DbService.Auth.Menu;
using XUCore.Template.WeChat.DbService.Auth.Role;

namespace XUCore.Template.WeChat.Web.Pages.Admin.Sys.Admin.Role
{
    [Authorize]
    [AccessControl(AccessKey = "sys-role-add")]
    public class AddModel : PageModel
    {
        private readonly IRoleAppService roleAppService;
        private readonly IMenuAppService menuAppService;
        public AddModel(IRoleAppService roleAppService, IMenuAppService menuAppService)
        {
            this.roleAppService = roleAppService;
            this.menuAppService = menuAppService;
        }

        [BindProperty]
        public RoleDto RoleDto { get; set; }
        public IList<MenuDto> Menus { get; set; }

        public async Task OnGetAsync(CancellationToken cancellationToken)
        {
            RoleDto = new RoleDto { Status = Status.Show };
            Menus = await menuAppService.GetListAsync(new MenuQueryCommand { Status = Status.Show }, cancellationToken);
            Menus = TreeUtils.AuthMenusTree(Menus, true);
        }

        public async Task<IActionResult> OnPostRoleByAddAsync(long[] MenuKeys)
        {
            var command = new RoleCreateCommand
            {
                Name = RoleDto.Name,
                Status = RoleDto.Status,
                MenuIds = MenuKeys
            };

            if (!command.IsVaild())
                return new Result(StateCode.Fail, "", command.GetErrors("</br>"));

            var res = await roleAppService.CreateAsync(command);

            if (res > 0)
                return new Result(StateCode.Success, "", "添加成功");
            else
                return new Result(StateCode.Fail, "", "添加失败");
        }
    }
}
