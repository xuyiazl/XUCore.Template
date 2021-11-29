using XUCore.Template.Razor.Persistence.Entities.User;

namespace XUCore.Template.Razor.DbService.Auth.Role
{
    public interface IRoleService : ICurdService<long, RoleEntity,  RoleDto, RoleCreateCommand, RoleUpdateCommand, RoleQueryCommand, RoleQueryPagedCommand>, IScoped
    {
        Task<int> UpdateAsync(long id, string field, string value, CancellationToken cancellationToken);
        Task<int> UpdateAsync(long[] ids, bool enabled, CancellationToken cancellationToken);
        Task<IList<long>> GetRelevanceMenuAsync(int roleId, CancellationToken cancellationToken);
    }
}