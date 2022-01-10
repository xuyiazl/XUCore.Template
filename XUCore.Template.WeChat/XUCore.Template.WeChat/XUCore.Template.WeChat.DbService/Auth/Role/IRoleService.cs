using XUCore.Template.WeChat.Persistence.Entities.Auth;
using XUCore.Template.WeChat.Persistence.Enums;

namespace XUCore.Template.WeChat.DbService.Auth.Role
{
    public interface IRoleService : ICurdService<long, RoleEntity, RoleDto, RoleCreateCommand, RoleUpdateCommand, RoleQueryCommand, RoleQueryPagedCommand>, IScoped
    {
        Task<int> UpdateAsync(long id, string field, string value, CancellationToken cancellationToken);
        Task<int> UpdateAsync(long[] ids, Status status, CancellationToken cancellationToken);
        /// <summary>
        /// 检查名字是否重复
        /// </summary>
        /// <param name="name"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<bool> AnyAsync(string name, CancellationToken cancellationToken);
        Task<IList<long>> GetRelevanceMenuAsync(int roleId, CancellationToken cancellationToken);
    }
}
