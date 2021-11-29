namespace XUCore.Template.Razor.DbService.User.User
{
    /// <summary>
    /// 登录命令
    /// </summary>
    public class UserLoginCommand : CreateCommand
    {
        /// <summary>
        /// 登录账号
        /// </summary>
        [Required]
        public string Account { get; set; }
        /// <summary>
        /// 密码
        /// </summary>
        [Required]
        public string Password { get; set; }

        public override bool IsVaild()
        {
            ValidationResult = new Validator().Validate(this);

            return ValidationResult.ThrowValidation();
        }

        public class Validator : CommandValidator<UserLoginCommand>
        {
            public Validator()
            {
                RuleFor(x => x.Account).NotEmpty().WithName("登录账号");
                RuleFor(x => x.Password).NotEmpty().WithName("登录密码");
            }
        }
    }
}
