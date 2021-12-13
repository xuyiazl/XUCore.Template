using XUCore.Template.Razor2.Persistence.Enums;
using XUCore.Template.Razor2.Persistence.Entities.User;

namespace XUCore.Template.Razor2.Persistence.Entities
{
    /// <summary>
    /// 后台公告板
    /// </summary>
	[Table(Name = "sys_notice")]
    //[Index("idx_{tablename}_01", nameof(UserName), true)]
    public partial class NoticeEntity : EntityFull
    {
        /// <summary>
        /// 用户id
        /// </summary>
        public long UserId { get; set; }
        /// <summary>
        /// 标题
        /// </summary>
        [Column(StringLength = 100)]
        public string Title { get; set; }
        /// <summary>
        /// 正文
        /// </summary>
        [Column(StringLength = -2)]
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
        /// 启用
        /// </summary>
		public Status Status { get; set; }
        /// <summary>
        /// 对应用户
        /// </summary>
        public UserEntity User { get; set; }
    }
}
