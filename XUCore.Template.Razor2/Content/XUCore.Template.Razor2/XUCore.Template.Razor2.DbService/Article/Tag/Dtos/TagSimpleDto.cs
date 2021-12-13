using XUCore.Template.Razor2.Core;
using XUCore.Template.Razor2.Persistence.Entities;
using XUCore.Template.Razor2.Persistence.Enums;

namespace XUCore.Template.Razor2.DbService.Article
{
    /// <summary>
    /// 标签
    /// </summary>
    public class TagSimpleDto : DtoKeyBase<TagEntity>
    {
        /// <summary>
        /// 标签名
        /// </summary>
        public string Name { get; set; }
    }
}
