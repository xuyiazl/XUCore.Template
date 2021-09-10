using Microsoft.AspNetCore.Http;
using System.Threading;
using System.Threading.Tasks;
using XUCore.NetCore;
using XUCore.NetCore.Uploads.Params;

namespace XUCore.Template.EasyFreeSql.Applaction.Upload
{
    /// <summary>
    /// 文件上传
    /// </summary>
    public interface IUploadAppService : IAppService
    {
        Task<Result<XUCore.Files.FileInfo>> File(IFormFile formFile, CancellationToken cancellationToken);
        Task<Result<ImageFileInfo>> Image(IFormFile formFile, CancellationToken cancellationToken);
        Task<Result<ImageFileInfo>> Base64(Base64Command request, CancellationToken cancellationToken);
    }
}
