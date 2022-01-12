using XUCore.Template.WeChat.DbService.User.WeChatUser;
using XUCore.Template.WeChat.Persistence.Enums;

namespace XUCore.Template.WeChat.Applaction.WeChat
{
    /// <summary>
    /// 用户管理
    /// </summary>
    public class WeChatUserAppService : IWeChatUserAppService
    {
        private readonly IWeChatUserService userService;

        public WeChatUserAppService(IServiceProvider serviceProvider)
        {
            this.userService = serviceProvider.GetRequiredService<IWeChatUserService>();
        }

        /// <summary>
        /// 创建用户账号
        /// </summary>
        /// <param name="request"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task<long> CreateAsync(WeChatUserCreateCommand request, CancellationToken cancellationToken = default)
        {
            var res = await userService.CreateAsync(request, cancellationToken);

            if (res != null)
                return res.Id;
            else
                return 0L;
        }
        /// <summary>
        /// 更新账号信息
        /// </summary>
        /// <param name="request"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task<int> UpdateAsync(WeChatUserUpdateInfoCommand request, CancellationToken cancellationToken = default)
        {
            return await userService.UpdateAsync(request, cancellationToken);
        }
        /// <summary>
        /// 更新密码
        /// </summary>
        /// <param name="request"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task<int> UpdatePasswordAsync(WeChatUserUpdatePasswordCommand request, CancellationToken cancellationToken = default)
        {
            return await userService.UpdateAsync(request, cancellationToken);
        }
        /// <summary>
        /// 更新指定字段内容
        /// </summary>
        /// <param name="id"></param>
        /// <param name="field"></param>
        /// <param name="value"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task<int> UpdateFieldAsync(long id, string field, string value, CancellationToken cancellationToken = default)
        {
            return await userService.UpdateAsync(id, field, value, cancellationToken);
        }
        /// <summary>
        /// 更新状态
        /// </summary>
        /// <param name="ids"></param>
        /// <param name="status"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task<int> UpdateStatusAsync(long[] ids, Status status, CancellationToken cancellationToken = default)
        {
            return await userService.UpdateAsync(ids, status, cancellationToken);
        }
        /// <summary>
        /// 删除账号（物理删除）
        /// </summary>
        /// <param name="ids"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task<int> DeleteAsync(long[] ids, CancellationToken cancellationToken = default)
        {
            return await userService.DeleteAsync(ids, cancellationToken);
        }
        /// <summary>
        /// 获取账号信息
        /// </summary>
        /// <param name="id"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task<WeChatUserDto> GetByIdAsync(long id, CancellationToken cancellationToken = default)
        {
            return await userService.GetByIdAsync(id, cancellationToken);
        }
        /// <summary>
        /// 获取账号信息（根据OpenId）
        /// </summary>
        /// <param name="opendId"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task<WeChatUserDto> GetByOpenIdAsync(string opendId, CancellationToken cancellationToken = default)
        {
            return await userService.GetByOpenIdAsync(opendId, cancellationToken);
        }
        /// <summary>
        /// 获取账号分页
        /// </summary>
        /// <param name="request"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task<PagedModel<WeChatUserDto>> GetPageAsync(WeChatUserQueryPagedCommand request, CancellationToken cancellationToken = default)
        {
            return await userService.GetPagedListAsync(request, cancellationToken);
        }
    }
}