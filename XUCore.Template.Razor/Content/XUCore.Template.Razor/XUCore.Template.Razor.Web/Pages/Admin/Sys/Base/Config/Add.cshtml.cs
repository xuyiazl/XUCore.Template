using Nigel.Application.Interfaces;
using Nigel.Domain.Sys.Base.Config;
using Nigel.Infrastructure.Access;

namespace Nigel.Web.Pages.Admin.Sys.Base.Config
{
    [Authorize]
    [AccessControl(AccessKey = "base-config-add")]
    public class AddModel : PageModel
    {
        private readonly IBasicsAppService basicsAppService;
        private readonly IAdminUserService adminUserService;

        public AddModel(IBasicsAppService basicsAppService, IAdminUserService adminUserService)
        {
            this.basicsAppService = basicsAppService;
            this.adminUserService = adminUserService;
        }

        [BindProperty]
        public ConfigDto Config { get; set; } = new ConfigDto()
        {
            Status = Domain.Core.Enums.Status.Show
        };
        public IList<SelectListItem> DropDownTreeList { get; set; } = new List<SelectListItem>();

        public void OnGet()
        {
            Config.Guid = Guid.NewGuid().ToString();

            DropDownTreeList = basicsAppService.GetConfigDropdownByTree();
        }

        public async Task<IActionResult> OnPostBaseConfigByAddAsync()
        {
            ConfigCreateCommand command = new ConfigCreateCommand();

            command.Name = Config.Name.SafeString();
            command.Value = Config.Value.SafeString();
            command.Code = Config.Code.SafeString();
            command.ParentGuid = Config.ParentGuid.SafeString();
            command.Remark = Config.Remark.SafeString();
            command.ValueType = Config.ValueType.SafeString();

            command.Status = Config.Status;

            if (!command.IsVaild())
                return new Result(StateCode.Fail, "", command.GetErrors("</br>"));

            var res = await basicsAppService.CreateConfigAsync(command);

            if (res > 0)
                return new Result(StateCode.Success, "", "添加成功");
            else
                return new Result(StateCode.Fail, "", "添加失败");
        }
    }
}