namespace XUCore.Template.Razor.Core
{
    public class ImageHelper
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
    }
}
