using Essensoft.AspNetCore.Payment.WeChatPay;
using Essensoft.AspNetCore.Payment.WeChatPay.V3;
using Essensoft.AspNetCore.Payment.WeChatPay.V3.Domain;
using Essensoft.AspNetCore.Payment.WeChatPay.V3.Request;
using System.ComponentModel.DataAnnotations;
using XUCore.Serializer;

namespace XUCore.Template.WeChat.Web.Pages.WeChat.Pay
{
    public class PubPayModel : PageModel
    {
        private readonly IWeChatPayClient client;
        private readonly WeChatPayOptions weChatPayOptions;

        public WeChatPayPubPayV3ViewModel viewModel { get; set; }

        public PubPayModel(IWeChatPayClient client, WeChatPayOptions weChatPayOptions)
        {
            this.client = client;
            this.weChatPayOptions = weChatPayOptions;
        }

        public void OnGet()
        {
            viewModel = new WeChatPayPubPayV3ViewModel();
        }

        /// <summary>
        /// 公众号支付-JSAPI下单
        /// </summary>
        /// <param name="viewModel"></param>
        public async Task<IActionResult> OnPostPubPayAsync(WeChatPayPubPayV3ViewModel _viewModel)
        {
            var model = new WeChatPayTransactionsJsApiBodyModel
            {
                AppId = weChatPayOptions.AppId,
                MchId = weChatPayOptions.MchId,
                Amount = new Amount { Total = _viewModel.Total, Currency = "CNY" },
                Description = _viewModel.Description,
                NotifyUrl = _viewModel.NotifyUrl,
                OutTradeNo = _viewModel.OutTradeNo,
                Payer = new PayerInfo { OpenId = _viewModel.OpenId }
            };

            var request = new WeChatPayTransactionsJsApiRequest();

            request.SetBodyModel(model);

            var response = await client.ExecuteAsync(request, weChatPayOptions);

            if (response.StatusCode == 200)
            {
                var req = new WeChatPayJsApiSdkRequest
                {
                    Package = "prepay_id=" + response.PrepayId
                };

                var parameter = await client.ExecuteAsync(req, weChatPayOptions);

                // 将参数(parameter)给 公众号前端
                // https://pay.weixin.qq.com/wiki/doc/apiv3/apis/chapter3_1_4.shtml
                ViewData["parameter"] = parameter.ToJson();
                ViewData["response"] = response.Body;
                return Page();
            }

            ViewData["response"] = response.Body;

            return Page();
        }
    }

    public class WeChatPayPubPayV3ViewModel
    {
        [Required]
        [Display(Name = "out_trade_no")]
        public string OutTradeNo { get; set; } = DateTime.Now.ToString("yyyyMMddHHmmssfff");

        [Required]
        [Display(Name = "description")]
        public string Description { get; set; } = "微信公众号支付测试";

        [Required]
        [Display(Name = "total")]
        public int Total { get; set; } = 1;

        [Required]
        [Display(Name = "notify_url")]
        public string NotifyUrl { get; set; } = "http://domain.com/wechatpay/v3/notify/transactions";

        [Required]
        [Display(Name = "openid")]
        public string OpenId { get; set; }
    }

}
