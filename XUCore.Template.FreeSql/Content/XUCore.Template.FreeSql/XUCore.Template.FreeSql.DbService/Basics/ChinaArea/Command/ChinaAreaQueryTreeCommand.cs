namespace XUCore.Template.FreeSql.DbService.Basics.ChinaArea
{
    /// <summary>
    /// 省市区域查询命令
    /// </summary>
    public class ChinaAreaQueryTreeCommand : Command<bool>
    {
        /// <summary>
        /// 城市Id
        /// </summary>
        [Required]
        public long CityId { get; set; }
        /// <summary>
        /// 行政区域等级 0-全部 1-到市级 2-到县级 3-到街道
        /// </summary>
        [Required]
        public short Level { get; set; }

        public override bool IsVaild()
        {
            ValidationResult = new Validator().Validate(this);

            return ValidationResult.ThrowValidation();
        }

        public class Validator : CommandValidator<ChinaAreaQueryTreeCommand>
        {
            public Validator()
            {

            }
        }
    }
}
