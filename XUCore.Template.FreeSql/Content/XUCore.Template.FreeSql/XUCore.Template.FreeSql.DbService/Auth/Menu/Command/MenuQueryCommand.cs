namespace XUCore.Template.FreeSql.DbService.Auth.Menu
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
        /// 启用
        /// </summary>s
		public bool Enabled { get; set; } = true;

        public override bool IsVaild()
        {
            ValidationResult = new Validator().Validate(this);

            return ValidationResult.ThrowValidation();
        }

        public class Validator : CommandLimitValidator<MenuQueryCommand, bool>
        {
            public Validator()
            {

            }
        }
    }
}
