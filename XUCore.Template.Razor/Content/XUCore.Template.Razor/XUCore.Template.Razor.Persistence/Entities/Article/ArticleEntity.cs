using XUCore.Template.Razor.Persistence.Enums;

namespace XUCore.Template.Razor.Persistence.Entities
{
    /// <summary>
    /// 文章表
    /// </summary>
	[Table(Name = "article")]
    //[Index("idx_{tablename}_01", nameof(UserName), true)]
    public partial class ArticleEntity : EntityFull
    {
        public ArticleEntity()
        {
            TagNavs = new List<ArticleTagEntity>();
        }
        /// <summary>
        /// 用户id
        /// </summary>
        public long UserId { get; set; }
        /// <summary>
        /// 文章目录
        /// </summary>
        public long CategoryId { get; set; }
        /// <summary>
        /// 标题
        /// </summary>
        [Column(StringLength = 100)]
        public string Title { get; set; }
        /// <summary>
        /// 图片
        /// </summary>
        [Column(StringLength = 100)]
        public string Picture { get; set; }
        /// <summary>
        /// 正文
        /// </summary>
        [Column(StringLength = -2)]
        public string Contents { get; set; }
        /// <summary>
        /// 排序权重
        /// </summary>
        public int Weight { get; set; }
        /// <summary>
        /// 状态
        /// </summary>
		public Status Status { get; set; }
        /// <summary>
        /// 对应目录
        /// </summary>
        public CategoryEntity Category { get; set; }
        /// <summary>
        /// 对应标签
        /// </summary>
        [Navigate(ManyToMany = typeof(ArticleTagEntity))]
        public ICollection<ArticleTagEntity> TagNavs { get; set; }
    }
}
