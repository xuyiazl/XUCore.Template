using XUCore.Template.Razor.Applaction.User;
using XUCore.Template.Razor.DbService.Auth.Menu;

namespace XUCore.Template.Razor.Web.Pages.Admin.Sys.Admin.Menu
{
    [Authorize]
    [AccessControl(AccessKey = "sys-menus-add")]
    public class AddModel : PageModel
    {
        private readonly IMenuAppService menuAppService;
        public AddModel(IMenuAppService menuAppService)
        {
            this.menuAppService = menuAppService;
        }

        [BindProperty]
        public MenuDto MenuDto { get; set; }

        public IList<SelectListItem> DropDownMenus { get; set; }

        public async Task OnGetAsync(CancellationToken cancellationToken)
        {
            MenuDto = new MenuDto();

            var entities = await menuAppService.GetListAsync(cancellationToken);

            DropDownMenus = TreeUtils.AuthMenusSelectItems(entities);
        }

        public async Task<IActionResult> OnPostMenuByAddAsync()
        {
            var command = new MenuCreateCommand
            {
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

            var res = await menuAppService.CreateAsync(command);

            if (res > 0)
                return new Result(StateCode.Success, "", "添加成功");
            else
                return new Result(StateCode.Fail, "", "添加失败");
        }
    }
}