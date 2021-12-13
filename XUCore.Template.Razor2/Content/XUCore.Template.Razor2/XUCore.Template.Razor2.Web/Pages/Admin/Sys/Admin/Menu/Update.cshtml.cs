using XUCore.Template.Razor2.Applaction.User;
using XUCore.Template.Razor2.DbService.Auth.Menu;

namespace XUCore.Template.Razor2.Web.Pages.Admin.Sys.Admin.Menu
{
    [Authorize]
    [AccessControl(AccessKey = "sys-menus-edit")]
    public class UpdateModel : PageModel
    {
        private readonly IMenuAppService menuAppService;
        public UpdateModel(IMenuAppService menuAppService)
        {
            this.menuAppService = menuAppService;
        }


        [BindProperty]
        public MenuDto MenuDto { get; set; }
        public IList<SelectListItem> DropDownMenus { get; set; }

        public async Task OnGetAsync(long id, CancellationToken cancellationToken)
        {
            var entities = await menuAppService.GetListAsync(cancellationToken);

            DropDownMenus = TreeUtils.AuthMenusSelectItems(entities);

            MenuDto = await menuAppService.GetByIdAsync(id, cancellationToken);
        }

        public async Task<IActionResult> OnPutMenuByUpdateRowAsync()
        {
            var command = new MenuUpdateCommand
            {
                Id = MenuDto.Id,
                Name = MenuDto.Name,
                Icon = MenuDto.Icon.IsEmpty() ? "" : MenuDto.Icon,
                Url = MenuDto.Url.IsEmpty() ? "#" : MenuDto.Url,
                OnlyCode = MenuDto.OnlyCode,
                IsMenu = MenuDto.IsMenu,
                ParentId = MenuDto.ParentId,
                Weight = MenuDto.Weight,
                IsExpress = MenuDto.IsExpress,

                Status = MenuDto.Status
            };

            if (!command.IsVaild())
                return new Result(StateCode.Fail, "", command.GetErrors("</br>"));

            var res = await menuAppService.UpdateAsync(command);

            if (res > 0)
                return new Result(StateCode.Success, "", "修改成功");
            else
                return new Result(StateCode.Fail, "", "修改失败");
        }
    }
}