using System.Collections.Generic;

namespace XUCore.Template.EasyFreeSql.Applaction.User.Menu
{
    /// <summary>
    /// 导航列表
    /// </summary>
    public class MenuTreeDto : MenuDto
    {
        public IList<MenuTreeDto> Childs { get; set; }
    }
}
