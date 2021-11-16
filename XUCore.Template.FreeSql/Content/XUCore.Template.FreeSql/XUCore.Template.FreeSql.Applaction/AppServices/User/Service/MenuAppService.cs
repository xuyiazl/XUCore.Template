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
using XUCore.Template.FreeSql.DbService.Auth.Menu;

namespace XUCore.Template.FreeSql.Applaction.User
{
    /// <summary>
    /// 用户导航管理
    /// </summary>
    [ApiExplorerSettings(GroupName = ApiGroup.Admin)]
    [DynamicWebApi]
    public class MenuAppService : IMenuAppService, IDynamicWebApi
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
        public async Task<Result<long>> CreateAsync([Required][FromBody] MenuCreateCommand request, CancellationToken cancellationToken = default)
        {
            var res = await menuService.CreateAsync(request, cancellationToken);

            if (res != null)
                return RestFull.Success(data: res.Id);
            else
                return RestFull.Fail(data: 0L);
        }
        /// <summary>
        /// 更新导航信息
        /// </summary>
        /// <param name="request"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task<Result<int>> UpdateAsync([Required][FromBody] MenuUpdateCommand request, CancellationToken cancellationToken = default)
        {
            var res = await menuService.UpdateAsync(request, cancellationToken);

            if (res > 0)
                return RestFull.Success(data: res);
            else
                return RestFull.Fail(data: res);
        }
        /// <summary>
        /// 更新导航指定字段内容
        /// </summary>
        /// <param name="id"></param>
        /// <param name="field"></param>
        /// <param name="value"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task<Result<int>> UpdateFieldAsync([Required][FromQuery] long id, [Required][FromQuery] string field, [FromQuery] string value, CancellationToken cancellationToken = default)
        {
            var res = await menuService.UpdateAsync(id, field, value, cancellationToken);

            if (res > 0)
                return RestFull.Success(data: res);
            else
                return RestFull.Fail(data: res);
        }
        /// <summary>
        /// 更新状态
        /// </summary>
        /// <param name="ids"></param>
        /// <param name="enabled"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task<Result<int>> UpdateEnabledAsync([Required][FromQuery] long[] ids, [Required][FromQuery] bool enabled, CancellationToken cancellationToken = default)
        {
            var res = await menuService.UpdateAsync(ids, enabled, cancellationToken);

            if (res > 0)
                return RestFull.Success(data: res);
            else
                return RestFull.Fail(data: res);
        }
        /// <summary>
        /// 删除导航（物理删除）
        /// </summary>
        /// <param name="ids"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task<Result<int>> DeleteAsync([Required][FromQuery] long[] ids, CancellationToken cancellationToken = default)
        {
            var res = await menuService.DeleteAsync(ids, cancellationToken);

            if (res > 0)
                return RestFull.Success(data: res);
            else
                return RestFull.Fail(data: res);
        }
        /// <summary>
        /// 获取导航信息
        /// </summary>
        /// <param name="id"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [HttpGet("{id}")]
        public async Task<Result<MenuDto>> GetAsync([Required] long id, CancellationToken cancellationToken = default)
        {
            var res = await menuService.GetByIdAsync(id, cancellationToken);

            return RestFull.Success(data: res);
        }
        /// <summary>
        /// 获取导航树形结构
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task<Result<IList<MenuTreeDto>>> GetTreeAsync(CancellationToken cancellationToken = default)
        {
            var res = await menuService.GetListByTreeAsync(cancellationToken);

            return RestFull.Success(data: res);
        }
        /// <summary>
        /// 获取导航列表
        /// </summary>
        /// <param name="request"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task<Result<IList<MenuDto>>> GetListAsync([Required][FromQuery] MenuQueryCommand request, CancellationToken cancellationToken = default)
        {
            var res = await menuService.GetListAsync(request, cancellationToken);

            return RestFull.Success(data: res);
        }
    }
}
