using XUCore.Template.WeChat.Persistence.Enums;

namespace XUCore.Template.WeChat.DbService.Notice
{
    /// <summary>
    /// 公告查询命令
    /// </summary>
    public class NoticeQueryPagedCommand : PageCommand
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

        public class Validator : CommandPageValidator<NoticeQueryPagedCommand, bool>
        {
            public Validator()
            {
                AddPageVaildator();
            }
        }
    }
}

