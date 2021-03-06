using XUCore.Template.EasyFreeSql.Core;
using XUCore.Template.EasyFreeSql.Persistence.Entities.User;


namespace XUCore.Template.EasyFreeSql.Applaction.User.User
{
    /// <summary>
    /// 用户信息修改命令
    /// </summary>
    public class UserUpdateInfoCommand : UpdateCommand<long>, IMapFrom<UserEntity>
    {
        /// <summary>
        /// 名字
        /// </summary>
        [Required]
        public string Name { get; set; }
        /// <summary>
        /// 位置
        /// </summary>
        [Required]
        public string Location { get; set; }
        /// <summary>
        /// 职位
        /// </summary>
        [Required]
        public string Position { get; set; }
        /// <summary>
        /// 公司
        /// </summary>
        [Required]
        public string Company { get; set; }

        /// <summary>
        /// 验证
        /// </summary>
        public override bool IsVaild()
        {
            ValidationResult = new Validator().Validate(this);

            return ValidationResult.ThrowValidation();
        }
        /// <summary>
        /// 映射
        /// </summary>
        /// <param name="profile"></param>
        public void Mapping(Profile profile) =>
            profile.CreateMap<UserUpdateInfoCommand, UserEntity>()
                .ForMember(c => c.Location, c => c.MapFrom(s => s.Location.SafeString()))
                .ForMember(c => c.Position, c => c.MapFrom(s => s.Position.SafeString()))
                .ForMember(c => c.Company, c => c.MapFrom(s => s.Company.SafeString()))
            ;

        /// <summary>
        /// 验证
        /// </summary>
        public class Validator : CommandIdValidator<UserUpdateInfoCommand, bool, long>
        {
            /// <summary>
            /// 验证
            /// </summary>
            public Validator()
            {
                AddIdValidator();

                RuleFor(x => x.Name).NotEmpty().MaximumLength(30).WithName("名字");
                RuleFor(x => x.Company).NotEmpty().MaximumLength(30).WithName("公司");
                RuleFor(x => x.Location).NotEmpty().MaximumLength(30).WithName("位置");
                RuleFor(x => x.Position).NotEmpty().MaximumLength(20).WithName("职位");
            }
        }

    }
}
