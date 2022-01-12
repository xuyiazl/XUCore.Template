using XUCore.Template.WeChat.DbService.User.WeChatUser;
using XUCore.Template.WeChat.Persistence.Enums;

namespace XUCore.Template.WeChat.Applaction.WeChat
{
    /// <summary>
    /// 用户管理
    /// </summary>
    public interface IWeChatUserAppService : IScoped
    {
        /// <summary>
        /// 创建用户账号
        /// </summary>
        /// <param name="request"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<long> CreateAsync(WeChatUserCreateCommand request, CancellationToken cancellationToken = default);
        /// <summary>
        /// 更新账号信息
        /// </summary>
        /// <param name="request"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<int> UpdateAsync(WeChatUserUpdateInfoCommand request, CancellationToken cancellationToken = default);
        /// <summary>
        /// 更新密码
        /// </summary>
        /// <param name="request"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<int> UpdatePasswordAsync(WeChatUserUpdatePasswordCommand request, CancellationToken cancellationToken = default);
        /// <summary>
        /// 更新指定字段内容
        /// </summary>
        /// <param name="id"></param>
        /// <param name="field"></param>
        /// <param name="value"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<int> UpdateAsync(long id, string field, string value, CancellationToken cancellationToken = default);
        /// <summary>
        /// 更新状态
        /// </summary>
        /// <param name="ids"></param>
        /// <param name="status"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<int> UpdateAsync(long[] ids, Status status, CancellationToken cancellationToken = default);
        /// <summary>
        /// 删除账号（物理删除）
        /// </summary>
        /// <param name="ids"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<int> DeleteAsync(long[] ids, CancellationToken cancellationToken = default);
        /// <summary>
        /// 获取账号信息
        /// </summary>
        /// <param name="id"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<WeChatUserDto> GetByIdAsync(long id, CancellationToken cancellationToken = default);
        /// <summary>
        /// 获取账号信息（根据OpenId）
        /// </summary>
        /// <param name="opendId"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<WeChatUserDto> GetByOpenIdAsync(string opendId, CancellationToken cancellationToken = default);
        /// <summary>
        /// 获取账号分页
        /// </summary>
        /// <param name="request"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<PagedModel<WeChatUserDto>> GetPageAsync(WeChatUserQueryPagedCommand request, CancellationToken cancellationToken = default);
    }
}

