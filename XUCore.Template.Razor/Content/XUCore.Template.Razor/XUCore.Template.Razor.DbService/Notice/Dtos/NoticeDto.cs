using XUCore.Template.Razor.Core;
using XUCore.Template.Razor.Core.Enums;
using XUCore.Template.Razor.Persistence.Entities;

namespace XUCore.Template.Razor.DbService.Notice
{
    /// <summary>
    /// 公告板
    /// </summary>
    public class NoticeDto : DtoBase<NoticeEntity>
    {
        /// <summary>
        /// 用户id
        /// </summary>
        public long UserId { get; set; }
        /// <summary>
        /// 标题
        /// </summary>
        public string Title { get; set; }
        /// <summary>
        /// 正文
        /// </summary>
        public string Contents { get; set; }
        /// <summary>
        /// 级别
        /// </summary>
        public long LevelId { get; set; }
        /// <summary>
        /// 排序权重
        /// </summary>
        public int Weight { get; set; }
        /// <summary>
        /// 数据状态
        /// </summary>
        public Status Status { get; set; }
        /// <summary>
        /// 级别样式
        /// </summary>
        public string LevelCss => Utils.LevelSwap(LevelId).Css;
        /// <summary>
        /// 级别名
        /// </summary>
        public string LevelName => Utils.LevelSwap(LevelId).Name;
    }
}
