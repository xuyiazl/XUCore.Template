namespace XUCore.Template.Razor.Core
{
    public class AppSettings
    {
        public string RootUrl { get; set; }
        public string LocalPath => Web.WebRootPath;
        public string Domain { get; set; }
    }

    public static class GlobalStatic
    {
        public static string RootUrl { get; set; }
    }
}
