
using XUCore.Template.Razor2.Core;
using XUCore.Template.Razor2.Persistence.Entities;
using XUCore.Template.Razor2.Persistence.Enums;

namespace XUCore.Template.Razor2.DbService.Article
{
    /// <summary>
    /// 创建文章命令
    /// </summary>
    public class ArticleCreateCommand : CreateCommand, IMapFrom<ArticleEntity>
    {
        /// <summary>
        /// 文章目录
        /// </summary>
        [Required]
        public long CategoryId { get; set; }
        /// <summary>
        /// 标签集合
        /// </summary>
        public long[] TagIds { get; set; }
        /// <summary>
        /// 标题
        /// </summary>
        [Required]
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
        [Required]
        public Status Status { get; set; }

        public override bool IsVaild()
        {
            ValidationResult = new Validator().Validate(this);

            return ValidationResult.IsValid;
        }

        public void Mapping(Profile profile) =>
            profile.CreateMap<ArticleCreateCommand, ArticleEntity>()
                .ForMember(c => c.UserId, c => c.MapFrom(s => Web.GetService<IUserInfo>().GetId<long>()))
            ;

        public class Validator : CommandValidator<ArticleCreateCommand>
        {
            public Validator()
            {
                RuleFor(x => x.Title).NotEmpty().WithName("文章标题");
                RuleFor(x => x.Status).IsInEnum().NotEqual(Status.Default).WithName("数据状态");
            }
        }
    }
}
