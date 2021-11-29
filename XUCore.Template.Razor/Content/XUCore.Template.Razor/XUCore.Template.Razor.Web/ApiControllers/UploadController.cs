using XUCore.NetCore.Controllers;
using XUCore.Template.Razor.Applaction.Upload;

namespace XUCore.Template.Razor.ApiControllers
{
    public class UploadController : ApiControllerBase
    {
        private readonly AppSettings appSettings;
        private UploadAppService uploadAppService;

        public UploadController(ILogger<UploadController> logger, UploadAppService uploadAppService, AppSettings appSettings)
        : base(logger)
        {
            this.uploadAppService = uploadAppService;
            this.appSettings = appSettings;
        }

        /// <summary>
        /// 上传图片
        /// </summary> 
        /// <param name="imgFile"></param>
        /// <param name="cutorgin">是否裁剪原图 {cutorgin=true}</param>
        /// <param name="autocutsize">自动适配裁剪尺寸最大宽高{800}</param>
        /// <param name="thumbs">裁剪尺寸{300x300,200x200}</param>
        /// <param name="editor">源自哪个编辑器</param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> Image([FromForm] IFormFile imgFile, bool cutorgin, int autocutsize, string thumbs, string editor, CancellationToken cancellationToken)
        {
            var result = await uploadAppService.Image(imgFile, cutorgin, autocutsize, thumbs, cancellationToken);

            return new Result(StateCode.Success, "", "上传成功", new
            {
                FileName = result.Info.FileName,
                Url = result.Info.Url,
                Root = appSettings.RootUrl,
                RootUrl = result.RootUrl,
                Thumbs = result.Info.Thumbs
            });
        }

        /// <summary>
        /// 上传base64图片
        /// </summary>
        /// <param name="request"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> Base64Async([FromForm] Base64Command request, CancellationToken cancellationToken)
        {
            var result = await uploadAppService.Base64(request, cancellationToken);

            return new Result(StateCode.Success, "", "上传成功", new
            {
                FileName = result.Info.FileName,
                Url = result.Info.Url,
                Root = appSettings.RootUrl,
                RootUrl = result.RootUrl
            });
        }
    }
}