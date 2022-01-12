using XUCore.Template.WeChat.Core;
using XUCore.Template.WeChat.Persistence.Entities.User;


namespace XUCore.Template.WeChat.DbService.User.WeChatUser
{
    /// <summary>
    /// 用户信息修改命令
    /// </summary>
    public class WeChatUserUpdateInfoCommand : UpdateCommand<long>, IMapFrom<UserWeChatEntity>
    {
        /// <summary>
        /// 用户的昵称
        /// </summary>
        [Required]
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

        public override bool IsVaild()
        {
            ValidationResult = new Validator().Validate(this);

            return ValidationResult.IsValid;
        }

        public void Mapping(Profile profile) =>
            profile.CreateMap<WeChatUserUpdateInfoCommand, UserWeChatEntity>()
                .ForMember(c => c.City, c => c.MapFrom(s => s.City.SafeString()))
                .ForMember(c => c.Province, c => c.MapFrom(s => s.Province.SafeString()))
                .ForMember(c => c.Country, c => c.MapFrom(s => s.Country.SafeString()))
                .ForMember(c => c.Headimgurl, c => c.MapFrom(s => s.Headimgurl.SafeString()))
            ;

        public class Validator : CommandIdValidator<WeChatUserUpdateInfoCommand, bool, long>
        {
            public Validator()
            {
                AddIdValidator();
            }
        }

    }
}

