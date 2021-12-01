using XUCore.Template.Razor.Persistence.Enums;

namespace XUCore.Template.Razor.DbService.Auth.Menu
{
    /// <summary>
    /// 菜单查询命令
    /// </summary>
    public class MenuQueryCommand : ListCommand
    {
        /// <summary>
        /// 是否是导航
        /// </summary>
        public bool IsMenu { get; set; }
        /// <summary>
        /// 状态
        /// </summary>
		public Status Status { get; set; }

        public override bool IsVaild()
        {
            ValidationResult = new Validator().Validate(this);

            return ValidationResult.IsValid;
        }

        public class Validator : CommandLimitValidator<MenuQueryCommand, bool>
        {
            public Validator()
            {

            }
        }
    }
}
