using XUCore.Template.Ddd.Domain.Core;
using XUCore.Template.Ddd.Domain.Core.Entities.User;

namespace XUCore.Template.Ddd.Domain.User.User
{
    /// <summary>
    /// 更新部分字段
    /// </summary>
    public class UserUpdateFieldCommand : CommandId<int, string>
    {
        /// <summary>
        /// 字段名
        /// </summary>
        [Required]
        public string Field { get; set; }
        /// <summary>
        /// 需要修改的值
        /// </summary>
        [Required]
        public string Value { get; set; }

        public class Validator : CommandIdValidator<UserUpdateFieldCommand, int, string>
        {
            public Validator()
            {
                AddIdValidator();

                RuleFor(x => x.Field).NotEmpty().WithMessage("字段名");
            }
        }

        public class Handler : CommandHandler<UserUpdateFieldCommand, int>
        {
            private readonly IDefaultDbRepository db;
            private readonly IUserInfo userInfo;

            public Handler(IDefaultDbRepository db, IMediatorHandler bus, IUserInfo userInfo) : base(bus)
            {
                this.db = db;
                this.userInfo = userInfo;
            }


            public override async Task<int> Handle(UserUpdateFieldCommand request, CancellationToken cancellationToken)
            {
                switch (request.Field.ToLower())
                {
                    case "name":
                        return await db.UpdateAsync<UserEntity>(c => c.Id == request.Id, c => new UserEntity() { Name = request.Value, UpdatedAt = DateTime.Now, UpdatedAtUserId = userInfo.Id });
                    case "username":
                        return await db.UpdateAsync<UserEntity>(c => c.Id == request.Id, c => new UserEntity() { UserName = request.Value, UpdatedAt = DateTime.Now, UpdatedAtUserId = userInfo.Id });
                    case "mobile":
                        return await db.UpdateAsync<UserEntity>(c => c.Id == request.Id, c => new UserEntity() { Mobile = request.Value, UpdatedAt = DateTime.Now, UpdatedAtUserId = userInfo.Id });
                    case "password":
                        return await db.UpdateAsync<UserEntity>(c => c.Id == request.Id, c => new UserEntity() { Password = Encrypt.Md5By32(request.Value), UpdatedAt = DateTime.Now, UpdatedAtUserId = userInfo.Id });
                    case "position":
                        return await db.UpdateAsync<UserEntity>(c => c.Id == request.Id, c => new UserEntity() { Position = request.Value, UpdatedAt = DateTime.Now, UpdatedAtUserId = userInfo.Id });
                    case "location":
                        return await db.UpdateAsync<UserEntity>(c => c.Id == request.Id, c => new UserEntity() { Location = request.Value, UpdatedAt = DateTime.Now, UpdatedAtUserId = userInfo.Id });
                    case "company":
                        return await db.UpdateAsync<UserEntity>(c => c.Id == request.Id, c => new UserEntity() { Company = request.Value, UpdatedAt = DateTime.Now, UpdatedAtUserId = userInfo.Id });
                    case "picture":
                        return await db.UpdateAsync<UserEntity>(c => c.Id == request.Id, c => new UserEntity() { Picture = request.Value, UpdatedAt = DateTime.Now, UpdatedAtUserId = userInfo.Id });
                    default:
                        return 0;
                }
            }
        }
    }
}
