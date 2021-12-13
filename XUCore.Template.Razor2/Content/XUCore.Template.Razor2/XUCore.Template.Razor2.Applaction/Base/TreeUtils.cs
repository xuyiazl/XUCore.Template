using XUCore.Template.Razor2.DbService.Auth.Menu;

namespace XUCore.Template.Razor2.Applaction
{
    public static class TreeUtils
    {
        public static IList<SelectListItem> AuthMenusSelectItems(IList<MenuDto> entities)
        {
            List<SelectListItem> newMenu = new List<SelectListItem>();

            AuthMenusSelectItems(entities, newMenu, 0, "");

            newMenu.Insert(0, new SelectListItem { Value = "0", Text = "·顶级导航" });

            return newMenu;
        }

        private static void AuthMenusSelectItems(IList<MenuDto> entities, IList<SelectListItem> newMenu, long parentNo, string str)
        {
            IList<MenuDto> foundRows = entities.Where(c => c.ParentId == parentNo).ToList();
            if (parentNo > 0)
                str = str + "　";
            for (int i = 0; i < foundRows.Count; i++)
            {
                string name = string.Empty;
                if (parentNo == 0)
                    name = str + "·" + foundRows[i].Name;
                else
                    name = str + "-" + foundRows[i].Name;
                newMenu.Add(new SelectListItem { Value = foundRows[i].Id.ToString(), Text = name });
                AuthMenusSelectItems(entities, newMenu, foundRows[i].Id, str);
            }
        }

        public static IList<MenuDto> AuthMenusTree(IList<MenuDto> entities, bool isList)
        {
            IList<MenuDto> menus = new List<MenuDto>();

            AuthMenusTree(entities, menus, 0, "", isList);

            return menus;
        }

        private static void AuthMenusTree(IList<MenuDto> entities, IList<MenuDto> newMenu, long parentNo, string str, bool isList)
        {
            IList<MenuDto> foundRows = entities.Where(c => c.ParentId == parentNo).ToList();
            if (parentNo > 0)
                str = str + "　";
            for (int i = 0; i < foundRows.Count; i++)
            {
                var newRow = foundRows[i].DeepCopyByReflect();
                if (parentNo == 0)
                    newRow.Name = str + "<span class=\"" + newRow.Icon + "\"></span>&nbsp;<span class=\"font-bold\">" + newRow.Name + "</span>";
                else
                {
                    if (newRow.IsMenu)
                        newRow.Name = str + "&emsp;<span class=\"" + newRow.Icon + "\"></span>&nbsp;<span class=\"font-bold\">" + newRow.Name + "</span>";
                    else
                    {
                        if (isList)
                            newRow.Name = str + "&emsp;<span class=\"" + newRow.Icon + "\"></span>&nbsp;" + newRow.Name;
                        else
                            newRow.Name = "<span class=\"" + newRow.Icon + "\"></span>&nbsp;" + newRow.Name;
                    }
                }
                newMenu.Add(newRow);
                AuthMenusTree(entities, newMenu, newRow.Id, str, isList);
            }
        }
    }
}
