﻿using XUCore.Template.Razor.Applaction.User;
using XUCore.Template.Razor.DbService.Notice;

namespace XUCore.Template.Razor.Web.Pages.Admin.Sys.Admin.Notices
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