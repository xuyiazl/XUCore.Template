using XUCore.Template.Razor.Persistence.Enums;

namespace XUCore.Template.Razor.Persistence.Entities
{
    /// <summary>
    /// 文章目录表
    /// </summary>
	[Table(Name = "article_category")]
    [Index("idx_{tablename}_01", nameof(Name), true)]
    public partial class CategoryEntity : EntityFull
    {
        /// <summary>
        /// 目录名
        /// </summary>
        [Column(StringLength = 50)]
        public string Name { get; set; }
        /// <summary>
        /// 排序权重
        /// </summary>
        public int Weight { get; set; }
        /// <summary>
        /// 状态
        /// </summary>
		public Status Status { get; set; }
        /// <summary>
        /// 对应关联的文章
        /// </summary>
        public ArticleEntity Article { get; set; }
    }
}
