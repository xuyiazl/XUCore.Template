
using XUCore.Template.Razor.Core;
using XUCore.Template.Razor.Persistence.Entities;
using XUCore.Template.Razor.Persistence.Enums;

namespace XUCore.Template.Razor.DbService.Article
{
    /// <summary>
    /// 创建目录命令
    /// </summary>
    public class CategoryCreateCommand : CreateCommand, IMapFrom<CategoryEntity>
    {
        /// <summary>
        /// 目录名
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
            profile.CreateMap<CategoryCreateCommand, CategoryEntity>()
            ;

        public class Validator : CommandValidator<CategoryCreateCommand>
        {
            public Validator()
            {
                RuleFor(x => x.Name).NotEmpty().MaximumLength(20).WithName("目录名")
                    .MustAsync(async (name, cancel) =>
                    {
                        var res = await Web.GetRequiredService<ICategoryService>().AnyAsync(name, cancel);

                        return !res;
                    })
                    .WithMessage(c => $"该目录名已存在。");
                RuleFor(x => x.Status).IsInEnum().NotEqual(Status.Default).WithName("数据状态");
            }
        }
    }
}
