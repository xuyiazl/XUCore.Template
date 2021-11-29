using Nigel.Application.Interfaces;
using Nigel.Domain.Core.Enums;
using Nigel.Domain.Sys.AdminRole;
using Nigel.Domain.Sys.AdminUser;

namespace Nigel.Web.Pages.Admin.Sys.Admin.User
{
    [Authorize]
    [AccessControl(AccessKey = "sys-admin-add")]
    public class AddModel : PageModel
    {
        private readonly IAdminUserAppService adminUserAppService;
        public AddModel(IAdminUserAppService adminUserAppService)
        {
            this.adminUserAppService = adminUserAppService;
        }

        [BindProperty]
        public AdminUserFrom Users { get; set; }
        public IList<AdminRoleDto> Roles { get; set; }

        public async Task OnGetAsync()
        {
            Users = new AdminUserFrom
            {
                Status = Domain.Core.Enums.Status.Show
            };
            Roles = await adminUserAppService.GetRoleAllAsync();
        }

        public async Task<IActionResult> OnPostUserByAddAsync(long[] Roles)
        {
            AdminUserCreateCommand command = new AdminUserCreateCommand
            {
                Name = Users.Name.SafeString(),
                UserName = Users.UserName.SafeString(),
                Mobile = Users.Mobile.SafeString(),
                Password = Users.Password.SafeString(),
                Position = Users.Position.SafeString(),
                Location = Users.Location.SafeString(),
                Company = Users.Company.SafeString(),
                Roles = Roles
            };

            if (!command.IsVaild())
                return new Result(StateCode.Fail, "", command.GetErrors("</br>"));

            var res = await adminUserAppService.CreateAsync(command);

            if (res > 0)
                return new Result(StateCode.Success, "", "添加成功");
            else
                return new Result(StateCode.Fail, "", "添加失败");
        }

        public async Task<IActionResult> OnGetUserExistAsync(string field, string value)
        {
            switch (field.ToLower())
            {
                case "username":
                    {
                        var res = await adminUserAppService.AnyAsync(AccountMode.UserName, value, 0);
                        if (!res)
                            return new Result(StateCode.Success, "", "该账号可用");
                        else
                            return new Result(StateCode.Fail, "", "该账号已存在");
                    }
                case "mobile":
                    {
                        var res = await adminUserAppService.AnyAsync(AccountMode.Mobile, value, 0);
                        if (!res)
                            return new Result(StateCode.Success, "", "该手机可用");
                        else
                            return new Result(StateCode.Fail, "", "该手机已存在");
                    }
            }

            return new Result(StateCode.Fail, "", "请设置要检查的字段");
        }
    }

    public class AdminUserFrom : AdminUserDto
    {
        public string Password { get; set; }
    }
}