namespace XUCore.Template.FreeSql.DbService.User.User
{
    /// <summary>
    /// 用户查询
    /// </summary>
    public class UserQueryCommand : ListCommand
    {
        /// <summary>
        /// 搜索关键字
        /// </summary>
        public string Keyword { get; set; }
        /// <summary>
        /// 启用
        /// </summary>s
		public bool Enabled { get; set; } = true;

        public override bool IsVaild()
        {
            ValidationResult = new Validator().Validate(this);

            return ValidationResult.ThrowValidation();
        }

        public class Validator : CommandLimitValidator<UserQueryCommand, bool>
        {
            public Validator()
            {

            }
        }
    }
}
