namespace XUCore.Template.EasyFreeSql.Applaction.Basics
{
    /// <summary>
    /// 省市区域查询命令
    /// </summary>
    public class ChinaAreaQueryCommand : ListCommand
    {
        /// <summary>
        /// 城市Id
        /// </summary>
        [Required]
        public long CityId { get; set; }

        public override bool IsVaild()
        {
            ValidationResult = new Validator().Validate(this);

            return ValidationResult.ThrowValidation();
        }

        public class Validator : CommandLimitValidator<ChinaAreaQueryCommand, bool>
        {
            public Validator()
            {

            }
        }
    }
}
