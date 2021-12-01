﻿using XUCore.Template.Razor.Persistence.Enums;

namespace XUCore.Template.Razor.Persistence.Entities
{
    /// <summary>
    /// 文章标签
    /// </summary>
	[Table(Name = "article_tag")]
    [Index("idx_{tablename}_01", nameof(Name), true)]
    public partial class TagEntity : EntityFull
    {
        public TagEntity()
        {
            ArticleTags = new List<ArticleTagEntity>();
        }
        /// <summary>
        /// 标签名
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
        /// 文章标签关联列表
        /// </summary>
        [Navigate(ManyToMany = typeof(ArticleTagEntity))]
        public ICollection<ArticleTagEntity> ArticleTags { get; set; }
    }
}
