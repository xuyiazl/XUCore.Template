using XUCore.Template.Razor.Persistence.Entities.Auth;
using XUCore.Template.Razor.Persistence.Enums;

namespace XUCore.Template.Razor.DbService.Auth.Menu
{
    public interface IMenuService : ICurdService<long, MenuEntity, MenuDto, MenuCreateCommand, MenuUpdateCommand, MenuQueryCommand, MenuQueryPagedCommand>, IScoped
    {
        Task<IList<MenuDto>> GetListAsync(CancellationToken cancellationToken);
        Task<IList<MenuTreeDto>> GetListByTreeAsync(CancellationToken cancellationToken);
        Task<int> UpdateAsync(long id, string field, string value, CancellationToken cancellationToken);
        Task<int> UpdateAsync(long[] ids, Status status, CancellationToken cancellationToken);
    }
}