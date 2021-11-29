namespace XUCore.Template.Razor.Core
{
    public class AppSettings
    {
        public string RootUrl { get; set; }
        public string LocalPath => Web.WebRootPath;
        public string Domain { get; set; }
    }

    public static class Root
    {
        public static string DomainUrl { get; set; }
    }
}
