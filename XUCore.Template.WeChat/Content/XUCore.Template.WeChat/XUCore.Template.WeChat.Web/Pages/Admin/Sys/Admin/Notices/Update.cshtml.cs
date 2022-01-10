using XUCore.Template.WeChat.Applaction.User;
using XUCore.Template.WeChat.DbService.Notice;

namespace XUCore.Template.WeChat.Web.Pages.Admin.Sys.Admin.Notices
{
    [Authorize]
    [AccessControl(AccessKey = "sys-notice-edit")]
    public class UpdateModel : PageModel
    {
        private readonly INoticeAppService noticeAppService;
        public UpdateModel(INoticeAppService noticeAppService)
        {
            this.noticeAppService = noticeAppService;
        }

        [BindProperty]
        public NoticeDto NoticeDto { get; set; }

        public IList<SelectListItem> NoticeLevelSelectItems { get; set; }

        public async Task OnGetAsync(int id, CancellationToken cancellationToken)
        {
            NoticeDto = await noticeAppService.GetByIdAsync(id, cancellationToken);

            NoticeLevelSelectItems = await noticeAppService.GetLevelSelectItemsCheck(NoticeDto.LevelId, cancellationToken);
        }

        public async Task<IActionResult> OnPutNoticeByUpdateRowAsync(CancellationToken cancellationToken)
        {
            var command = new NoticeUpdateCommand
            {
                Id = NoticeDto.Id,
                Contents = NoticeDto.Contents,
                LevelId = NoticeDto.LevelId,
                Title = NoticeDto.Title,
                Weight = NoticeDto.Weight,

                Status = NoticeDto.Status
            };

            if (!command.IsVaild())
                return new Result(StateCode.Fail, "", command.GetErrors("</br>"));

            var res = await noticeAppService.UpdateAsync(command, cancellationToken);

            if (res > 0)
                return new Result(StateCode.Success, "", "修改成功");
            else
                return new Result(StateCode.Fail, "", "修改失败");
        }
    }
}
