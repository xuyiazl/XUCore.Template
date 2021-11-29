using Nigel.Application.Interfaces;
using Nigel.Domain.Common;
using Nigel.Domain.Sys.Notice;
using Nigel.Infrastructure.Access;

namespace Nigel.Web.Pages.Admin.Sys.Admin.Notices
{
    [Authorize]
    [AccessControl(AccessKey = "sys-notice-add")]
    public class AddModel : PageModel
    {
        private readonly IAdminUserService adminUserService;
        private readonly INoticeAppService noticeAppService;
        private readonly IBasicsAppService basicsAppService;
        public AddModel(IAdminUserService adminUserService, INoticeAppService noticeAppService, IBasicsAppService basicsAppService)
        {
            this.adminUserService = adminUserService;
            this.noticeAppService = noticeAppService;
            this.basicsAppService = basicsAppService;
        }

        [BindProperty]
        public NoticeDto Notice { get; set; }

        public IList<SelectListItem> NoticeLevelSelectItems { get; set; }

        public void OnGet()
        {
            Notice = new NoticeDto
            {
                Status = Domain.Core.Enums.Status.Show
            };
            NoticeLevelSelectItems = basicsAppService.GetCategorySelectItemsCheck(code: CategoryCode.NoticeLevel);
        }

        public async Task<IActionResult> OnPostNoticeByAddAsync()
        {
            NoticeCreateCommand command = new NoticeCreateCommand
            {
                Contents = Notice.Contents,
                LevelId = Notice.LevelId,
                Title = Notice.Title,
                Weight = Notice.Weight,
                Status = Notice.Status,
                AdminId = adminUserService.GetId<long>()
            };

            if (!command.IsVaild())
                return new Result(StateCode.Fail, "", command.GetErrors("</br>"));

            var res = await noticeAppService.CreateAsync(command);

            if (res > 0)
                return new Result(StateCode.Success, "", "添加成功");
            else
                return new Result(StateCode.Fail, "", "添加失败");
        }
    }
}