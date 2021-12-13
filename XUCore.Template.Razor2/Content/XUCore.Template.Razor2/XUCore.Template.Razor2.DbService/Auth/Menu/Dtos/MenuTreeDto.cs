namespace XUCore.Template.Razor2.DbService.Auth.Menu
{
    /// <summary>
    /// 导航列表
    /// </summary>
    public class MenuTreeDto : MenuDto
    {
        public IList<MenuTreeDto> Childs { get; set; }
    }
}
