namespace XUCore.Template.Razor.DbService.Auth.Menu
{
    /// <summary>
    /// 导航列表
    /// </summary>
    public class MenuTreeDto : MenuDto
    {
        public IList<MenuTreeDto> Childs { get; set; }
    }
}
