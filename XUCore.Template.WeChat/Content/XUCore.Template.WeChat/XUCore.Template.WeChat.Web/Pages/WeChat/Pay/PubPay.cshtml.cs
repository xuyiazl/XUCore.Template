using Essensoft.Paylink.WeChatPay;
using Essensoft.Paylink.WeChatPay.V3;
using Essensoft.Paylink.WeChatPay.V3.Domain;
using Essensoft.Paylink.WeChatPay.V3.Request;

namespace XUCore.Template.WeChat.Web.Pages.WeChat.Pay
{
    public class PubPayModel : WeChatPageModel
    {
        private readonly IWeChatPayClient client;
        private readonly WeChatPayOptions weChatPayOptions;

        public PubPayModel(IWeChatPayClient client, WeChatPayOptions weChatPayOptions)
        {
            this.client = client;
            this.weChatPayOptions = weChatPayOptions;
        }

        public void OnGet()
        {
        }

        /// <summary>
        /// 公众号支付-JSAPI下单
        /// </summary>
        /// <param name="viewModel"></param>
        public async Task<IActionResult> OnPostPubPayAsync()
        {
            var order = new
            {
                OutTradeNo = Str.CreateOrderNumber("N"),
                Total = 1,
                Description = "微信公众号支付测试",
                NotifyUrl = "http://test.shougolf.com/wechatpay/v3/notify/transactions",
            };

            var model = new WeChatPayTransactionsJsApiBodyModel
            {
                AppId = weChatPayOptions.AppId,
                MchId = weChatPayOptions.MchId,
                Amount = new Amount { Total = order.Total, Currency = "CNY" },
                Description = order.Description,
                NotifyUrl = order.NotifyUrl,
                OutTradeNo = order.OutTradeNo,
                Payer = new PayerInfo { OpenId = OpenId }
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
                //ViewData["parameter"] = parameter.ToJson();
                //ViewData["response"] = response.Body;

                return new Result(StateCode.Success, "", "", parameter);
            }

            //ViewData["response"] = response.Body;

            return new Result(StateCode.Fail, "", "", new WeChatPayDictionary());
        }
    }

}
