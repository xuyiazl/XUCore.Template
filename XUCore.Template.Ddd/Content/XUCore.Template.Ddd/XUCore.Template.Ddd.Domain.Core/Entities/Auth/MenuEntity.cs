namespace XUCore.Template.Ddd.Domain.Core.Entities.Auth
{
    /// <summary>
    /// 权限导航表
    /// </summary>
    public partial class MenuEntity : BaseEntity
    {
        public MenuEntity()
        {
            RoleMenus = new List<RoleMenuEntity>();
        }
        /// <summary>
        /// 导航父级id
        /// </summary>
        public string FatherId { get; set; }
        /// <summary>
        /// 名字
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 图标样式
        /// </summary>
        public string Icon { get; set; }
        /// <summary>
        /// 链接地址
        /// </summary>
        public string Url { get; set; }
        /// <summary>
        /// 唯一代码（权限使用）
        /// </summary>
        public string OnlyCode { get; set; }
        /// <summary>
        /// 是否是导航
        /// </summary>
        public bool IsMenu { get; set; }
        /// <summary>
        /// 排序权重
        /// </summary>
        public int Weight { get; set; }
        /// <summary>
        /// 是否是快捷导航
        /// </summary>
        public bool IsExpress { get; set; }
        /// <summary>
        /// 角色导航关联列表
        /// </summary>
        public ICollection<RoleMenuEntity> RoleMenus;
    }
}
