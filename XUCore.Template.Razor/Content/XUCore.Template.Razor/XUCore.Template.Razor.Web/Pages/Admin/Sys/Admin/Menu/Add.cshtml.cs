using Nigel.Application.Interfaces;
using Nigel.Domain.Common;
using Nigel.Domain.Sys.AdminMenu;

namespace Nigel.Web.Pages.Admin.Sys.Admin.Menu
{
    [Authorize]
    [AccessControl(AccessKey = "sys-menus-add")]
    public class AddModel : PageModel
    {
        private readonly IAdminUserAppService adminUserAppService;
        public AddModel(IAdminUserAppService adminUserAppService)
        {
            this.adminUserAppService = adminUserAppService;
        }

        [BindProperty]
        public AdminMenuDto Menus { get; set; }
        public IList<SelectListItem> DropDownMenus { get; set; }

        public async Task OnGetAsync()
        {
            Menus = new AdminMenuDto
            {
                Status = Domain.Core.Enums.Status.Show
            };
            var entities = await adminUserAppService.GetMenuByWeightAsync(true);

            DropDownMenus = TreeUtils.AuthMenusSelectItems(entities);
        }

        public async Task<IActionResult> OnPostMenuByAddAsync()
        {
            AdminMenuCreateCommand command = new AdminMenuCreateCommand();

            command.Name = Menus.Name;
            command.Icon = Menus.Icon.IsEmpty() ? "" : Menus.Icon;
            command.Url = Menus.Url.IsEmpty() ? "#" : Menus.Url;
            command.OnlyCode = Menus.OnlyCode;
            command.IsMenu = Menus.IsMenu;
            command.FatherId = Menus.FatherId;
            command.Weight = Menus.Weight;
            command.IsExpress = Menus.IsExpress;

            command.Status = Menus.Status;

            if (!command.IsVaild())
                return new Result(StateCode.Fail, "", command.GetErrors("</br>"));

            var res = await adminUserAppService.CreateMenuAsync(command);

            if (res > 0)
                return new Result(StateCode.Success, "", "添加成功");
            else
                return new Result(StateCode.Fail, "", "添加失败");
        }
    }
}