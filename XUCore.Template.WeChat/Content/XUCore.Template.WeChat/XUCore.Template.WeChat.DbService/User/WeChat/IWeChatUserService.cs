using XUCore.Template.WeChat.Persistence.Enums;
using XUCore.Template.WeChat.Persistence.Entities.User;

namespace XUCore.Template.WeChat.DbService.User.WeChatUser
{
    public interface IWeChatUserService : IScoped
    {
        Task<WeChatUserEntity> CreateAsync(WeChatUserCreateCommand request, CancellationToken cancellationToken);

        Task<int> UpdateAsync(WeChatUserUpdateInfoCommand request, CancellationToken cancellationToken);

        Task<int> UpdateAsync(WeChatUserUpdatePasswordCommand request, CancellationToken cancellationToken);

        Task<int> UpdateAsync(long id, string field, string value, CancellationToken cancellationToken);

        Task<int> UpdateAsync(long[] ids, Status status, CancellationToken cancellationToken);

        Task<int> DeleteAsync(long[] ids, CancellationToken cancellationToken);

        Task<WeChatUserDto> LoginAsync(WeChatUserLoginCommand request, CancellationToken cancellationToken);

        Task<WeChatUserDto> GetByIdAsync(long id, CancellationToken cancellationToken);

        Task<WeChatUserDto> GetByOpenIdAsync(string opendId, CancellationToken cancellationToken);

        Task<IList<WeChatUserDto>> GetListAsync(WeChatUserQueryCommand request, CancellationToken cancellationToken);

        Task<PagedModel<WeChatUserDto>> GetPagedListAsync(WeChatUserQueryPagedCommand request, CancellationToken cancellationToken);
    }
}
