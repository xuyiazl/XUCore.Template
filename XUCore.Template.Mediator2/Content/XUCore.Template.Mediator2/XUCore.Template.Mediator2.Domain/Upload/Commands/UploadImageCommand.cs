namespace XUCore.Template.Mediator2.Domain;

/// <summary>
/// 上传FromImage图片
/// </summary>
public class UploadImageCommand : Command<Result<ImageFileInfo>>
{
    [Required]
    public IFormFile FormFile { get; set; }
}


internal class UploadImageCommandValidator : CommandValidator<UploadImageCommand>
{
    public UploadImageCommandValidator()
    {

    }
}

internal class UploadImageCommandHandler : CommandHandler<UploadImageCommand, Result<ImageFileInfo>>
{
    protected readonly IFileUploadService fileUploadService;
    protected readonly IUserInfo user;

    public UploadImageCommandHandler(IFileUploadService fileUploadService, IMediator mediator, IMapper mapper, IUserInfo user) : base(mediator, mapper)
    {
        this.fileUploadService = fileUploadService;
        this.user = user;
    }

    public override async Task<Result<ImageFileInfo>> Handle(UploadImageCommand request, CancellationToken cancellationToken)
    {
        var param = new SingleImageUploadParam()
        {
            Request = Web.Request,
            FormFile = request.FormFile,
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

        result.Url = $"https://www.xx.com/{result.FullPath.Replace("\\", "/")}";

        return RestFull.Success(data: result);
    }
}