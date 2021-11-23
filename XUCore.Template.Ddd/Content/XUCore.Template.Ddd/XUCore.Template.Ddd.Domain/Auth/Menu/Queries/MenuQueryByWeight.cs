using XUCore.Template.Ddd.Domain.Core;
using XUCore.Template.Ddd.Domain.Core.Entities.Auth;

namespace XUCore.Template.Ddd.Domain.Auth.Menu
{
    /// <summary>
    /// 查询导航列表命令
    /// </summary>
    public class MenuQueryByWeight : Command<IList<MenuDto>>
    {
        /// <summary>
        /// 是否是导航
        /// </summary>
        [Required]
        public bool IsMenu { get; set; }

        public class Validator : CommandValidator<MenuQueryByWeight>
        {
            public Validator()
            {

            }
        }

        public class Handler : CommandHandler<MenuQueryByWeight, IList<MenuDto>>
        {
            private readonly IDefaultDbRepository db;
            private readonly IMapper mapper;

            public Handler(IDefaultDbRepository db, IMapper mapper)
            {
                this.db = db;
                this.mapper = mapper;
            }

            public override async Task<IList<MenuDto>> Handle(MenuQueryByWeight request, CancellationToken cancellationToken)
            {
                var selector = db.BuildFilter<MenuEntity>()
                    .And(c => c.IsMenu == request.IsMenu);

                var res = await db.GetListAsync<MenuEntity, MenuDto>(selector, orderby: "Weight desc", cancellationToken: cancellationToken);

                return res;
            }
        }
    }
}
