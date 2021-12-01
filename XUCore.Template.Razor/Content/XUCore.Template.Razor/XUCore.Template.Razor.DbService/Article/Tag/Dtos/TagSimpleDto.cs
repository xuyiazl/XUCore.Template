using XUCore.Template.Razor.Core;
using XUCore.Template.Razor.Persistence.Entities;
using XUCore.Template.Razor.Persistence.Enums;

namespace XUCore.Template.Razor.DbService.Article
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
