using XUCore.Template.Razor.Applaction.User;
using XUCore.Template.Razor.DbService.Auth.Role;
using XUCore.Template.Razor.DbService.User.User;

namespace XUCore.Template.Razor.Web.Pages.Admin.Sys.Admin.User
{
    [Authorize]
    [AccessControl(AccessKey = "sys-admin-add")]
    public class AddModel : PageModel
    {
        private readonly IUserAppService userAppService;
        private readonly IRoleAppService roleAppService;
        public AddModel(IUserAppService userAppService, IRoleAppService roleAppService)
        {
            this.userAppService = userAppService;
            this.roleAppService = roleAppService;
        }

        [BindProperty]
        public UserFrom UserDto { get; set; }

        public IList<RoleDto> Roles { get; set; }

        public async Task OnGetAsync(CancellationToken cancellationToken)
        {
            UserDto = new UserFrom { Status = Status.Show };

            Roles = await roleAppService.GetListAsync(new RoleQueryCommand { Status = Status.Show }, cancellationToken);
        }

        public async Task<IActionResult> OnPostUserByAddAsync(long[] Roles)
        {
            var command = new UserCreateCommand
            {
                Name = UserDto.Name.SafeString(),
                UserName = UserDto.UserName.SafeString(),
                Mobile = UserDto.Mobile.SafeString(),
                Password = UserDto.Password.SafeString(),
                Position = UserDto.Position.SafeString(),
                Location = UserDto.Location.SafeString(),
                Company = UserDto.Company.SafeString(),
                Roles = Roles
            };

            if (!command.IsVaild())
                return new Result(StateCode.Fail, "", command.GetErrors("</br>"));

            var res = await userAppService.CreateAsync(command);

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
                        var res = await userAppService.GetAnyAsync(AccountMode.UserName, value, 0);
                        if (!res)
                            return new Result(StateCode.Success, "", "该账号可用");
                        else
                            return new Result(StateCode.Fail, "", "该账号已存在");
                    }
                case "mobile":
                    {
                        var res = await userAppService.GetAnyAsync(AccountMode.Mobile, value, 0);
                        if (!res)
                            return new Result(StateCode.Success, "", "该手机可用");
                        else
                            return new Result(StateCode.Fail, "", "该手机已存在");
                    }
            }

            return new Result(StateCode.Fail, "", "请设置要检查的字段");
        }
    }

    public class UserFrom : UserDto
    {
        public string Password { get; set; }
    }
}