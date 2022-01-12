using Magicodes.Wx.PublicAccount.Sdk;
using Magicodes.Wx.PublicAccount.Sdk.Apis.Sns;
using Magicodes.Wx.PublicAccount.Sdk.AspNet;
using Microsoft.AspNetCore.Mvc.RazorPages;
using XUCore.Template.WeChat.Applaction.WeChat;
using XUCore.Template.WeChat.Persistence.Enums;

namespace XUCore.Template.WeChat.Applaction;

[AllowAnonymous]
[WxPublicAccountOAuthFilter(OAuthLevel = OAuthLevels.OpenIdAndUserInfo)]
public class WeChatPageModel : PageModel
{
    /// <summary>
    /// 获取WebToken
    /// </summary>
    protected string WebAccessToken
    {
        get
        {
            var webAccessToken = Request.HttpContext.Items[WxConsts.COOKIE_WX_WEBTOKEN].SafeString();
            if (webAccessToken.IsEmpty())
            {
                webAccessToken = Request.Cookies[WxConsts.COOKIE_WX_WEBTOKEN];
            }

            return webAccessToken;
        }
    }

    /// <summary>
    /// 获取OpenId
    /// </summary>
    protected string OpenId
    {
        get
        {
            var openId = Request.HttpContext.Items[WxConsts.COOKIE_WX_OPENID].SafeString();
            if (openId.IsEmpty())
            {
                openId = Request.Cookies[WxConsts.COOKIE_WX_OPENID];
            }
            return openId;
        }
    }

    /// <summary>
    /// 获取微信用户信息
    /// </summary>
    /// <returns></returns>
    protected async Task<GetUserInfoApiResult> GetWeChatUserInfoAsync()
    {
        var snsApi = Request.HttpContext.RequestServices.GetRequiredService<ISnsApi>();
        var result = await snsApi.GetUserInfoAsync(WebAccessToken, OpenId);
        result.EnsureSuccess();
        return result;
    }

    public override async void OnPageHandlerExecuted(PageHandlerExecutedContext context)
    {
        var user = Request.HttpContext.RequestServices.GetRequiredService<IWeChatUserAppService>();

        var info = await GetWeChatUserInfoAsync();

        await user.CreateAsync(new()
        {
            Unionid = info.Unionid,
            OpenId = info.OpenId,
            Mobile = string.Empty,
            Password = string.Empty,

            City = info.City,
            Province = info.Province,
            Country = info.Country,

            Headimgurl = info.Headimgurl,
            NickName = info.NickName,
            Sex = (int)info.Sex,

            Status = Status.Show
        });

        base.OnPageHandlerExecuted(context);
    }
}


