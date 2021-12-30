namespace XUCore.Template.Mediator.Domain;

/// <summary>
/// 上传From文件
/// </summary>
public class UploadFileCommand : Command<Result<XUCore.Files.FileInfo>>
{
    [Required]
    public IFormFile FormFile { get; set; }
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