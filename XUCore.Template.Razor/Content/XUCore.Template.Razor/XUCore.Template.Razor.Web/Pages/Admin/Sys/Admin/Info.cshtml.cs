using Nigel.Application.Interfaces;
using Nigel.Domain.Sys.AdminUser;
using Nigel.Infrastructure.Access;

namespace Nigel.Web.Pages.Admin.Sys.Admin
{
    [Authorize]
    public class InfoModel : PageModel
    {
        private readonly IAdminUserService adminUserService;
        private readonly IAdminUserAppService adminUserAppService;

        public InfoModel(IAdminUserService adminUserService, IAdminUserAppService adminUserAppService)
        {
            this.adminUserService = adminUserService;
            this.adminUserAppService = adminUserAppService;
        }

        [BindProperty]
        public AdminUserDto AdminUsers { get; set; }

        public async Task OnGetAsync()
        {
            AdminUsers = await adminUserAppService.GetByIdAsync(adminUserService.GetId<long>());
        }

        public async Task<IActionResult> OnPutUserUpdateInfoAsync()
        {
            AdminUserUpdateInfoCommand command = new AdminUserUpdateInfoCommand();

            command.Id = adminUserService.GetId<long>();
            command.Name = AdminUsers.Name;
            command.Position = AdminUsers.Position.SafeString();
            command.Location = AdminUsers.Location.SafeString();
            command.Company = AdminUsers.Company.SafeString();

            if (!command.IsVaild())
                return new Result(StateCode.Fail, "", command.GetErrors("</br>"));

            if (await adminUserAppService.UpdateAsync(command) > 0)
                return new Result(StateCode.Success, "", "修改成功");
            else
                return new Result(StateCode.Fail, "", "修改失败");
        }

        public async Task<IActionResult> OnPutUserUpdatePasswordAsync(string newpassword, string oldpassword)
        {
            try
            {
                var command = new AdminUserUpdatePasswordCommand()
                {
                    Id = adminUserService.GetId<long>(),
                    OldPassword = oldpassword,
                    NewPassword = newpassword
                };

                if (!command.IsVaild())
                    return new Result(StateCode.Fail, "", command.GetErrors("</br>"));

                var res = await adminUserAppService.UpdateAsync(command);

                if (res > 0)
                    return new Result(StateCode.Success, "", "修改成功");
                else
                    return new Result(StateCode.Fail, "", "修改失败");
            }
            catch (Exception ex)
            {
                return new Result(StateCode.Fail, "", ex.Message);
            }
        }

        public async Task<IActionResult> OnPutUserUpdatePictureAsync(string Picture)
        {
            var res = await adminUserAppService.UpdateAsync(adminUserService.GetId<long>(), "picture", Picture);

            if (res > 0)
                return new Result(StateCode.Success, "", "修改成功");
            else
                return new Result(StateCode.Fail, "", "修改失败");
        }

    }
}