
using Essensoft.Paylink.WeChatPay;
using Essensoft.Paylink.WeChatPay.V3;
using Essensoft.Paylink.WeChatPay.V3.Notify;

namespace XUCore.Template.WeChat.Web.ApiControllers
{
    [Route("wechatpay/v3/notify")]
    public class WeChatPayV3NotifyController : ApiControllerBase
    {
        private readonly IWeChatPayNotifyClient client;
        private readonly WeChatPayOptions weChatPayOptions;

        public WeChatPayV3NotifyController(ILogger<WeChatPayV3NotifyController> logger, IWeChatPayNotifyClient client, WeChatPayOptions weChatPayOptions) : base(logger)
        {
            this.client = client;
            this.weChatPayOptions = weChatPayOptions;
        }

        /// <summary>
        /// 支付结果通知
        /// </summary>
        [Route("transactions")]
        [HttpPost]
        public async Task<IActionResult> Transactions()
        {
            try
            {
                var notify = await client.ExecuteAsync<WeChatPayTransactionsNotify>(Request, weChatPayOptions);
                if (notify.TradeState == WeChatPayTradeState.Success)
                {
                    _logger.LogInformation("支付结果通知 => OutTradeNo: " + notify.OutTradeNo);
                    return WeChatPayNotifyResult.Success;
                }

                return WeChatPayNotifyResult.Failure;
            }
            catch (WeChatPayException ex)
            {
                _logger.LogWarning("出现异常: " + ex.Message);
                return WeChatPayNotifyResult.Failure;
            }
        }

        /// <summary>
        /// 退款结果通知
        /// </summary>
        [Route("refund")]
        [HttpPost]
        public async Task<IActionResult> Refund()
        {
            try
            {
                var notify = await client.ExecuteAsync<WeChatPayRefundDomesticRefundsNotify>(Request, weChatPayOptions);
                if (notify.RefundStatus == WeChatPayRefundStatus.Success)
                {
                    _logger.LogInformation("退款结果通知 => OutTradeNo: " + notify.OutTradeNo);
                    return WeChatPayNotifyResult.Success;
                }

                return WeChatPayNotifyResult.Failure;
            }
            catch (WeChatPayException ex)
            {
                _logger.LogWarning("出现异常: " + ex.Message);
                return WeChatPayNotifyResult.Failure;
            }
        }
    }
}
