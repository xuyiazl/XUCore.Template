using XUCore.Template.Ddd.Domain.Core;

namespace XUCore.Template.Ddd.Domain.Auth.Role
{
    /// <summary>
    /// 查询角色关联的导航id集合
    /// </summary>
    public class RoleQueryMenuKeys : Command<IList<string>>
    {
        /// <summary>
        /// 角色id
        /// </summary>
        [Required]
        public string RoleId { get; set; }

        public class Validator : CommandValidator<RoleQueryMenuKeys>
        {
            public Validator()
            {
            }
        }


        public class Handler : CommandHandler<RoleQueryMenuKeys, IList<string>>
        {
            private readonly IDefaultDbRepository db;
            private readonly IMapper mapper;

            public Handler(IDefaultDbRepository db, IMapper mapper)
            {
                this.db = db;
                this.mapper = mapper;
            }

            public override async Task<IList<string>> Handle(RoleQueryMenuKeys request, CancellationToken cancellationToken)
            {
                return await db.Context.RoleMenu
                    .Where(c => c.RoleId == request.RoleId)
                    .OrderBy(c => c.MenuId)
                    .Select(c => c.MenuId)
                    .ToListAsync();
            }
        }
    }
}
