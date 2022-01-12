using XUCore.Template.WeChat.Core;
using XUCore.Template.WeChat.Persistence.Entities;
using XUCore.Template.WeChat.Persistence.Enums;

namespace XUCore.Template.WeChat.DbService.Notice
{
    /// <summary>
    /// 公告修改命令
    /// </summary>
    public class NoticeUpdateCommand : UpdateCommand<long>, IMapFrom<NoticeEntity>
    {
        /// <summary>
        /// 标题
        /// </summary>
        [Required]
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

        public override bool IsVaild()
        {
            ValidationResult = new Validator().Validate(this);

            return ValidationResult.IsValid;
        }

        public void Mapping(Profile profile) =>
            profile.CreateMap<NoticeUpdateCommand, NoticeEntity>()
            ;

        public class Validator : CommandIdValidator<NoticeUpdateCommand, bool, long>
        {
            public Validator()
            {
                AddIdValidator();

                RuleFor(x => x.Title).NotEmpty().WithName("标题");
                RuleFor(x => x.Status).IsInEnum().NotEqual(Status.Default).WithName("数据状态");
            }
        }
    }
}
