using XUCore.Template.WeChat.Applaction.User;
using XUCore.Template.WeChat.DbService.Notice;

namespace XUCore.Template.WeChat.Web.Pages.Admin.Sys.Admin.Notices
{
    [Authorize]
    public class DetailModel : PageModel
    {
        private readonly INoticeAppService noticeAppService;
        public DetailModel(INoticeAppService noticeAppService)
        {
            this.noticeAppService = noticeAppService;
        }

        public NoticeDto NoticeDto { get; set; }

        public async Task OnGetAsync(long id, CancellationToken cancellationToken)
        {
            NoticeDto = await noticeAppService.GetByIdAsync(id, cancellationToken);
        }
    }
}
