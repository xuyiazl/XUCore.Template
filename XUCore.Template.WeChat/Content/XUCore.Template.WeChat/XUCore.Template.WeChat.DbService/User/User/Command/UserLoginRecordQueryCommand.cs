namespace XUCore.Template.WeChat.DbService.User.User
{
    /// <summary>
    /// 查询命令
    /// </summary>
    public class UserLoginRecordQueryCommand : ListCommand
    {
        /// <summary>
        /// 用户id
        /// </summary>
        public long UserId { get; set; }

        public override bool IsVaild()
        {
            ValidationResult = new Validator().Validate(this);

            return ValidationResult.IsValid;
        }

        public class Validator : CommandLimitValidator<UserLoginRecordQueryCommand, bool>
        {
            public Validator()
            {
            }
        }
    }
}

