using XUCore.Template.Razor.Core.Enums;
using XUCore.Template.Razor.Persistence.Entities.Auth;

namespace XUCore.Template.Razor.DbService.Auth.Role
{
    public interface IRoleService : ICurdService<long, RoleEntity, RoleDto, RoleCreateCommand, RoleUpdateCommand, RoleQueryCommand, RoleQueryPagedCommand>, IScoped
    {
        Task<int> UpdateAsync(long id, string field, string value, CancellationToken cancellationToken);
        Task<int> UpdateAsync(long[] ids, Status status, CancellationToken cancellationToken);
        Task<IList<long>> GetRelevanceMenuAsync(int roleId, CancellationToken cancellationToken);
    }
}