using Nigel.Application.Interfaces;
using Nigel.Domain.Common;
using Nigel.Domain.Sys.Notice;
using Nigel.Infrastructure.Access;

namespace Nigel.Web.Pages.Admin.Sys.Admin.Notices
{
    [Authorize]
    [AccessControl(AccessKey = "sys-notice-edit")]
    public class UpdateModel : PageModel
    {
        private readonly IAdminUserService adminUserService;
        private readonly INoticeAppService noticeAppService;
        private readonly IBasicsAppService basicsAppService;
        public UpdateModel(IAdminUserService adminUserService, INoticeAppService noticeAppService, IBasicsAppService basicsAppService)
        {
            this.adminUserService = adminUserService;
            this.noticeAppService = noticeAppService;
            this.basicsAppService = basicsAppService;
        }

        [BindProperty]
        public NoticeDto Notice { get; set; }
        public IList<SelectListItem> NoticeLevelSelectItems { get; set; }

        public async Task OnGetAsync(int id)
        {
            Notice = await noticeAppService.GetByIdAsync(id);

            NoticeLevelSelectItems = basicsAppService.GetCategorySelectItemsCheck(code: CategoryCode.NoticeLevel, checkeId: Notice.LevelId);
        }

        public async Task<IActionResult> OnPutNoticeByUpdateRowAsync()
        {
            NoticeUpdateCommand command = new NoticeUpdateCommand();

            command.Id = Notice.Id;
            command.Contents = Notice.Contents;
            command.LevelId = Notice.LevelId;
            command.Title = Notice.Title;
            command.Weight = Notice.Weight;

            command.Status = Notice.Status;

            if (!command.IsVaild())
                return new Result(StateCode.Fail, "", command.GetErrors("</br>"));

            var res = await noticeAppService.UpdateAsync(command);

            if (res > 0)
                return new Result(StateCode.Success, "", "修改成功");
            else
                return new Result(StateCode.Fail, "", "修改失败");
        }
    }
}