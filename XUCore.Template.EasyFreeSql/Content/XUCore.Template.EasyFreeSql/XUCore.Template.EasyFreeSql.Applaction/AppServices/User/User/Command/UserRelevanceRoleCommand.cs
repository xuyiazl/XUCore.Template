namespace XUCore.Template.EasyFreeSql.Applaction.User.User
{
    /// <summary>
    /// 关联角色命令
    /// </summary>
    public class UserRelevanceRoleCommand : CreateCommand
    {
        /// <summary>
        /// 用户id
        /// </summary>
        [Required]
        public long UserId { get; set; }
        /// <summary>
        /// 角色id集合
        /// </summary>
        public long[] RoleIds { get; set; }

        /// <summary>
        /// 验证
        /// </summary>
        public override bool IsVaild()
        {
            ValidationResult = new Validator().Validate(this);

            return ValidationResult.ThrowValidation();
        }

        /// <summary>
        /// 验证
        /// </summary>
        public class Validator : CommandValidator<UserRelevanceRoleCommand>
        {
            /// <summary>
            /// 验证
            /// </summary>
            public Validator()
            {
                RuleFor(x => x.UserId).NotEmpty().GreaterThan(0).WithName("UserId");
            }
        }
    }
}
