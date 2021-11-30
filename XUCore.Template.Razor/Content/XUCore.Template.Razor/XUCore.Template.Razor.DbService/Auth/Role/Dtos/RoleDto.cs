using XUCore.Template.Razor.Core;
using XUCore.Template.Razor.Core.Enums;
using XUCore.Template.Razor.Persistence.Entities.Auth;

namespace XUCore.Template.Razor.DbService.Auth.Role
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
