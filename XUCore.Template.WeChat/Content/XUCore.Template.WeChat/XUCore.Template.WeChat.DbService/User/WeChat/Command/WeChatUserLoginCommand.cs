namespace XUCore.Template.WeChat.DbService.User.WeChatUser
{
    /// <summary>
    /// 登录命令
    /// </summary>
    public class WeChatUserLoginCommand : CreateCommand
    {
        /// <summary>
        /// 手机
        /// </summary>
        [Required]
        public string Mobile { get; set; }
        /// <summary>
        /// 密码
        /// </summary>
        [Required]
        public string Password { get; set; }

        public override bool IsVaild()
        {
            ValidationResult = new Validator().Validate(this);

            return ValidationResult.IsValid;
        }

        public class Validator : CommandValidator<WeChatUserLoginCommand>
        {
            public Validator()
            {
                RuleFor(x => x.Mobile).NotEmpty().WithName("手机");
                RuleFor(x => x.Password).NotEmpty().WithName("密码");
            }
        }
    }
}

