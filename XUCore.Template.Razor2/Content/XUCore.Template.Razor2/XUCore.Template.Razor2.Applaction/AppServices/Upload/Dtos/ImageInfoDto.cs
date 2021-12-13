namespace XUCore.Template.Razor2.Applaction.Upload
{
    /// <summary>
    /// 图片信息
    /// </summary>
    public class ImageInfoDto
    {
        /// <summary>
        /// 文件名
        /// </summary>
        public string FileName { get; set; }
        /// <summary>
        /// Url
        /// </summary>
        public string Url { get; set; }
        /// <summary>
        /// Root
        /// </summary>
        public string Root { get; set; }
        /// <summary>
        /// 完整Url
        /// </summary>
        public string RootUrl { get; set; }
        /// <summary>
        /// 缩略图
        /// </summary>
        public Dictionary<string, string> Thumbs { get; set; }
    }
}
