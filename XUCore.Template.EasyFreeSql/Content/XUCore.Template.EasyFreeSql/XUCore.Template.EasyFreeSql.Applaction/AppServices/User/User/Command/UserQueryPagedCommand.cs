namespace XUCore.Template.EasyFreeSql.Applaction.User.User
{
    /// <summary>
    /// 用户查询分页
    /// </summary>
    public class UserQueryPagedCommand : PageCommand
    {
        /// <summary>
        /// 搜索关键字
        /// </summary>
        public string Keyword { get; set; }
        /// <summary>
        /// 启用
        /// </summary>s
		public bool Enabled { get; set; } = true;

        /// <summary>
        /// 验证
        /// </summary>
        public override bool IsVaild()
        {
            ValidationResult = new Validator().Validate(this);

            return ValidationResult.ThrowValidation();
        }

        /// <summary>
        /// 验证
        /// </summary>
        public class Validator : CommandPageValidator<UserQueryPagedCommand, bool>
        {
            /// <summary>
            /// 验证
            /// </summary>
            public Validator()
            {
                AddPageVaildator();
            }
        }
    }
}
