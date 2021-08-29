﻿
using System;
using System.Collections.Generic;

namespace XUCore.Template.EasyLayer.Persistence.Entities.Admin
{
    /// <summary>
    /// 权限导航表
    /// </summary>
    public partial class AdminMenuEntity : BaseEntity<long>
    {
        public AdminMenuEntity()
        {
            RoleMenus = new List<AdminRoleMenuEntity>();
        }
        /// <summary>
        /// 导航父级id
        /// </summary>
        public long FatherId { get; set; }
        /// <summary>
        /// 名字
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 图标样式
        /// </summary>
        public string Icon { get; set; }
        /// <summary>
        /// 链接地址
        /// </summary>
        public string Url { get; set; }
        /// <summary>
        /// 唯一代码（权限使用）
        /// </summary>
        public string OnlyCode { get; set; }
        /// <summary>
        /// 是否是导航
        /// </summary>
        public bool IsMenu { get; set; }
        /// <summary>
        /// 排序权重
        /// </summary>
        public int Weight { get; set; }
        /// <summary>
        /// 是否是快捷导航
        /// </summary>
        public bool IsExpress { get; set; }
        /// <summary>
        /// 角色导航关联列表
        /// </summary>
        public ICollection<AdminRoleMenuEntity> RoleMenus;
    }
}
