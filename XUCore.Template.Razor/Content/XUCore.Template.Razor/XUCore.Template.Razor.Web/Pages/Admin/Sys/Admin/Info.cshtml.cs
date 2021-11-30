using XUCore.Template.Razor.Applaction.User;
using XUCore.Template.Razor.DbService.User.User;

namespace XUCore.Template.Razor.Web.Pages.Admin.Sys.Admin
{
    [Authorize]
    public class InfoModel : PageModel
    {
        private readonly IUserInfo userInfo;
        private readonly IUserAppService userAppService;

        public InfoModel(IUserInfo userInfo, IUserAppService userAppService)
        {
            this.userInfo = userInfo;
            this.userAppService = userAppService;
        }

        [BindProperty]
        public UserDto UserDto { get; set; }

        public async Task OnGetAsync()
        {
            UserDto = await userAppService.GetAsync(userInfo.UserId);
        }

        public async Task<IActionResult> OnPutUserUpdateInfoAsync()
        {
            var command = new UserUpdateInfoCommand
            {
                Id = userInfo.UserId,
                Name = UserDto.Name,
                Position = UserDto.Position.SafeString(),
                Location = UserDto.Location.SafeString(),
                Company = UserDto.Company.SafeString()
            };

            if (!command.IsVaild())
                return new Result(StateCode.Fail, "", command.GetErrors("</br>"));

            var res = await userAppService.UpdateAsync(command);

            if (res > 0)
                return new Result(StateCode.Success, "", "修改成功");
            else
                return new Result(StateCode.Fail, "", "修改失败");
        }

        public async Task<IActionResult> OnPutUserUpdatePasswordAsync(string newpassword, string oldpassword)
        {
            var command = new UserUpdatePasswordCommand()
            {
                Id = userInfo.UserId,
                OldPassword = oldpassword,
                NewPassword = newpassword
            };

            if (!command.IsVaild())
                return new Result(StateCode.Fail, "", command.GetErrors("</br>"));

            var res = await userAppService.UpdatePasswordAsync(command);

            if (res > 0)
                return new Result(StateCode.Success, "", "修改成功");
            else
                return new Result(StateCode.Fail, "", "修改失败");
        }

        public async Task<IActionResult> OnPutUserUpdatePictureAsync(string Picture)
        {
            var res = await userAppService.UpdateFieldAsync(userInfo.UserId, "picture", Picture);

            if (res > 0)
                return new Result(StateCode.Success, "", "修改成功");
            else
                return new Result(StateCode.Fail, "", "修改失败");
        }
    }
}