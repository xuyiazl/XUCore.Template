using XUCore.Template.FreeSql.Persistence.Entities.User;

namespace XUCore.Template.FreeSql.DbService.Auth.Menu
{
    public interface IMenuService : ICurdService<long, MenuEntity, MenuDto, MenuCreateCommand, MenuUpdateCommand, MenuQueryCommand, MenuQueryPagedCommand>, IScoped
    {
        Task<IList<MenuTreeDto>> GetListByTreeAsync(CancellationToken cancellationToken);
        Task<int> UpdateAsync(long id, string field, string value, CancellationToken cancellationToken);
        Task<int> UpdateAsync(long[] ids, bool enabled, CancellationToken cancellationToken);
    }
}