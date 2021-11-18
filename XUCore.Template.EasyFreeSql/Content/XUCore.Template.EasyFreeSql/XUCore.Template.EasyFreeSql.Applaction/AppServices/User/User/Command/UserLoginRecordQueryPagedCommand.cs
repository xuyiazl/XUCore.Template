namespace XUCore.Template.EasyFreeSql.Applaction.User.User
{
    /// <summary>
    /// 查询命令
    /// </summary>
    public class UserLoginRecordQueryPagedCommand : PageCommand
    {
        /// <summary>
        /// 搜索关键字
        /// </summary>
        public string Keyword { get; set; }

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
        public class Validator : CommandPageValidator<UserLoginRecordQueryPagedCommand, bool>
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
