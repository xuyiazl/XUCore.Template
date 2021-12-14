using XUCore.Template.Razor2.Core;
using XUCore.Template.Razor2.Persistence.Entities.Auth;
using XUCore.Template.Razor2.Persistence.Enums;

namespace XUCore.Template.Razor2.DbService.Auth.Role
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
                RuleFor(x => x.Name).NotEmpty().MaximumLength(20).WithName("角色名")
                    .MustAsync(async (name, cancel) =>
                    {
                        var res = await Web.GetRequiredService<IRoleService>().AnyAsync(name, cancel);

                        return !res;
                    })
                    .WithMessage(c => $"该角色名已存在。");
                RuleFor(x => x.Status).IsInEnum().NotEqual(Status.Default).WithName("数据状态");
            }
        }
    }
}
