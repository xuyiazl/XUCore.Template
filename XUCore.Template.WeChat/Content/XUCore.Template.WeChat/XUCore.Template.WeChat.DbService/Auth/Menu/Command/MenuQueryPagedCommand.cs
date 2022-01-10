using XUCore.Template.WeChat.Persistence.Enums;

namespace XUCore.Template.WeChat.DbService.Auth.Menu
{
    /// <summary>
    /// 菜单查询命令
    /// </summary>
    public class MenuQueryPagedCommand : PageCommand
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

        public class Validator : CommandPageValidator<MenuQueryPagedCommand, bool>
        {
            public Validator()
            {
                AddPageVaildator();
            }
        }
    }
}

