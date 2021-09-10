using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using XUCore.NetCore;

namespace XUCore.Template.EasyFreeSql.Applaction.Basics
{
    /// <summary>
    /// 城市区域管理
    /// </summary>
    public interface IChinaAreaAppService : IAppService
    {
        /// <summary>
        /// 创建城市区域
        /// </summary>
        /// <param name="request"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<Result<long>> CreateAsync(ChinaAreaCreateCommand request, CancellationToken cancellationToken = default);
        /// <summary>
        /// 更新城市区域信息
        /// </summary>
        /// <param name="request"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<Result<int>> UpdateAsync(ChinaAreaUpdateCommand request, CancellationToken cancellationToken = default);
        /// <summary>
        /// 删除城市区域（物理删除）
        /// </summary>
        /// <param name="ids"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<Result<int>> DeleteAsync(long[] ids, CancellationToken cancellationToken = default);
        /// <summary>
        /// 获取城市区域信息
        /// </summary>
        /// <param name="id"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<Result<ChinaAreaDto>> GetAsync(long id, CancellationToken cancellationToken = default);
        /// <summary>
        /// 获取城市区域树形结构
        /// </summary>
        /// <param name="request"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<Result<List<ChinaAreaTreeDto>>> GetTreeAsync(ChinaAreaQueryTreeCommand request, CancellationToken cancellationToken = default);
        /// <summary>
        /// 获取城市区域列表
        /// </summary>
        /// <param name="request"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<Result<List<ChinaAreaDto>>> GetListAsync(ChinaAreaQueryCommand request, CancellationToken cancellationToken = default);
    }
}
