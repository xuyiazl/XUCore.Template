using XUCore.Template.WeChat.Persistence.Entities.Auth;
using XUCore.Template.WeChat.Persistence.Enums;

namespace XUCore.Template.WeChat.DbService.Auth.Role
{
    public interface IRoleService : IScoped
    {
        Task<RoleEntity> CreateAsync(RoleCreateCommand request, CancellationToken cancellationToken);

        Task<int> UpdateAsync(RoleUpdateCommand request, CancellationToken cancellationToken);

        Task<int> UpdateAsync(long id, string field, string value, CancellationToken cancellationToken);

        Task<int> UpdateAsync(long[] ids, Status status, CancellationToken cancellationToken);

        Task<int> DeleteAsync(long[] ids, CancellationToken cancellationToken);

        Task<bool> AnyAsync(string name, CancellationToken cancellationToken);

        Task<RoleDto> GetByIdAsync(long id, CancellationToken cancellationToken);

        Task<IList<RoleDto>> GetListAsync(RoleQueryCommand request, CancellationToken cancellationToken);

        Task<IList<long>> GetRelevanceMenuAsync(int roleId, CancellationToken cancellationToken);

        Task<PagedModel<RoleDto>> GetPagedListAsync(RoleQueryPagedCommand request, CancellationToken cancellationToken);
    }
}