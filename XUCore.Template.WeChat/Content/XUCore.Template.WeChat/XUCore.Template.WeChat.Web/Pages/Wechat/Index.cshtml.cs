
using Magicodes.Wx.PublicAccount.Sdk.Apis.Sns;

namespace XUCore.Template.WeChat.Web.Pages.WeChat
{
    public class IndexModel : WeChatPageModel
    {
        public GetUserInfoApiResult Info { get; set; }

        public async Task OnGetAsync()
        {
            Info = await GetWeChatUserInfoAsync();
        }
    }
}
