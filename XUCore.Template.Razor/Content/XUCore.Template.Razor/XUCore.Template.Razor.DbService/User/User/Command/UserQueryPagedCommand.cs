using XUCore.Template.Razor.Persistence.Enums;

namespace XUCore.Template.Razor.DbService.User.User
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
        /// 状态
        /// </summary>
		public Status Status { get; set; }
        /// <summary>
        /// 排序
        /// </summary>
        public string OrderBy { get; set; }

        public override bool IsVaild()
        {
            ValidationResult = new Validator().Validate(this);

            return ValidationResult.IsValid;
        }

        public class Validator : CommandPageValidator<UserQueryPagedCommand, bool>
        {
            public Validator()
            {
                AddPageVaildator();
            }
        }
    }
}
