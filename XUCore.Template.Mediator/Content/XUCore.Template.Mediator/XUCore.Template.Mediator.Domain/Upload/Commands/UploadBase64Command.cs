namespace XUCore.Template.Mediator.Domain;

/// <summary>
/// 上传base64图片
/// </summary>
public class UploadBase64Command : Command<Result<ImageFileInfo>>
{
    [Required]
    public string Base64 { get; set; }
}

internal class UploadBase64CommandValidator : CommandValidator<UploadBase64Command>
{
    public UploadBase64CommandValidator()
    {
        RuleFor(x => x.Base64).NotEmpty().WithName("Base64");
    }
}

internal class UploadBase64CommandHandler : CommandHandler<UploadBase64Command, Result<ImageFileInfo>>
{
    protected readonly IFileUploadService fileUploadService;
    protected readonly IUserInfo user;

    public UploadBase64CommandHandler(IFileUploadService fileUploadService, IMediatorHandler bus, IMapper mapper, IUserInfo user) : base(bus, mapper)
    {
        this.fileUploadService = fileUploadService;
        this.user = user;
    }

    public override async Task<Result<ImageFileInfo>> Handle(UploadBase64Command request, CancellationToken cancellationToken)
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

        result.Url = $"https://www.xx.com/{result.FullPath.Replace("\\", "/")}";

        return RestFull.Success(data: result);
    }
}