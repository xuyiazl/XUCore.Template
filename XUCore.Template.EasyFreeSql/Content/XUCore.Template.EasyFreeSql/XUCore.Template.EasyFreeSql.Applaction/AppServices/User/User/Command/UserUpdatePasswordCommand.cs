namespace XUCore.Template.EasyFreeSql.Applaction.User.User
{
    /// <summary>
    /// 密码修改命令
    /// </summary>
    public class UserUpdatePasswordCommand : UpdateCommand<long>
    {
        /// <summary>
        /// 旧密码
        /// </summary>
        [Required]
        public string OldPassword { get; set; }
        /// <summary>
        /// 新密码
        /// </summary>
        [Required]
        public string NewPassword { get; set; }
        /// <summary>
        /// 验证
        /// </summary>
        /// <returns></returns>
        public override bool IsVaild()
        {
            ValidationResult = new Validator().Validate(this);

            return ValidationResult.ThrowValidation();
        }
        /// <summary>
        /// 验证
        /// </summary>
        public class Validator : CommandIdValidator<UserUpdatePasswordCommand, bool, long>
        {
            /// <summary>
            /// 验证
            /// </summary>
            public Validator()
            {
                AddIdValidator();

                RuleFor(x => x.OldPassword).NotEmpty().MaximumLength(30).WithName("旧密码");
                RuleFor(x => x.NewPassword).NotEmpty().MaximumLength(30).WithName("新密码").NotEqual(c => c.OldPassword).WithName("新密码不能和旧密码相同");
            }
        }
    }
}
