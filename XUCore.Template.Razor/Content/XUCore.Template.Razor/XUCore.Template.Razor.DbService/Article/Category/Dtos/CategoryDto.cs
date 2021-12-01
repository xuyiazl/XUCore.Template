using XUCore.Template.Razor.Core;
using XUCore.Template.Razor.Persistence.Entities;
using XUCore.Template.Razor.Persistence.Enums;

namespace XUCore.Template.Razor.DbService.Article
{
    /// <summary>
    /// 目录
    /// </summary>
    public class CategoryDto : DtoBase<CategoryEntity>
    {
        /// <summary>
        /// 目录名
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
