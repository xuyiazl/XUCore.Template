using XUCore.Template.Razor2.Core;
using XUCore.Template.Razor2.Persistence.Entities.Auth;
using XUCore.Template.Razor2.Persistence.Enums;

namespace XUCore.Template.Razor2.DbService.Auth.Role
{
    /// <summary>
    /// 角色
    /// </summary>
    public class RoleDto : DtoBase<RoleEntity>
    {
        /// <summary>
        /// 角色名
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 数据状态
        /// </summary>
        public Status Status { get; set; }
    }
}
