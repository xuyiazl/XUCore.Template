﻿
using XUCore.Template.Razor.Core;
using XUCore.Template.Razor.Persistence.Entities;
using XUCore.Template.Razor.Persistence.Enums;

namespace XUCore.Template.Razor.DbService.Article
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
                RuleFor(x => x.Name).NotEmpty().WithName("标签名");
                RuleFor(x => x.Status).IsInEnum().NotEqual(Status.Default).WithName("数据状态");
            }
        }
    }
}
