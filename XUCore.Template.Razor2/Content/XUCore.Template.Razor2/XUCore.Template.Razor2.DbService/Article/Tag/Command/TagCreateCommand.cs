
using XUCore.Template.Razor2.Core;
using XUCore.Template.Razor2.Persistence.Entities;
using XUCore.Template.Razor2.Persistence.Enums;

namespace XUCore.Template.Razor2.DbService.Article
{
    /// <summary>
    /// 创建标签命令
    /// </summary>
    public class TagCreateCommand : CreateCommand, IMapFrom<TagEntity>
    {
        /// <summary>
        /// 标签名
        /// </summary>
        [Required]
        public string Name { get; set; }
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
            profile.CreateMap<TagCreateCommand, TagEntity>()
            ;

        public class Validator : CommandValidator<TagCreateCommand>
        {
            public Validator()
            {
                RuleFor(x => x.Name).NotEmpty().MaximumLength(20).WithName("标签名")
                    .MustAsync(async (name, cancel) =>
                    {
                        var res = await Web.GetRequiredService<ITagService>().AnyAsync(name, cancel);

                        return !res;
                    })
                    .WithMessage(c => $"该标签名已存在。");
                RuleFor(x => x.Status).IsInEnum().NotEqual(Status.Default).WithName("数据状态");
            }
        }
    }
}
