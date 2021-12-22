namespace XUCore.Template.Mediator.Domain;

/// <summary>
/// 导航列表
/// </summary>
public class MenuTreeDto : MenuDto
{
    public IList<MenuTreeDto> Childs { get; set; }
}
