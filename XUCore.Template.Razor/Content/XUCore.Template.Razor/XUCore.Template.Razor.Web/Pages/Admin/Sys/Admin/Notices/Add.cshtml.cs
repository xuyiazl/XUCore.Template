using XUCore.Template.Razor.Applaction.User;
using XUCore.Template.Razor.DbService.Notice;

namespace XUCore.Template.Razor.Web.Pages.Admin.Sys.Admin.Notices
{
    [Authorize]
    [AccessControl(AccessKey = "sys-notice-add")]
    public class AddModel : PageModel
    {
        private readonly INoticeAppService noticeAppService;
        public AddModel(INoticeAppService noticeAppService)
        {
            this.noticeAppService = noticeAppService;
        }

        [BindProperty]
        public NoticeDto NoticeDto { get; set; }

        public IList<SelectListItem> NoticeLevelSelectItems { get; set; }

        public async void OnGet(CancellationToken cancellationToken)
        {
            NoticeDto = new NoticeDto();

            NoticeLevelSelectItems = await noticeAppService.GetLevelSelectItemsCheck(0, cancellationToken);
        }

        public async Task<IActionResult> OnPostNoticeByAddAsync(CancellationToken cancellationToken)
        {
            var command = new NoticeCreateCommand
            {
                Contents = NoticeDto.Contents,
                LevelId = NoticeDto.LevelId,
                Title = NoticeDto.Title,
                Weight = NoticeDto.Weight,
                Status = NoticeDto.Status
            };

            if (!command.IsVaild())
                return new Result(StateCode.Fail, "", command.GetErrors("</br>"));

            var res = await noticeAppService.CreateAsync(command, cancellationToken);

            if (res > 0)
                return new Result(StateCode.Success, "", "添加成功");
            else
                return new Result(StateCode.Fail, "", "添加失败");
        }
    }
}