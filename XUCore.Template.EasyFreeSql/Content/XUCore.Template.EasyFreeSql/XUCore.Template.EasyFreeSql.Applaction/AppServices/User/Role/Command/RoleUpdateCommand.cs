using XUCore.Template.EasyFreeSql.Core;
using XUCore.Template.EasyFreeSql.Persistence.Entities.User;

namespace XUCore.Template.EasyFreeSql.Applaction.User.Role
{
    /// <summary>
    /// 角色修改命令
    /// </summary>
    public class RoleUpdateCommand : UpdateCommand<long>, IMapFrom<RoleEntity>
    {
        /// <summary>
        /// 角色名
        /// </summary>
        [Required]
        public string Name { get; set; }
        /// <summary>
        /// 导航id集合
        /// </summary>
        public long[] MenuIds { get; set; }
        /// <summary>
        /// 启用
        /// </summary>s
		public bool Enabled { get; set; } = true;

        public override bool IsVaild()
        {
            ValidationResult = new Validator().Validate(this);

            return ValidationResult.ThrowValidation();
        }

        public void Mapping(Profile profile) =>
            profile.CreateMap<RoleUpdateCommand, RoleEntity>()
            ;

        public class Validator : CommandIdValidator<RoleUpdateCommand, bool, long>
        {
            public Validator()
            {
                AddIdValidator();

                RuleFor(x => x.Name).NotEmpty().MaximumLength(20).WithName("角色名");
            }
        }
    }
}
