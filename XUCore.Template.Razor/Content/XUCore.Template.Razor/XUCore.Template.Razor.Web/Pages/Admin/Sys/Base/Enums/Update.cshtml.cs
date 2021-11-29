using Nigel.Application.Interfaces;
using Nigel.Domain.Sys.Base.Enum;
using Nigel.Infrastructure.Access;

namespace Nigel.Web.Pages.Admin.Sys.Base.Enums
{
    [Authorize]
    [AccessControl(AccessKey = "base-enum-edit")]
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
        public EnumDto Enum { get; set; }
        public IList<SelectListItem> DropDownTreeList { get; set; } = new List<SelectListItem>();

        public async Task OnGetAsync(int id)
        {
            Enum = await basicsAppService.GetEnumSingleAsync(id);

            DropDownTreeList = basicsAppService.GetEnumDropdownByTree();
        }

        public async Task<IActionResult> OnPutBaseEnumByUpdateRowAsync(int Id)
        {
            EnumUpdateCommand command = new EnumUpdateCommand();

            command.Id = Enum.Id;
            command.Name = Enum.Name.SafeString();
            command.Value = Enum.Value;
            command.Code = Enum.Code.SafeString();
            command.ParentGuid = Enum.ParentGuid.SafeString();
            command.Remark = Enum.Remark.SafeString();
            command.Css = Enum.Css.SafeString();

            command.Status = Enum.Status;

            if (!command.IsVaild())
                return new Result(StateCode.Fail, "", command.GetErrors("</br>"));

            var res = await basicsAppService.UpdateEnumAsync(command);

            if (res > 0)
            {
                return new Result(StateCode.Success, "", "修改成功");
            }
            else
                return new Result(StateCode.Fail, "", "修改失败");
        }
    }
}