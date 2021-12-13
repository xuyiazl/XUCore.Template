
using XUCore.Template.Razor2.Core;
using XUCore.Template.Razor2.Persistence.Entities;
using XUCore.Template.Razor2.Persistence.Enums;

namespace XUCore.Template.Razor2.DbService.Article
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
                RuleFor(x => x.Name).NotEmpty().WithName("目录名");
                RuleFor(x => x.Status).IsInEnum().NotEqual(Status.Default).WithName("数据状态");
            }
        }
    }
}
