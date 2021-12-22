namespace XUCore.Template.Mediator.Domain;

/// <summary>
/// 导航列表
/// </summary>
public class ChinaAreaTreeDto : ChinaAreaDto
{
    /// <summary>
    /// 下级城市
    /// </summary>
    public IList<ChinaAreaTreeDto> Childs { get; set; }
}
