
using XUCore.Template.Razor.Core;
using XUCore.Template.Razor.Persistence.Entities;
using XUCore.Template.Razor.Persistence.Enums;

namespace XUCore.Template.Razor.DbService.Notice
{
    /// <summary>
    /// 创建公告命令
    /// </summary>
    public class NoticeCreateCommand : CreateCommand, IMapFrom<NoticeEntity>
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
            profile.CreateMap<NoticeCreateCommand, NoticeEntity>()
                .ForMember(c => c.UserId, c => c.MapFrom(s => Web.GetService<IUserInfo>().GetId<long>()))
            ;

        public class Validator : CommandValidator<NoticeCreateCommand>
        {
            public Validator()
            {
                RuleFor(x => x.Title).NotEmpty().WithName("标题");
                RuleFor(x => x.Status).IsInEnum().NotEqual(Status.Default).WithName("数据状态");
            }
        }
    }
}
