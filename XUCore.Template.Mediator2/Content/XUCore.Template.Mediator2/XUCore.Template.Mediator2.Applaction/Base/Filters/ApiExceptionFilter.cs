namespace XUCore.Template.Mediator2.Applaction;

/// <summary>
/// API错误日志过滤器
/// </summary>
public class ApiExceptionFilter : IExceptionFilter
{
    /// <summary>
    /// 异常处理
    /// </summary>
    /// <param name="context">异常上下文</param>
    public void OnException(ExceptionContext context)
    {
        if (context == null)
            return;

        var result = new Result<object>();

        if (context.Exception is OperationCanceledException)
        {
            result = RestFull.Fail<object>(SubCode.Cancel, R.CanceledMessage);
        }
        else if (context.Exception is UnauthorizedAccessException)
        {
            result = RestFull.Fail<object>(SubCode.Unauthorized, context.Exception.Message);
        }
        else if (context.Exception.IsFailure())
        {
            var ex = context.Exception as XUCore.Ddd.Domain.ValidationException;

            var message = ex.Failures.Select(c => c.Value.Join("")).Join("");

            result = RestFull.Fail<object>(SubCode.ValidError, message);
        }
        else
        {
            var logger = context.HttpContext.RequestServices.GetRequiredService<ILogger<ApiExceptionFilter>>();

            if (logger.IsEnabled(LogLevel.Error))
            {
                var routes = context.GetRouteValues().Select(c => $"{c.Key}={c.Value}").Join("，");

                var str = new Str();
                str.AppendLine("WebApi全局异常");
                str.AppendLine($"路由信息：{routes}");
                str.AppendLine($"IP：{context.HttpContext.Connection.RemoteIpAddress}");
                str.AppendLine($"请求方法：{context.HttpContext.Request.Method}");
                str.AppendLine($"请求地址：{context.HttpContext.Request.Scheme}://{context.HttpContext.Request.Host}{context.HttpContext.Request.Path}{context.HttpContext.Request.QueryString}");
                logger.LogError(context.Exception.FormatMessage(str.ToString()));
            }

            result = RestFull.Fail<object>(SubCode.Fail, context.Exception.Message);
        }

        context.Result = new ObjectResult(result)
        {
            StatusCode = StatusCodes.Status200OK
        };

        //base.OnException(context);
    }
}