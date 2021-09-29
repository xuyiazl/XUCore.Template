using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace XUCore.Template.Layer.WebApi.Controller
{
    [ApiController]
    [Route("api/[controller]")]
    public class ApiControllerBase : ControllerBase
    {
        public ApiControllerBase(ILogger logger)
        {
            _logger = logger;
        }

        /// <summary>
        /// 日志
        /// </summary>
        public ILogger _logger;
    }
}
