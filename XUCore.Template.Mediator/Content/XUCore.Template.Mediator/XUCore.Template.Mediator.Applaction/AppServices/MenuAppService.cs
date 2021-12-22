namespace XUCore.Template.Mediator.Applaction;

/// <summary>
/// 用户导航管理
/// </summary>
[ApiExplorerSettings(GroupName = ApiGroup.Admin)]
[DynamicWebApi]
public class MenuAppService : IDynamicWebApi
{
    protected readonly IMediator mediator;

    public MenuAppService(IServiceProvider serviceProvider)
    {
        this.mediator = serviceProvider.GetRequiredService<IMediator>();
    }
    /// <summary>
    /// 创建导航
    /// </summary>
    /// <param name="request"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public async Task<Result<long>> CreateAsync([Required][FromBody] MenuCreateCommand request, CancellationToken cancellationToken = default)
        => await mediator.Send(request, cancellationToken);
    /// <summary>
    /// 更新导航信息
    /// </summary>
    /// <param name="request"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public async Task<Result<int>> UpdateAsync([Required][FromBody] MenuUpdateCommand request, CancellationToken cancellationToken = default)
        => await mediator.Send(request, cancellationToken);
    /// <summary>
    /// 更新导航指定字段内容
    /// </summary>
    /// <param name="request"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public async Task<Result<int>> UpdateFieldAsync([Required][FromBody] MenuUpdateFieldCommand request, CancellationToken cancellationToken = default)
        => await mediator.Send(request, cancellationToken);
    /// <summary>
    /// 更新状态
    /// </summary>
    /// <param name="request"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public async Task<Result<int>> UpdateEnabledAsync([Required][FromBody] MenuUpdateStatusCommand request, CancellationToken cancellationToken = default)
        => await mediator.Send(request, cancellationToken);
    /// <summary>
    /// 删除导航（物理删除）
    /// </summary>
    /// <param name="request"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public async Task<Result<int>> DeleteAsync([Required][FromQuery] MenuDeleteCommand request, CancellationToken cancellationToken = default)
        => await mediator.Send(request, cancellationToken);
    /// <summary>
    /// 获取导航信息
    /// </summary>
    /// <param name="id"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    [HttpGet("{id}")]
    public async Task<Result<MenuDto>> GetAsync([Required] long id, CancellationToken cancellationToken = default)
        => await mediator.Send(new MenuQueryByIdCommand { Id = id }, cancellationToken);
    /// <summary>
    /// 获取导航树形结构
    /// </summary>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public async Task<Result<List<MenuTreeDto>>> GetTreeAsync(CancellationToken cancellationToken = default)
        => await mediator.Send(new MenuQueryTreeCommand(), cancellationToken);
    /// <summary>
    /// 获取导航列表
    /// </summary>
    /// <param name="request"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public async Task<Result<List<MenuDto>>> GetListAsync([Required][FromQuery] MenuQueryListCommand request, CancellationToken cancellationToken = default)
        => await mediator.Send(request, cancellationToken);
    /// <summary>
    /// 获取分页
    /// </summary>
    /// <param name="request"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public async Task<Result<PagedModel<MenuDto>>> GetPageAsync([Required][FromQuery] MenuQueryPagedCommand request, CancellationToken cancellationToken = default)
        => await mediator.Send(request, cancellationToken);
}
