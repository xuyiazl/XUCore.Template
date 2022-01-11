﻿using XUCore.Template.WeChat.Persistence.Entities;
using XUCore.Template.WeChat.Persistence.Enums;

namespace XUCore.Template.WeChat.DbService.Article
{
    public interface ICategoryService : ICurdService<long, CategoryEntity, CategoryDto, CategoryCreateCommand, CategoryUpdateCommand, CategoryQueryCommand, CategoryQueryPagedCommand>, IScoped
    {
        /// <summary>
        /// 更新部分字段
        /// </summary>
        /// <param name="id"></param>
        /// <param name="field"></param>
        /// <param name="value"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<int> UpdateAsync(long id, string field, string value, CancellationToken cancellationToken);
        /// <summary>
        /// 更新状态
        /// </summary>
        /// <param name="ids"></param>
        /// <param name="status"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<int> UpdateStatusAsync(long[] ids, Status status, CancellationToken cancellationToken);
        /// <summary>
        /// 检查名字是否重复
        /// </summary>
        /// <param name="name"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<bool> AnyAsync(string name, CancellationToken cancellationToken);
    }
}