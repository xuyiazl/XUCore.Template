using XUCore.Template.Razor2.Applaction.Article;
using XUCore.Template.Razor2.DbService.Article;

namespace XUCore.Template.Razor2.Web.Pages.Admin.Articles.Tag
{
    [Authorize]
    [AccessControl(AccessKey = "content-article-add")]
    public class AddModel : PageModel
    {
        private readonly ITagAppService tagAppService;

        public AddModel(ITagAppService tagAppService)
        {
            this.tagAppService = tagAppService;
        }

        [BindProperty]
        public TagDto TagDto { get; set; } = new TagDto()
        {
            Status = Status.Show
        };

        public async Task<IActionResult> OnPostAddAsync(CancellationToken cancellationToken)
        {
            var command = new TagCreateCommand
            {
                Name = TagDto.Name.SafeString(),
                Weight = TagDto.Weight,
                Status = TagDto.Status
            };

            if (!command.IsVaild())
                return new Result(StateCode.Fail, "", command.GetErrors("</br>"));

            var res = await tagAppService.CreateAsync(command, cancellationToken);

            if (res > 0)
                return new Result(StateCode.Success, "", "添加成功");
            else
                return new Result(StateCode.Fail, "", "添加失败");
        }
    }
}