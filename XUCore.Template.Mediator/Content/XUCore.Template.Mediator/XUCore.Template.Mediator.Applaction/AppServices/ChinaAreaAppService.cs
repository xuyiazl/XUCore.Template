namespace XUCore.Template.Mediator.Applaction;

/// <summary>
/// 城市区域管理
/// </summary>
[ApiExplorerSettings(GroupName = ApiGroup.Admin)]
[DynamicWebApi]
public class ChinaAreaAppService : IDynamicWebApi
{
    protected readonly IMediator mediator;

    public ChinaAreaAppService(IServiceProvider serviceProvider)
    {
        this.mediator = serviceProvider.GetRequiredService<IMediator>();
    }
    /// <summary>
    /// 创建城市区域
    /// </summary>
    /// <param name="request"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public async Task<Result<long>> CreateAsync([Required][FromBody] ChinaAreaCreateCommand request, CancellationToken cancellationToken = default)
        => await mediator.Send(request, cancellationToken);
    /// <summary>
    /// 更新城市区域信息
    /// </summary>
    /// <param name="request"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public async Task<Result<int>> UpdateAsync([Required][FromBody] ChinaAreaUpdateCommand request, CancellationToken cancellationToken = default)
        => await mediator.Send(request, cancellationToken);
    /// <summary>
    /// 删除城市区域（物理删除）
    /// </summary>
    /// <param name="request"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public async Task<Result<int>> DeleteAsync([Required][FromQuery] ChinaAreaDeleteCommand request, CancellationToken cancellationToken = default)
        => await mediator.Send(request, cancellationToken);
    /// <summary>
    /// 获取城市区域信息
    /// </summary>
    /// <param name="id"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    [HttpGet("{id}")]
    public async Task<Result<ChinaAreaDto>> GetAsync([Required] long id, CancellationToken cancellationToken = default)
        => await mediator.Send(new ChinaAreaQueryByIdCommand { Id = id }, cancellationToken);
    /// <summary>
    /// 获取城市区域树形结构
    /// </summary>
    /// <param name="request"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public async Task<Result<List<ChinaAreaTreeDto>>> GetTreeAsync(ChinaAreaQueryTreeCommand request, CancellationToken cancellationToken = default)
        => await mediator.Send(request, cancellationToken);
    /// <summary>
    /// 获取城市区域列表
    /// </summary>
    /// <param name="request"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public async Task<Result<List<ChinaAreaDto>>> GetListAsync([Required][FromQuery] ChinaAreaQueryListCommand request, CancellationToken cancellationToken = default)
        => await mediator.Send(request, cancellationToken);
}
