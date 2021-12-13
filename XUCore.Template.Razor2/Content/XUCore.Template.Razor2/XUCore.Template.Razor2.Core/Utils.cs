namespace XUCore.Template.Razor2.Core
{
    public class Utils
    {
        public static string Face(string rootUrl, object url, int sex)
        {
            if (url == null || url == DBNull.Value || url.ToString().Length == 0)
            {
                if (sex == 0)
                    return "/images/woman.jpg";
                else
                    return "/images/man.jpg";
            }
            if (((string)url).StartsWith("http"))
            {
                return (string)url;
            }
            return rootUrl + url;
        }

        public static (string Css, string Name) LevelSwap(long levelId)
        {
            return levelId switch
            {
                1 => ("text-dark", "普通"),
                2 => ("text-default", "一般"),
                3 => ("text-danger", "重要"),
                4 => ("text-warning", "紧急"),
                _ => ("text-default", "普通"),
            };
        }
    }
}
