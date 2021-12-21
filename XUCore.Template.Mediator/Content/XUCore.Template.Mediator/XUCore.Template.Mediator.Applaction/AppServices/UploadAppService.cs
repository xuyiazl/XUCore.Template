﻿namespace XUCore.Template.Mediator.Applaction;

/// <summary>
/// 文件上传
/// </summary>
[ApiExplorerSettings(GroupName = ApiGroup.Admin)]
[DynamicWebApi]
public class UploadAppService : IDynamicWebApi
{
    protected readonly IMediatorHandler bus;
    public UploadAppService(IServiceProvider serviceProvider)
    {
        this.bus = serviceProvider.GetRequiredService<IMediatorHandler>();
    }
    /// <summary>
    /// 上传文件
    /// </summary>
    /// <param name="formFile"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public async Task<Result<XUCore.Files.FileInfo>> File([Required] IFormFile formFile, CancellationToken cancellationToken)
        => await bus.SendCommand(new UploadFileCommand { FormFile = formFile }, cancellationToken);
    /// <summary>
    /// 上传图片
    /// </summary>
    /// <param name="formFile"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public async Task<Result<ImageFileInfo>> Image([Required] IFormFile formFile, CancellationToken cancellationToken)
        => await bus.SendCommand(new UploadImageCommand { FormFile = formFile }, cancellationToken);
    /// <summary>
    /// 上传图片
    /// </summary>
    /// <param name="request"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public async Task<Result<ImageFileInfo>> Base64([Required][FromBody] UploadBase64Command request, CancellationToken cancellationToken)
        => await bus.SendCommand(request, cancellationToken);
}
