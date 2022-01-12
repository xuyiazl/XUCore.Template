using XUCore.Template.Razor.DbService.Auth.Menu;
using XUCore.Template.Razor.Persistence.Enums;

namespace XUCore.Template.Razor.Applaction.User
{
    /// <summary>
    /// 用户导航管理
    /// </summary>
    public class MenuAppService : IMenuAppService
    {
        private readonly IMenuService menuService;

        public MenuAppService(IServiceProvider serviceProvider)
        {
            this.menuService = serviceProvider.GetRequiredService<IMenuService>();
        }

        /// <summary>
        /// 创建导航
        /// </summary>
        /// <param name="request"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task<long> CreateAsync(MenuCreateCommand request, CancellationToken cancellationToken = default)
        {
            var res = await menuService.CreateAsync(request, cancellationToken);

            if (res != null)
                return res.Id;
            else
                return 0L;
        }
        /// <summary>
        /// 更新导航信息
        /// </summary>
        /// <param name="request"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task<int> UpdateAsync(MenuUpdateCommand request, CancellationToken cancellationToken = default)
        {
            return await menuService.UpdateAsync(request, cancellationToken);
        }
        /// <summary>
        /// 更新导航指定字段内容
        /// </summary>
        /// <param name="id"></param>
        /// <param name="field"></param>
        /// <param name="value"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task<int> UpdateAsync(long id, string field, string value, CancellationToken cancellationToken = default)
        {
            return await menuService.UpdateAsync(id, field, value, cancellationToken);
        }
        /// <summary>
        /// 更新状态
        /// </summary>
        /// <param name="ids"></param>
        /// <param name="status"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task<int> UpdateAsync(long[] ids, Status status, CancellationToken cancellationToken = default)
        {
            return await menuService.UpdateAsync(ids, status, cancellationToken);
        }
        /// <summary>
        /// 删除导航（物理删除）
        /// </summary>
        /// <param name="ids"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task<int> DeleteAsync(long[] ids, CancellationToken cancellationToken = default)
        {
            return await menuService.DeleteAsync(ids, cancellationToken);
        }
        /// <summary>
        /// 获取导航信息
        /// </summary>
        /// <param name="id"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task<MenuDto> GetByIdAsync(long id, CancellationToken cancellationToken = default)
        {
            return await menuService.GetByIdAsync(id, cancellationToken);
        }
        /// <summary>
        /// 获取导航树形结构
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task<IList<MenuTreeDto>> GetTreeAsync(CancellationToken cancellationToken = default)
        {
            return await menuService.GetListByTreeAsync(cancellationToken);
        }
        /// <summary>
        /// 获取导航列表
        /// </summary>
        /// <param name="request"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task<IList<MenuDto>> GetListAsync(MenuQueryCommand request, CancellationToken cancellationToken = default)
        {
            return await menuService.GetListAsync(request, cancellationToken);
        }
        /// <summary>
        /// 获取导航列表
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task<IList<MenuDto>> GetListAsync(CancellationToken cancellationToken = default)
        {
            return await menuService.GetListAsync(cancellationToken);
        }
    }
}
