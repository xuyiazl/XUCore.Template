using XUCore.Template.Razor.Core;
using XUCore.Template.Razor.Core.Enums;
using XUCore.Template.Razor.Persistence.Entities.Auth;


namespace XUCore.Template.Razor.DbService.Auth.Role
{
    /// <summary>
    /// 创建角色命令
    /// </summary>
    public class RoleCreateCommand : CreateCommand, IMapFrom<RoleEntity>
    {
        /// <summary>
        /// 角色名
        /// </summary>
        [Required]
        public string Name { get; set; }
        /// <summary>
        /// 导航关联id集合
        /// </summary>
        public long[] MenuIds { get; set; }
        /// <summary>
        /// 状态
        /// </summary>
		public Status Status { get; set; }

        public override bool IsVaild()
        {
            ValidationResult = new Validator().Validate(this);

            return ValidationResult.IsValid;
        }

        public void Mapping(Profile profile) =>
            profile.CreateMap<RoleCreateCommand, RoleEntity>()
            ;

        public class Validator : CommandValidator<RoleCreateCommand>
        {
            public Validator()
            {
                RuleFor(x => x.Name).NotEmpty().MaximumLength(20).WithName("角色名");
                RuleFor(x => x.Status).IsInEnum().NotEqual(Status.Default).WithName("数据状态");
            }
        }
    }
}
