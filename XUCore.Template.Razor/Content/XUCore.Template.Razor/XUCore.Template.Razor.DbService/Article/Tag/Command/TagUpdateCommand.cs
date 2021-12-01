using XUCore.Template.Razor.Core;
using XUCore.Template.Razor.Persistence.Entities;
using XUCore.Template.Razor.Persistence.Enums;

namespace XUCore.Template.Razor.DbService.Article
{
    /// <summary>
    /// 标签修改命令
    /// </summary>
    public class TagUpdateCommand : UpdateCommand<long>, IMapFrom<TagEntity>
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
            profile.CreateMap<TagUpdateCommand, TagEntity>()
            ;

        public class Validator : CommandIdValidator<TagUpdateCommand, bool, long>
        {
            public Validator()
            {
                AddIdValidator();

                RuleFor(x => x.Name).NotEmpty().WithName("标签名");
                RuleFor(x => x.Status).IsInEnum().NotEqual(Status.Default).WithName("数据状态");
            }
        }
    }
}
