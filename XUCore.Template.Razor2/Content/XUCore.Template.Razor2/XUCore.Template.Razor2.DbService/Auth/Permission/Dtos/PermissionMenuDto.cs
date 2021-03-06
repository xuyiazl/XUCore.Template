using XUCore.Template.Razor2.Core;
using XUCore.Template.Razor2.Persistence.Entities.Auth;

namespace XUCore.Template.Razor2.DbService.Auth.Permission
{
    /// <summary>
    /// 权限导航
    /// </summary>
    public class PermissionMenuDto : DtoKeyBase<MenuEntity>
    {
        /// <summary>
        /// 导航父级id
        /// </summary>
        public long ParentId { get; set; }
        /// <summary>
        /// 名字
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 图标样式
        /// </summary>
        public string Icon { get; set; }
        /// <summary>
        /// 连接地址
        /// </summary>
        public string Url { get; set; }
        /// <summary>
        /// 唯一代码
        /// </summary>
        public string OnlyCode { get; set; }
    }
}
