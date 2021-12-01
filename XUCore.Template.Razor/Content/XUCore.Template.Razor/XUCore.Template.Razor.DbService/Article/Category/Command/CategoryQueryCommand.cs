using XUCore.Template.Razor.Persistence.Enums;

namespace XUCore.Template.Razor.DbService.Article
{
    /// <summary>
    /// 目录查询命令
    /// </summary>
    public class CategoryQueryCommand : ListCommand
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

        public class Validator : CommandLimitValidator<CategoryQueryCommand, bool>
        {
            public Validator()
            {

            }
        }
    }
}
