namespace XUCore.Template.Mediator.Applaction;

/// <summary>
/// 用户角色管理
/// </summary>
[ApiExplorerSettings(GroupName = ApiGroup.Admin)]
[DynamicWebApi]
public class RoleAppService : IDynamicWebApi
{
    protected readonly IMediatorHandler bus;
    public RoleAppService(IServiceProvider serviceProvider)
    {
        this.bus = serviceProvider.GetRequiredService<IMediatorHandler>();
    }

    /// <summary>
    /// 创建角色
    /// </summary>
    /// <param name="request"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public async Task<Result<long>> CreateAsync([Required][FromBody] RoleCreateCommand request, CancellationToken cancellationToken = default)
        => await bus.SendCommand(request, cancellationToken);
    /// <summary>
    /// 更新角色信息
    /// </summary>
    /// <param name="request"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public async Task<Result<int>> UpdateAsync([Required][FromBody] RoleUpdateCommand request, CancellationToken cancellationToken = default)
        => await bus.SendCommand(request, cancellationToken);
    /// <summary>
    /// 更新角色指定字段内容
    /// </summary>
    /// <param name="request"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public async Task<Result<int>> UpdateFieldAsync([Required][FromBody] RoleUpdateFieldCommand request, CancellationToken cancellationToken = default)
        => await bus.SendCommand(request, cancellationToken);
    /// <summary>
    /// 更新状态
    /// </summary>
    /// <param name="request"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public async Task<Result<int>> UpdateEnabledAsync([Required][FromQuery] RoleUpdateStatusCommand request, CancellationToken cancellationToken = default)
        => await bus.SendCommand(request, cancellationToken);
    /// <summary>
    /// 删除角色（物理删除）
    /// </summary>
    /// <param name="request"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public async Task<Result<int>> DeleteAsync([Required][FromQuery] RoleDeleteCommand request, CancellationToken cancellationToken = default)
        => await bus.SendCommand(request, cancellationToken);
    /// <summary>
    /// 获取角色信息
    /// </summary>
    /// <param name="id"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    [HttpGet("{id}")]
    public async Task<Result<RoleDto>> GetAsync([Required] long id, CancellationToken cancellationToken = default)
        => await bus.SendCommand(new RoleQueryByIdCommand { Id = id }, cancellationToken);
    /// <summary>
    /// 获取所有角色
    /// </summary>
    /// <param name="request"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public async Task<Result<List<RoleDto>>> GetListAsync([Required][FromQuery] RoleQueryListCommand request, CancellationToken cancellationToken = default)
        => await bus.SendCommand(request, cancellationToken);
    /// <summary>
    /// 获取角色分页
    /// </summary>
    /// <param name="request"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public async Task<Result<PagedModel<RoleDto>>> GetPageAsync([Required][FromQuery] RoleQueryPagedCommand request, CancellationToken cancellationToken = default)
        => await bus.SendCommand(request, cancellationToken);
    /// <summary>
    /// 获取角色关联的所有导航id集合
    /// </summary>
    /// <param name="request"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public async Task<Result<List<long>>> GetRelevanceMenuAsync([Required][FromQuery] RoleQueryRelevanceMenuCommand request, CancellationToken cancellationToken = default)
        => await bus.SendCommand(request, cancellationToken);
}
