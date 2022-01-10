using XUCore.Template.WeChat.Core;
using XUCore.Template.WeChat.Persistence.Entities;
using XUCore.Template.WeChat.Persistence.Enums;

namespace XUCore.Template.WeChat.DbService.Article
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

