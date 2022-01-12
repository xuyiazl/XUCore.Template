using XUCore.Template.WeChat.Core;
using XUCore.Template.WeChat.Persistence.Entities;
using XUCore.Template.WeChat.Persistence.Enums;

namespace XUCore.Template.WeChat.DbService.Article
{
    /// <summary>
    /// 文章
    /// </summary>
    public class ArticleDto : DtoBase<ArticleEntity>
    {
        /// <summary>
        /// 文章目录
        /// </summary>
        public long CategoryId { get; set; }
        /// <summary>
        /// 文章目录名
        /// </summary>
        public string CategoryName { get; set; }
        /// <summary>
        /// 标题
        /// </summary>
        public string Title { get; set; }
        /// <summary>
        /// 图片
        /// </summary>
        public string Picture { get; set; }
        /// <summary>
        /// 正文
        /// </summary>
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
        /// 标签集合Id
        /// </summary>
        public IList<long> TagIds { get; set; }
        /// <summary>
        /// 标签集合
        /// </summary>
        public IList<TagSimpleDto> Tags { get; set; }

        public override void Mapping(Profile profile) =>
            profile.CreateMap<ArticleEntity, ArticleDto>()
                .ForMember(c => c.CategoryName, c => c.MapFrom(s => s.Category.Name))
            ;
    }
}
