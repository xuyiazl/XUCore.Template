namespace XUCore.Template.Mediator.Domain;

/// <summary>
/// 省市区域更新命令
/// </summary>
public class ChinaAreaUpdateCommand : Command<Result<int>>, IMapFrom<ChinaAreaEntity>
{
    /// <summary>
    /// id
    /// </summary>
    [Required]
    public long Id { get; set; }
    /// <summary>
    /// 上级区域Id
    /// </summary>
    [Required]
    public long ParentId { get; set; }
    /// <summary>
    /// 行政区域等级 1-省 2-市 3-区县 4-街道镇
    /// </summary>
    [Required]
    public short Level { get; set; }
    /// <summary>
    /// 名称
    /// </summary>
    [Required]
    public string Name { get; set; }
    /// <summary>
    /// 完整名称
    /// </summary>
    [Required]
    public string WholeName { get; set; }
    /// <summary>
    /// 本区域经度
    /// </summary>
    [Required]
    public string Lon { get; set; }
    /// <summary>
    /// 本区域维度
    /// </summary>
    [Required]
    public string Lat { get; set; }
    /// <summary>
    /// 电话区号
    /// </summary>
    [Required]
    public string CityCode { get; set; }
    /// <summary>
    /// 邮政编码
    /// </summary>
    [Required]
    public char ZipCode { get; set; }
    /// <summary>
    /// 行政区划代码
    /// </summary>
    [Required]
    public string AreaCode { get; set; }
    /// <summary>
    /// 名称全拼
    /// </summary>
    [Required]
    public string PinYin { get; set; }
    /// <summary>
    /// 首字母简拼
    /// </summary>
    [Required]
    public string SimplePy { get; set; }
    /// <summary>
    /// 区域名称拼音的第一个字母
    /// </summary>
    [Required]
    public char PerPinYin { get; set; }
    /// <summary>
    /// 权重排序
    /// </summary>
    public int Sort { get; set; }

    public void Mapping(Profile profile) =>
        profile.CreateMap<ChinaAreaUpdateCommand, ChinaAreaEntity>()
        ;
}

internal class ChinaAreaUpdateCommandValidator : CommandValidator<ChinaAreaUpdateCommand>
{
    public ChinaAreaUpdateCommandValidator()
    {
        RuleFor(x => x.Id).NotEmpty().WithName("Id不可为空");

        RuleFor(x => x.Name).NotEmpty().MaximumLength(20).WithName("名称");
        RuleFor(x => x.WholeName).NotEmpty().MaximumLength(50).WithName("完整名称");
        RuleFor(x => x.Level).NotEmpty().WithName("行政区域等级");
        RuleFor(x => x.Lon).NotEmpty().WithName("本区域经度");
        RuleFor(x => x.Lat).NotEmpty().WithName("本区域维度");
        RuleFor(x => x.CityCode).NotEmpty().WithName("电话区号");
        RuleFor(x => x.ZipCode).NotEmpty().WithName("邮政编码");
        RuleFor(x => x.AreaCode).NotEmpty().WithName("行政区划代码");
        RuleFor(x => x.PinYin).NotEmpty().WithName("名称全拼");
        RuleFor(x => x.SimplePy).NotEmpty().WithName("首字母简拼");
        RuleFor(x => x.PerPinYin).NotEmpty().WithName("区域名称拼音的第一个字母");
    }
}


internal class ChinaAreaUpdateCommandHandler : CommandHandler<ChinaAreaUpdateCommand, Result<int>>
{
    protected readonly FreeSqlUnitOfWorkManager db;
    protected readonly IUserInfo user;

    public ChinaAreaUpdateCommandHandler(FreeSqlUnitOfWorkManager db, IMediator mediator, IMapper mapper, IUserInfo user) : base(mediator, mapper)
    {
        this.db = db;
        this.user = user;
    }

    public override async Task<Result<int>> Handle(ChinaAreaUpdateCommand request, CancellationToken cancellationToken)
    {
        var repo = db.Orm.GetRepository<ChinaAreaEntity>();

        var entity = await repo.Select.WhereDynamic(request.Id).ToOneAsync<ChinaAreaEntity>(cancellationToken);

        if (entity == null)
            return RestFull.Fail(data: 0);

        entity = mapper.Map(request, entity);

        var res = await repo.UpdateAsync(entity, cancellationToken);

        if (res > 0)
            return RestFull.Success(data: res);
        else
            return RestFull.Fail(data: res);
    }
}