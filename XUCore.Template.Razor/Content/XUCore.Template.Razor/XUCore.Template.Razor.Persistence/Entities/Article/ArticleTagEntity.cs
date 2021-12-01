
namespace XUCore.Template.Razor.Persistence.Entities
{
    /// <summary>
    /// 文章标签关联表
    /// </summary>
	[Table(Name = "article_tag_nav")]
    [Index("idx_{tablename}_01", nameof(ArticleId) + "," + nameof(TagId), true)]
    public partial class ArticleTagEntity : EntityAdd
    {
        /// <summary>
        /// 文章Id
        /// </summary>
        public long ArticleId { get; set; }
        /// <summary>
        /// 标签Id
        /// </summary>
        public long TagId { get; set; }
        /// <summary>
        /// 对应关联的文章
        /// </summary>
        public ArticleEntity Article { get; set; }
        /// <summary>
        /// 对应关联的标签
        /// </summary>
        public TagEntity Tag { get; set; }
    }
}
