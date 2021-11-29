using XUCore.Template.Razor.Applaction.Login;
using XUCore.Template.Razor.Applaction.User;
using XUCore.Template.Razor.DbService.User.User;

namespace XUCore.Template.Razor.Web.Pages.Admin
{
    [Authorize]
    [NoAccessControl]
    public class HelloModel : PageModel
    {
        private readonly IUserInfo userInfo;
        private readonly IUserAppService userAppService;
        private readonly ILoginAppService loginAppService;
        public HelloModel(IUserInfo userInfo, IUserAppService userAppService, ILoginAppService loginAppService)
        {
            this.userInfo = userInfo;
            this.userAppService = userAppService;
            this.loginAppService = loginAppService;
        }

        public UserDto userDto { get; set; }
        public IList<UserLoginRecordDto> AdminLoginRecords { get; set; }
        public string RootUrl { get; set; }

        public async Task OnGetAsync()
        {
            userDto = await userAppService.GetAsync(userInfo.GetId<long>());
            AdminLoginRecords = await userAppService.GetRecordListAsync(new UserLoginRecordQueryCommand
            {
                Limit = 30,
                UserId = userInfo.GetId<long>()
            });
        }
    }
}