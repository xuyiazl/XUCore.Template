using XUCore.Template.Ddd.Domain.Core;
using XUCore.Template.Ddd.Domain.Core.Entities.User;

namespace XUCore.Template.Ddd.Domain.User.User
{
    /// <summary>
    /// 更新密码
    /// </summary>
    public class UserUpdatePasswordCommand : CommandId<int, string>
    {
        [Required]
        public string OldPassword { get; set; }
        [Required]
        public string NewPassword { get; set; }

        public class Validator : CommandIdValidator<UserUpdatePasswordCommand, int, string>
        {
            public Validator()
            {
                AddIdValidator();

                RuleFor(x => x.OldPassword).NotEmpty().MaximumLength(30).WithName("旧密码");
                RuleFor(x => x.NewPassword).NotEmpty().MaximumLength(30).WithName("新密码").NotEqual(c => c.OldPassword).WithName("新密码不能和旧密码相同");
            }
        }

        public class Handler : CommandHandler<UserUpdatePasswordCommand, int>
        {
            private readonly IDefaultDbRepository db;
            private readonly IUserInfo userInfo;

            public Handler(IDefaultDbRepository db, IMediatorHandler bus, IUserInfo userInfo) : base(bus)
            {
                this.db = db;
                this.userInfo = userInfo;
            }

            public override async Task<int> Handle(UserUpdatePasswordCommand request, CancellationToken cancellationToken)
            {
                var user = await db.GetByIdAsync<UserEntity>(request.Id, cancellationToken);

                request.NewPassword = Encrypt.Md5By32(request.NewPassword);
                request.OldPassword = Encrypt.Md5By32(request.OldPassword);

                if (!user.Password.Equals(request.OldPassword))
                    throw new Exception("旧密码错误");

                return await db.UpdateAsync<UserEntity>(c => c.Id == request.Id, c => new UserEntity
                {
                    Password = request.NewPassword,
                    UpdatedAt = DateTime.Now,
                    UpdatedAtUserId = userInfo.Id
                });
            }
        }
    }
}
