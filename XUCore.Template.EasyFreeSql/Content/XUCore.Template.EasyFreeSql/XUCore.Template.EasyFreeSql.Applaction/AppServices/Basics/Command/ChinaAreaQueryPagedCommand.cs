namespace XUCore.Template.EasyFreeSql.Applaction.Basics
{
    /// <summary>
    /// 省市区域查询命令
    /// </summary>
    public class ChinaAreaQueryPagedCommand : PageCommand
    {
        /// <summary>
        /// 搜索关键字
        /// </summary>
        public string Keyword { get; set; }

        public override bool IsVaild()
        {
            ValidationResult = new Validator().Validate(this);

            return ValidationResult.ThrowValidation();
        }

        public class Validator : CommandPageValidator<ChinaAreaQueryPagedCommand, bool>
        {
            public Validator()
            {
                AddPageVaildator();
            }
        }
    }
}
