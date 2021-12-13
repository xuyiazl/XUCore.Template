using XUCore.Template.Razor2.Core;
using XUCore.Template.Razor2.Persistence.Enums;
using XUCore.Template.Razor2.Persistence.Entities.User;

namespace XUCore.Template.Razor2.DbService.User.User
{
    public interface IUserService : ICurdService<long, UserEntity, UserDto, UserCreateCommand, UserUpdateInfoCommand, UserQueryCommand, UserQueryPagedCommand>, IScoped
    {
        Task<bool> AnyByAccountAsync(AccountMode accountMode, string account, long notId, CancellationToken cancellationToken);
        Task<UserDto> GetByAccountAsync(AccountMode accountMode, string account, CancellationToken cancellationToken);
        Task<IList<long>> GetRoleKeysAsync(long userId, CancellationToken cancellationToken);
        Task<UserDto> LoginAsync(UserLoginCommand request, CancellationToken cancellationToken);
        Task<int> CreateRelevanceRoleAsync(UserRelevanceRoleCommand request, CancellationToken cancellationToken);
        Task<int> UpdateAsync(UserUpdatePasswordCommand request, CancellationToken cancellationToken);
        Task<int> UpdateAsync(long id, string field, string value, CancellationToken cancellationToken);
        Task<int> UpdateAsync(long[] ids, Status status, CancellationToken cancellationToken);

        Task<IList<UserLoginRecordDto>> GetRecordListAsync(UserLoginRecordQueryCommand request, CancellationToken cancellationToken);

        Task<PagedModel<UserLoginRecordDto>> GetRecordPagedListAsync(UserLoginRecordQueryPagedCommand request, CancellationToken cancellationToken);
    }
}