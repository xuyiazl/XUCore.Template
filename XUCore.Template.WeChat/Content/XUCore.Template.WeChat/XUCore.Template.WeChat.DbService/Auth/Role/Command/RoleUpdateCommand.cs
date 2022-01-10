using XUCore.Template.WeChat.Core;
using XUCore.Template.WeChat.Persistence.Entities.Auth;
using XUCore.Template.WeChat.Persistence.Enums;

namespace XUCore.Template.WeChat.DbService.Auth.Role
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
        /// 状态
        /// </summary>
		public Status Status { get; set; }

        public override bool IsVaild()
        {
            ValidationResult = new Validator().Validate(this);

            return ValidationResult.IsValid;
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
                RuleFor(x => x.Status).IsInEnum().NotEqual(Status.Default).WithName("数据状态");
            }
        }
    }
}

