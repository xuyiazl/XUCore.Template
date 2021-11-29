using Nigel.Application.Interfaces;
using Nigel.Domain.Sys.Base.Enum;
using Nigel.Infrastructure.Access;

namespace Nigel.Web.Pages.Admin.Sys.Base.Enums
{
    [Authorize]
    [AccessControl(AccessKey = "base-enum-add")]
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
        public EnumDto Enum { get; set; } = new EnumDto()
        {
            Status = Domain.Core.Enums.Status.Show
        };
        public IList<SelectListItem> DropDownTreeList { get; set; } = new List<SelectListItem>();

        public void OnGet()
        {
            Enum.Guid = Guid.NewGuid().ToString();

            DropDownTreeList = basicsAppService.GetEnumDropdownByTree();
        }

        public async Task<IActionResult> OnPostBaseEnumByAddAsync()
        {
            EnumCreateCommand command = new EnumCreateCommand();
            command.Name = Enum.Name.SafeString();
            command.Value = Enum.Value;
            command.Code = Enum.Code.SafeString();
            command.ParentGuid = Enum.ParentGuid.SafeString();
            command.Remark = Enum.Remark.SafeString();
            command.Css = Enum.Css.SafeString();

            command.Status = Enum.Status;

            if (!command.IsVaild())
                return new Result(StateCode.Fail, "", command.GetErrors("</br>"));

            var res = await basicsAppService.CreateEnumAsync(command);

            if (res > 0)
                return new Result(StateCode.Success, "", "添加成功");
            else
                return new Result(StateCode.Fail, "", "添加失败");
        }
    }
}