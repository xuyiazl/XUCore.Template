﻿using XUCore.Template.Razor2.Core;
using XUCore.Template.Razor2.Persistence.Entities;
using XUCore.Template.Razor2.Persistence.Enums;

namespace XUCore.Template.Razor2.DbService.Article
{
    /// <summary>
    /// 标签
    /// </summary>
    public class TagDto : DtoBase<TagEntity>
    {
        /// <summary>
        /// 标签名
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 排序权重
        /// </summary>
        public int Weight { get; set; }
        /// <summary>
        /// 状态
        /// </summary>
		public Status Status { get; set; }
    }
}
