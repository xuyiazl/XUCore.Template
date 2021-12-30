namespace XUCore.Template.Mediator2.Applaction;

/// <summary>
/// 上传服务
/// </summary>
[ApiExplorerSettings(GroupName = ApiGroup.Admin)]
[DynamicWebApi(Name = "Upload")]
public class UploadFileCommand : Command<Result<XUCore.Files.FileInfo>>, IDynamicWebApi
{
    [Required]
    public IFormFile FormFile { get; set; }

    /// <summary>
    /// 上传文件
    /// </summary>
    /// <param name="mediator"></param>
    /// <param name="formFile"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public async Task<Result<XUCore.Files.FileInfo>> FileAsync([FromServices] IMediator mediator, [Required] IFormFile formFile, CancellationToken cancellationToken)
        => await mediator.Send(new UploadFileCommand { FormFile = formFile }, cancellationToken);
}

public class UploadFileCommandValidator : CommandValidator<UploadFileCommand>
{
    public UploadFileCommandValidator()
    {

    }
}

public class UploadFileCommandHandler : CommandHandler<UploadFileCommand, Result<XUCore.Files.FileInfo>>
{
    protected readonly IFileUploadService fileUploadService;
    protected readonly IUserInfo user;

    public UploadFileCommandHandler(IFileUploadService fileUploadService, IMediator mediator, IMapper mapper, IUserInfo user) : base(mediator, mapper)
    {
        this.fileUploadService = fileUploadService;
        this.user = user;
    }

    public override async Task<Result<XUCore.Files.FileInfo>> Handle(UploadFileCommand request, CancellationToken cancellationToken)
    {
        var param = new SingleFileUploadParam()
        {
            Request = Web.Request,
            FormFile = request.FormFile,
            RootPath = Web.WebRootPath,
            Module = "upload",
            Group = "file"
        };

        var result = await fileUploadService.UploadAsync(param, cancellationToken);

        result.Url = $"https://www.xx.com/{result.FullPath.Replace("\\", "/")}";

        return RestFull.Success(data: result);
    }
}