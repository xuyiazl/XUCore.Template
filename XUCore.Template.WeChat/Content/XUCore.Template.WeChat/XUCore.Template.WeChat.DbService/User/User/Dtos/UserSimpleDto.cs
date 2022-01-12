using XUCore.Template.WeChat.Core;
using XUCore.Template.WeChat.Persistence.Enums;
using XUCore.Template.WeChat.Persistence.Entities.User;

namespace XUCore.Template.WeChat.DbService.User.User
{
    /// <summary>
    /// 用户信息
    /// </summary>
    public class UserSimpleDto : DtoKeyBase<UserEntity>
    {
        /// <summary>
        /// 名字
        /// </summary>
        public string Name { get; set; }
    }
}

