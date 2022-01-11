using Magicodes.Wx.PublicAccount.Sdk;
using Magicodes.Wx.PublicAccount.Sdk.Apis.Menu;

namespace XUCore.Template.WeChat.Applaction.WeChat;

public class WeChatMenuAppService : IWeChatMenuAppService
{
    private IMenuApi menuApi;

    public WeChatMenuAppService(IMenuApi menuApi)
    {
        this.menuApi = menuApi;
    }

    public async Task CreateAsync()
    {
        var res = await menuApi.CreateAsync(new CreateMenuInput()
        {
            Button = new()
            {
                new ClickButton()
                {
                    Name = "今日歌曲",
                    Key = "V1001_TODAY_MUSIC"
                },
                new SubMenuButton()
                {
                    Name = "菜单",
                    SubButtons = new()
                    {
                        new ViewButton()
                        {
                            Name = "搜索",
                            Url = "http://www.soso.com/"
                        },
                        //需关联小程序后
                        //new MiniprogramButton()
                        //{
                        //    Name = "wxa",
                        //    Url = "http://mp.weixin.qq.com",
                        //    AppId = "wx286b93c14bbf93aa",
                        //    Pagepath = "pages/lunar/index"
                        //},
                        new ClickButton()
                        {
                            Name = "赞一下我们",
                            Key = "V1001_GOOD"
                        }
                    }
                }
            }
        });

        res.EnsureSuccess();
    }
}

