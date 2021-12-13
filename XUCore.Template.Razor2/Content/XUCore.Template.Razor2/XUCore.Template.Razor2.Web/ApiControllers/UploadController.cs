using XUCore.Template.Razor2.Applaction.Upload;

namespace XUCore.Template.Razor2.Web.ApiControllers
{
    public class UploadController : ApiControllerBase
    {
        private readonly UploadAppService uploadAppService;

        public UploadController(ILogger<UploadController> logger, UploadAppService uploadAppService)
            : base(logger)
        {
            this.uploadAppService = uploadAppService;
        }

        /// <summary>
        /// 上传图片
        /// </summary> 
        /// <param name="imgFile"></param>
        /// <param name="cutorgin">是否裁剪原图 {cutorgin=true}</param>
        /// <param name="autocutsize">自动适配裁剪尺寸最大宽高{800}</param>
        /// <param name="thumbs">裁剪尺寸{300x300,200x200}</param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> ImageAsync([FromForm] IFormFile imgFile, bool cutorgin, int autocutsize, string thumbs, CancellationToken cancellationToken)
        {
            var res = await uploadAppService.ImageAsync(imgFile, cutorgin, autocutsize, thumbs, cancellationToken);

            return new Result(StateCode.Success, "", "上传成功", res);
        }

        /// <summary>
        /// 上传base64图片
        /// </summary>
        /// <param name="request"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> Base64ImageAsync([FromForm] Base64Command request, CancellationToken cancellationToken)
        {
            var res = await uploadAppService.Base64ImageAsync(request, cancellationToken);

            return new Result(StateCode.Success, "", "上传成功", res);
        }
    }
}