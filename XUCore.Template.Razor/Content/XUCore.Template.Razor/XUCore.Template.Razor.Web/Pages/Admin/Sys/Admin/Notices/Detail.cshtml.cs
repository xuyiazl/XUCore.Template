using Nigel.Application.Interfaces;
using Nigel.Domain.Sys.Notice;

namespace Nigel.Web.Pages.Admin.Sys.Admin.Notices
{
    [Authorize]
    public class DetailModel : PageModel
    {
        private readonly INoticeAppService noticeAppService;
        public DetailModel(INoticeAppService noticeAppService)
        {
            this.noticeAppService = noticeAppService;
        }

        public NoticeDto Notice { get; set; }

        public async Task OnGetAsync(long id)
        {
            Notice = await noticeAppService.GetByIdAsync(id);
        }
    }
}