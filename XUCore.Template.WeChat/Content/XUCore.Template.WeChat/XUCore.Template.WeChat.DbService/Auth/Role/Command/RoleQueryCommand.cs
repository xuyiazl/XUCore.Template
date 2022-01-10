using XUCore.Template.WeChat.Persistence.Enums;

namespace XUCore.Template.WeChat.DbService.Auth.Role
{
    /// <summary>
    /// 角色查询命令
    /// </summary>
    public class RoleQueryCommand : ListCommand
    {
        /// <summary>
        /// 搜索关键字
        /// </summary>
        public string Keyword { get; set; }
        /// <summary>
        /// 状态
        /// </summary>
		public Status Status { get; set; }

        public override bool IsVaild()
        {
            ValidationResult = new Validator().Validate(this);

            return ValidationResult.IsValid;
        }

        public class Validator : CommandLimitValidator<RoleQueryCommand, bool>
        {
            public Validator()
            {

            }
        }
    }
}

