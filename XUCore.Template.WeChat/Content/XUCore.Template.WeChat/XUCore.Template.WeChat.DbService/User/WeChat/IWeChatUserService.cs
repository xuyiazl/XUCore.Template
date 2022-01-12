using XUCore.Template.WeChat.Persistence.Enums;
using XUCore.Template.WeChat.Persistence.Entities.User;

namespace XUCore.Template.WeChat.DbService.User.WeChatUser
{
    public interface IWeChatUserService : ICurdService<long, UserWeChatEntity, WeChatUserDto, WeChatUserCreateCommand, WeChatUserUpdateInfoCommand, WeChatUserQueryCommand, WeChatUserQueryPagedCommand>, IScoped
    {
        Task<WeChatUserDto> LoginAsync(WeChatUserLoginCommand request, CancellationToken cancellationToken);
        Task<int> UpdateAsync(WeChatUserUpdatePasswordCommand request, CancellationToken cancellationToken);
        Task<int> UpdateAsync(long id, string field, string value, CancellationToken cancellationToken);
        Task<int> UpdateAsync(long[] ids, Status status, CancellationToken cancellationToken);
        Task<WeChatUserDto> GetByOpenIdAsync(string opendId, CancellationToken cancellationToken);
    }
}
