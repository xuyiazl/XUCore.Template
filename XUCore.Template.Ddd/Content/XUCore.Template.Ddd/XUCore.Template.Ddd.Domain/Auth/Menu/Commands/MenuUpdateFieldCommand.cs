using XUCore.Template.Ddd.Domain.Core;
using XUCore.Template.Ddd.Domain.Core.Entities.Auth;

namespace XUCore.Template.Ddd.Domain.Auth.Menu
{
    /// <summary>
    /// 更新部分字段
    /// </summary>
    public class MenuUpdateFieldCommand : CommandId<int, string>
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

        public class Validator : CommandIdValidator<MenuUpdateFieldCommand, int, string>
        {
            public Validator()
            {
                AddIdValidator();

                RuleFor(x => x.Field).NotEmpty().WithMessage("字段名");
            }
        }

        public class Handler : CommandHandler<MenuUpdateFieldCommand, int>
        {
            private readonly IDefaultDbRepository db;
            private readonly IUserInfo userInfo;

            public Handler(IDefaultDbRepository db, IMediatorHandler bus, IUserInfo userInfo) : base(bus)
            {
                this.db = db;
                this.userInfo = userInfo;
            }

            public override async Task<int> Handle(MenuUpdateFieldCommand request, CancellationToken cancellationToken)
            {
                switch (request.Field.ToLower())
                {
                    case "icon":
                        return await db.UpdateAsync<MenuEntity>(c => c.Id == request.Id, c => new MenuEntity() { Icon = request.Value, UpdatedAt = DateTime.Now, UpdatedAtUserId = userInfo.Id });
                    case "url":
                        return await db.UpdateAsync<MenuEntity>(c => c.Id == request.Id, c => new MenuEntity() { Url = request.Value, UpdatedAt = DateTime.Now, UpdatedAtUserId = userInfo.Id });
                    case "onlycode":
                        return await db.UpdateAsync<MenuEntity>(c => c.Id == request.Id, c => new MenuEntity() { OnlyCode = request.Value, UpdatedAt = DateTime.Now, UpdatedAtUserId = userInfo.Id });
                    case "weight":
                        return await db.UpdateAsync<MenuEntity>(c => c.Id == request.Id, c => new MenuEntity() { Weight = request.Value.ToInt(), UpdatedAt = DateTime.Now, UpdatedAtUserId = userInfo.Id });
                    default:
                        return 0;
                }
            }
        }
    }
}
