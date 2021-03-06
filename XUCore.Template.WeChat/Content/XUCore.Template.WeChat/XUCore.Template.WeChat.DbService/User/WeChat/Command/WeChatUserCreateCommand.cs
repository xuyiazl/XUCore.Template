using XUCore.Template.WeChat.Core;
using XUCore.Template.WeChat.Persistence.Enums;
using XUCore.Template.WeChat.Persistence.Entities.User;

namespace XUCore.Template.WeChat.DbService.User.WeChatUser
{
    /// <summary>
    /// 创建用户命令
    /// </summary>
    public class WeChatUserCreateCommand : CreateCommand, IMapFrom<WeChatUserEntity>
    {
        /// <summary>
        /// 关联的管理员Id
        /// </summary>
        public long UserId { get; set; }
        /// <summary>
        /// 只有在用户将公众号绑定到微信开放平台帐号后，才会出现该字段。详见：获取用户个人信息（UnionID机制）
        /// </summary>
        public string Unionid { get; set; }

        /// <summary>
        /// 用户的标识，对当前公众号唯一
        /// </summary>
        [Required]
        public string OpenId { get; set; }

        /// <summary>
        /// 手机号
        /// </summary>
        public string Mobile { get; set; }

        /// <summary>
        /// 密码
        /// </summary>
        public string Password { get; set; }

        /// <summary>
        /// 用户的昵称
        /// </summary>
        public string NickName { get; set; }

        /// <summary>
        ///  用户的性别，值为1时是男性，值为2时是女性，值为0时是未知
        /// </summary>
        public int Sex { get; set; }

        /// <summary>
        /// 用户所在城市
        /// </summary>
        public string City { get; set; }

        /// <summary>
        /// 用户所在省份
        /// </summary>
        public string Province { get; set; }

        /// <summary>
        /// 用户所在国家
        /// </summary>
        public string Country { get; set; }

        /// <summary>
        /// 用户头像，最后一个数值代表正方形头像大小（有0、46、64、96、132数值可选，0代表640*640正方形头像），用户没有头像时该项为空。若用户更换头像，原有头像URL将失效。
        /// </summary>
        public string Headimgurl { get; set; }

        /// <summary>
        /// 数据状态
        /// </summary>
        [Required]
        public Status Status { get; set; }

        public override bool IsVaild()
        {
            ValidationResult = new Validator().Validate(this);

            return ValidationResult.IsValid;
        }

        public void Mapping(Profile profile) =>
            profile.CreateMap<WeChatUserCreateCommand, WeChatUserEntity>()
                .ForMember(c => c.City, c => c.MapFrom(s => s.City.SafeString()))
                .ForMember(c => c.Province, c => c.MapFrom(s => s.Province.SafeString()))
                .ForMember(c => c.Country, c => c.MapFrom(s => s.Country.SafeString()))
                .ForMember(c => c.Headimgurl, c => c.MapFrom(s => s.Headimgurl.SafeString()))
                .ForMember(c => c.Mobile, c => c.MapFrom(s => ""))
                .ForMember(c => c.Password, c => c.MapFrom(s => ""))
                .ForMember(c => c.Status, c => c.MapFrom(s => Status.Show))
            ;

        public class Validator : CommandValidator<WeChatUserCreateCommand>
        {
            public Validator()
            {
            }
        }
    }
}

