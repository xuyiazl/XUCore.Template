
namespace XUCore.Template.Mediator.Applaction.Commands;

/// <summary>
/// 导航列表
/// </summary>
public class MenuTreeDto : MenuDto
{
    public IList<MenuTreeDto> Childs { get; set; }
}
