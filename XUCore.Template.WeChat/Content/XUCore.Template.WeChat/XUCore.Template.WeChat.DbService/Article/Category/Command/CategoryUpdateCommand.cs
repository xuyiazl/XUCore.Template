using XUCore.Template.WeChat.Core;
using XUCore.Template.WeChat.Persistence.Entities;
using XUCore.Template.WeChat.Persistence.Enums;

namespace XUCore.Template.WeChat.DbService.Article
{
    /// <summary>
    /// 目录修改命令
    /// </summary>
    public class CategoryUpdateCommand : UpdateCommand<long>, IMapFrom<CategoryEntity>
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
            profile.CreateMap<CategoryUpdateCommand, CategoryEntity>()
            ;

        public class Validator : CommandIdValidator<CategoryUpdateCommand, bool, long>
        {
            public Validator()
            {
                AddIdValidator();

                RuleFor(x => x.Name).NotEmpty().WithName("目录名");
                RuleFor(x => x.Status).IsInEnum().NotEqual(Status.Default).WithName("数据状态");
            }
        }
    }
}
