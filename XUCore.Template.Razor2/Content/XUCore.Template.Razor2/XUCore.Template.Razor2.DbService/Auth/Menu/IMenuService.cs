using XUCore.Template.Razor2.Persistence.Entities.Auth;
using XUCore.Template.Razor2.Persistence.Enums;

namespace XUCore.Template.Razor2.DbService.Auth.Menu
{
    public interface IMenuService : IScoped
    {
        Task<MenuEntity> CreateAsync(MenuCreateCommand request, CancellationToken cancellationToken);

        Task<int> UpdateAsync(MenuUpdateCommand request, CancellationToken cancellationToken);

        Task<int> UpdateAsync(long id, string field, string value, CancellationToken cancellationToken);

        Task<int> UpdateAsync(long[] ids, Status status, CancellationToken cancellationToken);

        Task<int> DeleteAsync(long[] ids, CancellationToken cancellationToken);

        Task<MenuDto> GetByIdAsync(long id, CancellationToken cancellationToken);

        Task<IList<MenuDto>> GetListAsync(CancellationToken cancellationToken);

        Task<IList<MenuDto>> GetListAsync(MenuQueryCommand request, CancellationToken cancellationToken);

        Task<PagedModel<MenuDto>> GetPagedListAsync(MenuQueryPagedCommand request, CancellationToken cancellationToken);

        Task<IList<MenuTreeDto>> GetListByTreeAsync(CancellationToken cancellationToken);
    }
}