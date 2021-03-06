
using XUCore.Template.Ddd.Domain.Core.Entities.Auth;

namespace XUCore.Template.Ddd.Domain.Core.Entities.User
{
    /// <summary>
    /// 用户表
    /// </summary>
    public partial class UserEntity : BaseEntity
    {
        public UserEntity()
        {
            UserRoles = new List<UserRoleEntity>();
            LoginRecords = new List<UserLoginRecordEntity>();
        }
        /// <summary>
        /// 用户名
        /// </summary>
        public string UserName { get; set; }
        /// <summary>
        /// 手机号
        /// </summary>
        public string Mobile { get; set; }
        /// <summary>
        /// 密码
        /// </summary>
        public string Password { get; set; }
        /// <summary>
        /// 名字
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 头像
        /// </summary>
        public string Picture { get; set; }
        /// <summary>
        /// 所在位置
        /// </summary>
        public string Location { get; set; }
        /// <summary>
        /// 职位
        /// </summary>
        public string Position { get; set; }
        /// <summary>
        /// 公司
        /// </summary>
        public string Company { get; set; }
        /// <summary>
        /// 登录次数
        /// </summary>
        public int LoginCount { get; set; }
        /// <summary>
        /// 最后登录时间
        /// </summary>
        public DateTime LoginLastTime { get; set; }
        /// <summary>
        /// 最后登录id
        /// </summary>
        public string LoginLastIp { get; set; }
        /// <summary>
        /// 对应用户角色关联
        /// </summary>
        public ICollection<UserRoleEntity> UserRoles;
        /// <summary>
        /// 对应登录记录关联
        /// </summary>
        public ICollection<UserLoginRecordEntity> LoginRecords;
    }
}
