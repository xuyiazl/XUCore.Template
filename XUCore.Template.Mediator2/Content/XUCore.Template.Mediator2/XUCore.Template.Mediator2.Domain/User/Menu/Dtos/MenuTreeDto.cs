namespace XUCore.Template.Mediator2.Domain;

/// <summary>
/// 导航列表
/// </summary>
public class MenuTreeDto : MenuDto
{
    public IList<MenuTreeDto> Childs { get; set; }
}
