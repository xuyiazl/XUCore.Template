using AutoMapper;
using FreeSql;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using XUCore.Ddd.Domain.Exceptions;
using XUCore.Extensions;
using XUCore.NetCore;
using XUCore.NetCore.DynamicWebApi;
using XUCore.NetCore.FreeSql;
using XUCore.NetCore.FreeSql.Curd;
using XUCore.Paging;
using XUCore.Template.EasyFreeSql.Core;
using XUCore.Template.EasyFreeSql.Persistence.Entities.User;

namespace XUCore.Template.EasyFreeSql.Applaction.User.Menu
{
    /// <summary>
    /// 用户导航管理
    /// </summary>
    [ApiExplorerSettings(GroupName = ApiGroup.Admin)]
    [DynamicWebApi]
    public class MenuAppService : IMenuAppService, IDynamicWebApi
    {
        protected readonly FreeSqlUnitOfWorkManager unitOfWork;
        protected readonly IBaseRepository<MenuEntity> repo;
        protected readonly IMapper mapper;
        protected readonly IUserInfo user;
        public MenuAppService(IServiceProvider serviceProvider)
        {
            this.unitOfWork = serviceProvider.GetService<FreeSqlUnitOfWorkManager>();
            this.repo = unitOfWork.Orm.GetRepository<MenuEntity>();
            this.mapper = serviceProvider.GetService<IMapper>();
            this.user = serviceProvider.GetService<IUserInfo>();
        }

        /// <summary>
        /// 创建导航
        /// </summary>
        /// <param name="request"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task<Result<long>> CreateAsync([Required][FromBody] MenuCreateCommand request, CancellationToken cancellationToken = default)
        {
            var entity = mapper.Map<MenuCreateCommand, MenuEntity>(request);

            var res = await repo.InsertAsync(entity, cancellationToken);

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
            var entity = await repo.Select.WhereDynamic(request.Id).ToOneAsync<MenuEntity>(cancellationToken);

            if (entity == null)
                return RestFull.Fail(data: 0);

            entity = mapper.Map(request, entity);

            var res = await repo.UpdateAsync(entity, cancellationToken);


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
            var entity = await repo.Select.WhereDynamic(id).ToOneAsync(cancellationToken);

            if (entity.IsNull())
                Failure.Error("没有找到该记录");

            switch (field.ToLower())
            {
                case "name":
                    entity.Name = value;
                    break;
                case "icon":
                    entity.Icon = value;
                    break;
                case "url":
                    entity.Url = value;
                    break;
                case "onlycode":
                    entity.OnlyCode = value;
                    break;
                case "sort":
                    entity.Sort = value.ToInt();
                    break;
            }

            var res = await repo.UpdateAsync(entity, cancellationToken);

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
            var list = await repo.Select.Where(c => ids.Contains(c.Id)).ToListAsync<MenuEntity>(cancellationToken);

            list.ForEach(c => c.Enabled = enabled);

            var res = await repo.UpdateAsync(list, cancellationToken);

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
            var res = await unitOfWork.Orm.Delete<MenuEntity>(ids).ExecuteAffrowsAsync(cancellationToken);

            if (res > 0)
            {
                await unitOfWork.Orm.Delete<RoleMenuEntity>().Where(c => ids.Contains(c.MenuId)).ExecuteAffrowsAsync(cancellationToken);
            }

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
            var res = await repo.Select.WhereDynamic(id).ToOneAsync<MenuDto>(cancellationToken);

            return RestFull.Success(data: res);
        }
        /// <summary>
        /// 获取导航树形结构
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task<Result<List<MenuTreeDto>>> GetTreeAsync(CancellationToken cancellationToken = default)
        {
            //var res = await repo.Select.OrderByDescending(c => c.Sort).OrderBy(c => c.CreatedAt).ToListAsync<MenuTreeDto>(cancellationToken);

            //var tree = res.ToTree(
            //    rootWhere: (r, c) => c.ParentId == 0,
            //    childsWhere: (r, c) => r.Id == c.ParentId,
            //    addChilds: (r, datalist) =>
            //    {
            //        r.Childs ??= new List<MenuTreeDto>();
            //        r.Childs.AddRange(datalist);
            //    });

            var res = await repo.Select.OrderByDescending(c => c.Sort).OrderBy(c => c.CreatedAt).ToTreeListAsync(cancellationToken);

            var tree = mapper.Map<List<MenuEntity>, List<MenuTreeDto>>(res);

            return RestFull.Success(data: tree);
        }
        /// <summary>
        /// 获取导航列表
        /// </summary>
        /// <param name="request"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task<Result<List<MenuDto>>> GetListAsync([Required][FromQuery] MenuQueryCommand request, CancellationToken cancellationToken = default)
        {
            var select = repo.Select
                   .Where(c => c.IsMenu == request.IsMenu)
                   .Where(c => c.Enabled == request.Enabled)
                   .OrderBy(c => c.Id);

            if (request.Limit > 0)
                select = select.Take(request.Limit);

            var res = await select.ToListAsync<MenuDto>(cancellationToken);

            return RestFull.Success(data: res);
        }
        /// <summary>
        /// 获取分页
        /// </summary>
        /// <param name="request"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task<PagedModel<MenuDto>> GetPageAsync([Required][FromQuery] MenuQueryPagedCommand request, CancellationToken cancellationToken)
        {
            var res = await repo.Select
                .Where(c => c.Enabled == request.Enabled)
                .WhereIf(request.Keyword.NotEmpty(), c => c.Name.Contains(request.Keyword))
                .OrderBy(c => c.Id)
                .ToPagedListAsync<MenuEntity, MenuDto>(request.CurrentPage, request.PageSize, cancellationToken);

            return res.ToModel();
        }
    }
}
