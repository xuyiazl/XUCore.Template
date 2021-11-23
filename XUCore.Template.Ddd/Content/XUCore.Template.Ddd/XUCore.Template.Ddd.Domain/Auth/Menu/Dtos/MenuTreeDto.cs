namespace XUCore.Template.Ddd.Domain.Auth.Menu
{
    public class MenuTreeDto : MenuDto
    {
        public IList<MenuTreeDto> Child { get; set; }
    }
}
