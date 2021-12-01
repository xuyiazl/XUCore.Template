namespace XUCore.Template.Razor.Core
{
    public class AppSettings
    {
        public string RootUrl { get; set; }
        public string LocalPath => Web.WebRootPath;
        public string Domain { get; set; }

        public WebSite WebSite { get; set; }
    }

    public class WebSite
    {
        /// <summary>
        /// 网站标题
        /// </summary>
        public string Title { get; set; }
        /// <summary>
        /// 网站标题简写
        /// </summary>
        public string SimpleTitle { get; set; }
        /// <summary>
        /// 网站Logo
        /// </summary>
        public string Logo { get; set; }
        /// <summary>
        /// 备案号
        /// </summary>
        public string AQ { get; set; }
    }

    public static class GlobalStatic
    {
        public static string RootUrl { get; set; }
        public static WebSite WebSite { get; set; }
    }
}
