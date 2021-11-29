using Nigel.Application.Interfaces;
using Nigel.Domain.Sys.Base.Config;
using Nigel.Infrastructure.Access;

namespace Nigel.Web.Pages.Admin.Sys.Base.Config
{
    [Authorize]
    [AccessControl(AccessKey = "base-config-edit")]
    public class UpdateModel : PageModel
    {
        private readonly IBasicsAppService basicsAppService;
        private readonly IAdminUserService adminUserService;

        public UpdateModel(IBasicsAppService basicsAppService, IAdminUserService adminUserService)
        {
            this.basicsAppService = basicsAppService;
            this.adminUserService = adminUserService;
        }

        [BindProperty]
        public ConfigDto Config { get; set; }
        public IList<SelectListItem> DropDownTreeList { get; set; } = new List<SelectListItem>();

        public async Task OnGetAsync(int id)
        {
            Config = await basicsAppService.GetConfigByIdAsync(id);

            DropDownTreeList = basicsAppService.GetConfigDropdownByTree();
        }

        public async Task<IActionResult> OnPutBaseConfigByUpdateRowAsync(int Id)
        {
            ConfigUpdateCommand command = new ConfigUpdateCommand();

            command.Id = Config.Id;
            command.Name = Config.Name.SafeString();
            command.Value = Config.Value.SafeString();
            command.Code = Config.Code.SafeString();
            command.ParentGuid = Config.ParentGuid.SafeString();
            command.Remark = Config.Remark.SafeString();
            command.ValueType = Config.ValueType.SafeString();

            command.Status = Config.Status;

            if (!command.IsVaild())
                return new Result(StateCode.Fail, "", command.GetErrors("</br>"));

            var res = await basicsAppService.UpdateConfigAsync(command);

            if (res > 0)
            {
                return new Result(StateCode.Success, "", "修改成功");
            }
            else
                return new Result(StateCode.Fail, "", "修改失败");
        }
    }
}