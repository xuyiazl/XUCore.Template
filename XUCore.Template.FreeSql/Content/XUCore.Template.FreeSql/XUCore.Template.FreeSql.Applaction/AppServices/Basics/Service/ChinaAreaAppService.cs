using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Threading;
using System.Threading.Tasks;
using XUCore.NetCore;
using XUCore.NetCore.DynamicWebApi;
using XUCore.Template.FreeSql.Core;
using XUCore.Template.FreeSql.DbService.Basics.ChinaArea;

namespace XUCore.Template.FreeSql.Applaction.Basics
{
    /// <summary>
    /// 城市区域管理
    /// </summary>
    [ApiExplorerSettings(GroupName = ApiGroup.Admin)]
    [DynamicWebApi]
    public class ChinaAreaAppService : IChinaAreaAppService, IDynamicWebApi
    {
        private readonly IChinaAreaService chinaAreaService;

        public ChinaAreaAppService(IServiceProvider serviceProvider)
        {
            this.chinaAreaService = serviceProvider.GetService<IChinaAreaService>();
        }

        /// <summary>
        /// 创建城市区域
        /// </summary>
        /// <param name="request"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<Result<long>> CreateAsync([Required][FromBody] ChinaAreaCreateCommand request, CancellationToken cancellationToken = default)
        {
            var res = await chinaAreaService.CreateAsync(request, cancellationToken);

            if (res != null)
                return RestFull.Success(data: res.Id);
            else
                return RestFull.Fail(data: 0L);
        }
        /// <summary>
        /// 更新城市区域信息
        /// </summary>
        /// <param name="request"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [HttpPut]
        public async Task<Result<int>> UpdateAsync([Required][FromBody] ChinaAreaUpdateCommand request, CancellationToken cancellationToken = default)
        {
            var res = await chinaAreaService.UpdateAsync(request, cancellationToken);

            if (res > 0)
                return RestFull.Success(data: res);
            else
                return RestFull.Fail(data: res);
        }
        /// <summary>
        /// 删除城市区域（物理删除）
        /// </summary>
        /// <param name="ids"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [HttpDelete]
        public async Task<Result<int>> DeleteAsync([Required][FromQuery] long[] ids, CancellationToken cancellationToken = default)
        {
            var res = await chinaAreaService.DeleteAsync(ids, cancellationToken);

            if (res > 0)
                return RestFull.Success(data: res);
            else
                return RestFull.Fail(data: res);
        }
        /// <summary>
        /// 获取城市区域信息
        /// </summary>
        /// <param name="id"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [HttpGet("{id}")]
        public async Task<Result<ChinaAreaDto>> GetAsync([Required] long id, CancellationToken cancellationToken = default)
        {
            var res = await chinaAreaService.GetByIdAsync(id, cancellationToken);

            return RestFull.Success(data: res);
        }
        /// <summary>
        /// 获取城市区域树形结构
        /// </summary>
        /// <param name="request"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task<Result<IList<ChinaAreaTreeDto>>> GetTreeAsync(ChinaAreaQueryTreeCommand request, CancellationToken cancellationToken = default)
        {
            var res = await chinaAreaService.GetListByTreeAsync(request, cancellationToken);

            return RestFull.Success(data: res);
        }
        /// <summary>
        /// 获取城市区域列表
        /// </summary>
        /// <param name="request"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task<Result<IList<ChinaAreaDto>>> GetListAsync([Required][FromQuery] ChinaAreaQueryCommand request, CancellationToken cancellationToken = default)
        {
            var res = await chinaAreaService.GetListAsync(request, cancellationToken);

            return RestFull.Success(data: res);
        }
    }
}
