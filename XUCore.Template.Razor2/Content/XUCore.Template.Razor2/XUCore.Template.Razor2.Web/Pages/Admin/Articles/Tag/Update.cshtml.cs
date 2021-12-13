using XUCore.Template.Razor2.Applaction.Article;
using XUCore.Template.Razor2.DbService.Article;

namespace XUCore.Template.Razor2.Web.Pages.Admin.Articles.Tag
{
    [Authorize]
    [AccessControl(AccessKey = "content-article-edit")]
    public class UpdateModel : PageModel
    {
        private readonly ITagAppService  tagAppService;

        public UpdateModel(ITagAppService tagAppService)
        {
            this.tagAppService = tagAppService;
        }

        [BindProperty]
        public TagDto TagDto { get; set; }

        public async Task OnGetAsync(int id, CancellationToken cancellationToken)
        {
            TagDto = await tagAppService.GetByIdAsync(id, cancellationToken);
        }

        public async Task<IActionResult> OnPutUpdateRowAsync(int id)
        {
            var command = new TagUpdateCommand
            {
                Id = TagDto.Id,
                Name = TagDto.Name.SafeString(),
                Weight = TagDto.Weight,
                Status = TagDto.Status
            };

            if (!command.IsVaild())
                return new Result(StateCode.Fail, "", command.GetErrors("</br>"));

            var res = await tagAppService.UpdateAsync(command);

            if (res > 0)
            {
                return new Result(StateCode.Success, "", "修改成功");
            }
            else
                return new Result(StateCode.Fail, "", "修改失败");
        }
    }
}