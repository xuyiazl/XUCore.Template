using XUCore.Template.Razor2.Persistence.Enums;

namespace XUCore.Template.Razor2.DbService.User.User
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
        /// 状态
        /// </summary>
		public Status Status { get; set; }

        public override bool IsVaild()
        {
            ValidationResult = new Validator().Validate(this);

            return ValidationResult.IsValid;
        }

        public class Validator : CommandLimitValidator<UserQueryCommand, bool>
        {
            public Validator()
            {

            }
        }
    }
}
