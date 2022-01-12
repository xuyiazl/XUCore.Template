using XUCore.Template.WeChat.Applaction.User;
using XUCore.Template.WeChat.Applaction.WeChat;
using XUCore.Template.WeChat.DbService.User.User;
using XUCore.Template.WeChat.DbService.User.WeChatUser;

namespace XUCore.Template.WeChat.Web.Pages.Admin.WeChat
{
    [Authorize]
    [AccessControl(AccessKey = "wechat-user")]
    public class ListModel : PageModel
    {
        private readonly IWeChatUserAppService weChatUserAppService;
        private readonly IUserAppService userAppService;

        public ListModel(IWeChatUserAppService weChatUserAppService, IUserAppService userAppService)
        {
            this.weChatUserAppService = weChatUserAppService;
            this.userAppService = userAppService;
        }

        public IList<UserSimpleDto> AdminUsers;

        public async Task OnGetAsync(CancellationToken cancellationToken)
        {
            AdminUsers = await userAppService.GetListAsync(cancellationToken);
            AdminUsers.Insert(0, new() { Id = 0, Name = "默认不选择" });
        }

        public async Task<IActionResult> OnGetPageListAsync(int limit, int offset, string field, string search, string sort, string order, Status status)
        {
            var paged = await weChatUserAppService.GetPageAsync(new WeChatUserQueryPagedCommand
            {
                CurrentPage = offset / limit + 1,
                PageSize = limit,
                Status = status,
                Keyword = search,
                //OrderBy = $"{sort} {order}"
            });

            return new JsonResult(new
            {
                error = 0,
                total = paged.TotalCount,
                pages = paged.TotalPages,
                number = paged.CurrentPage,
                rows = paged.Items
            });
        }

        public async Task<IActionResult> OnPutUpdateAsync(long id, string field, string value)
        {
            value = value.SafeString().Trim();
            field = field.ToLower();

            var res = await weChatUserAppService.UpdateAsync(id, field, value);

            if (res > 0)
                return new Result(StateCode.Success, "", "更新成功");
            else
                return new Result(StateCode.Fail, "", "更新失败");
        }

        public async Task<IActionResult> OnPutBatchStatusAsync([FromForm] long[] ids, [FromForm] Status status)
        {
            var res = await weChatUserAppService.UpdateAsync(ids, status);

            if (res > 0)
                return new Result(StateCode.Success, "", "操作成功");
            else
                return new Result(StateCode.Fail, "", "操作失败");
        }

        public async Task<IActionResult> OnDeleteBatchDeleteAsync([FromForm] long[] ids)
        {
            var res = await weChatUserAppService.DeleteAsync(ids);

            if (res > 0)
                return new Result(StateCode.Success, "", "永久删除成功");
            else
                return new Result(StateCode.Fail, "", "永久删除失败");
        }
    }
}
