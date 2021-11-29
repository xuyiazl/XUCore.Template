using XUCore.Template.Razor.Core;

namespace XUCore.Template.Razor.Applaction.Upload
{
    /// <summary>
    /// 文件上传
    /// </summary>
    public class UploadAppService : IScoped
    {
        /// <summary>
        /// 文件上传服务
        /// </summary>
        private readonly IFileUploadService fileUploadService;
        /// <summary>
        /// 配置
        /// </summary>
        private readonly AppSettings appSettings;

        public UploadAppService(IFileUploadService fileUploadService, AppSettings appSettings)
        {
            this.fileUploadService = fileUploadService;
            this.appSettings = appSettings;
        }
        /// <summary>
        /// 上传文件
        /// </summary>
        /// <param name="formFile"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task<XUCore.Files.FileInfo> File(IFormFile formFile, CancellationToken cancellationToken)
        {
            var param = new SingleFileUploadParam()
            {
                Request = Web.Request,
                FormFile = formFile,
                RootPath = Web.WebRootPath,
                Module = "upload",
                Group = "file"
            };

            var result = await fileUploadService.UploadAsync(param, cancellationToken);

            result.Url = $"/{result.FullPath.Replace("\\", "/")}";

            return result;
        }
        /// <summary>
        /// 上传图片
        /// </summary>
        /// <param name="formFile"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task<ImageFileInfo> Image(IFormFile formFile, CancellationToken cancellationToken)
        {
            var param = new SingleImageUploadParam()
            {
                Request = Web.Request,
                FormFile = formFile,
                RootPath = Web.WebRootPath,
                Module = "upload",
                Group = "image",

                //*******裁剪和等比缩放 二者取其一*******
                //是否开自动裁剪原图（根据自定大小，自动对宽高裁剪）
                //IsCutOriginal = true,
                //AutoCutSize = 800,

                //是否开启等比缩放原图（根据等比大小和压缩质量裁剪）
                IsZoomOriginal = true,
                Ratio = 50,
                Quality = 100,

                //ThumbCutMode = ThumbnailMode.Cut,
                //Thumbs = new List<string> { "200x300", "400x200" },
            };

            var result = await fileUploadService.UploadImageAsync(param, cancellationToken);

            result.Url = $"/{result.FullPath.Replace("\\", "/")}";

            return result;
        }
        /// <summary>
        /// 上传图片
        /// </summary>
        /// <param name="request"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task<ImageFileInfo> Base64([Required][FromBody] Base64Command request, CancellationToken cancellationToken)
        {
            var param = new SingleImageBase64UploadParam()
            {
                Base64String = request.Base64,
                RootPath = Web.WebRootPath,
                Module = "upload",
                Group = "image",

                //*******裁剪和等比缩放 二者取其一*******
                //是否开自动裁剪原图（根据自定大小，自动对宽高裁剪）
                //IsCutOriginal = true, 
                //AutoCutSize = 800,

                //是否开启等比缩放原图（根据等比大小和压缩质量裁剪）
                IsZoomOriginal = true,
                Ratio = 50,
                Quality = 100,

                //ThumbCutMode = ThumbnailMode.Cut,
                //Thumbs = new List<string> { "200x200", "400x400" },
            };

            var result = await fileUploadService.UploadImageAsync(param, cancellationToken);

            result.Url = $"/{result.FullPath.Replace("\\", "/")}";

            return result;
        }
    }
}
