using XUCore.Template.Razor.Applaction.User;
using XUCore.Template.Razor.DbService.Notice;
using XUCore.Template.Razor.DbService.User.User;

namespace XUCore.Template.Razor.Web.Pages.Admin
{
    [Authorize]
    [NoAccessControl]
    public class HelloModel : PageModel
    {
        private readonly IUserInfo userInfo;
        private readonly IUserAppService userAppService;
        private readonly INoticeAppService noticeAppService;
        public HelloModel(IUserInfo userInfo, IUserAppService userAppService, INoticeAppService noticeAppService)
        {
            this.userInfo = userInfo;
            this.userAppService = userAppService;
            this.noticeAppService = noticeAppService;
        }

        public UserDto UserDto { get; set; }
        public IList<UserLoginRecordDto> UserLoginRecords { get; set; }
        public string RootUrl { get; set; }

        public async Task OnGetAsync(CancellationToken cancellationToken)
        {
            UserDto = await userAppService.GetByIdAsync(userInfo.UserId, cancellationToken);

            UserLoginRecords = await userAppService.GetRecordListAsync(new UserLoginRecordQueryCommand
            {
                Limit = 30,
                UserId = userInfo.UserId
            }, cancellationToken);
        }

        public async Task<IActionResult> OnGetIndexNoticeListAsync(int limit, int offset, CancellationToken cancellationToken)
        {
            var paged = await noticeAppService.GetPageAsync(new NoticeQueryPagedCommand { CurrentPage = offset / limit + 1, PageSize = limit, Status = Status.Show }, cancellationToken);

            return new JsonResult(new
            {
                error = 0,
                total = paged.TotalCount,
                pages = paged.TotalPages,
                number = paged.CurrentPage,
                rows = paged.Items
            });
        }
    }
}