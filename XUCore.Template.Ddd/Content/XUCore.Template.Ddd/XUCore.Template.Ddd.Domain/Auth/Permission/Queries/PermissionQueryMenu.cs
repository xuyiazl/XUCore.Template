using XUCore.Template.Ddd.Domain.Core.Entities.Auth;

namespace XUCore.Template.Ddd.Domain.Auth.Permission
{
    public class PermissionQueryMenu : Command<IList<PermissionMenuTreeDto>>
    {
        public string UserId { get; set; }

        public class Validator : CommandValidator<PermissionQueryMenu>
        {
            public Validator()
            {
                RuleFor(x => x.UserId).NotEmpty().WithName("UserId");
            }
        }

        public class Handler : CommandHandler<PermissionQueryMenu, IList<PermissionMenuTreeDto>>
        {

            public Handler(IMapper mapper, IMediatorHandler bus) : base(bus, mapper)
            {

            }

            public override async Task<IList<PermissionMenuTreeDto>> Handle(PermissionQueryMenu request, CancellationToken cancellationToken)
            {
                var menus = await bus.SendCommand(new PermissionQueryUserMenus { UserId = request.UserId }, cancellationToken);

                var list = menus
                    .Where(c => c.IsMenu == true)
                    .OrderByDescending(c => c.Weight)
                    .ToList();

                return AuthMenuTree(list, "");
            }

            private IList<PermissionMenuTreeDto> AuthMenuTree(IList<MenuEntity> entities, string parentId)
            {
                IList<PermissionMenuTreeDto> menus = new List<PermissionMenuTreeDto>();

                entities.Where(c => c.FatherId == parentId).ToList().ForEach(entity =>
                {
                    var dto = mapper.Map<MenuEntity, PermissionMenuTreeDto>(entity);

                    dto.Child = AuthMenuTree(entities, dto.Id);

                    menus.Add(dto);
                });

                return menus;
            }
        }
    }
}
