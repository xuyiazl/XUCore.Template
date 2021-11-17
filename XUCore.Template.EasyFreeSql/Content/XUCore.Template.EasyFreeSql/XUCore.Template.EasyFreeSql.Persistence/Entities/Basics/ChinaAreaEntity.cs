namespace XUCore.Template.EasyFreeSql.Persistence.Entities
{
    [Table(Name = "sys_china_area")]
    public class ChinaAreaEntity : EntityFull
    {
        /// <summary>
        /// 上级区域Id
        /// </summary>
        public long ParentId { get; set; }
        /// <summary>
        /// 下级城市
        /// </summary>
        [Navigate(nameof(ParentId))]
        public List<ChinaAreaEntity> Childs { get; set; }
        /// <summary>
        /// 行政区域等级 1-省 2-市 3-区县 4-街道镇
        /// </summary>
        public short Level { get; set; }
        /// <summary>
        /// 名称
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 完整名称
        /// </summary>
        public string WholeName { get; set; }
        /// <summary>
        /// 本区域经度
        /// </summary>
        public string Lon { get; set; }
        /// <summary>
        /// 本区域维度
        /// </summary>
        public string Lat { get; set; }
        /// <summary>
        /// 电话区号
        /// </summary>
        public string CityCode { get; set; }
        /// <summary>
        /// 邮政编码
        /// </summary>
        public string ZipCode { get; set; }
        /// <summary>
        /// 行政区划代码
        /// </summary>
        public string AreaCode { get; set; }
        /// <summary>
        /// 名称全拼
        /// </summary>
        public string PinYin { get; set; }
        /// <summary>
        /// 首字母简拼
        /// </summary>
        public string SimplePy { get; set; }
        /// <summary>
        /// 区域名称拼音的第一个字母
        /// </summary>
        public string PerPinYin { get; set; }
        /// <summary>
        /// 权重排序
        /// </summary>
        public int Sort { get; set; }
    }
}
