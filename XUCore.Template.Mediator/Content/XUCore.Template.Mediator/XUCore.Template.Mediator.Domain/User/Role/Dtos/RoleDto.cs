namespace XUCore.Template.Mediator.Domain;

/// <summary>
/// 角色
/// </summary>
public class RoleDto : DtoBase<RoleEntity>
{
    /// <summary>
    /// 角色名
    /// </summary>
    public string Name { get; set; }
}
