using XUCore.Template.WeChat.Applaction.Article;
using XUCore.Template.WeChat.DbService.Article;

namespace XUCore.Template.WeChat.Web.Pages.Admin.Articles.Category
{
    [Authorize]
    [AccessControl(AccessKey = "content-article-edit")]
    public class UpdateModel : PageModel
    {
        private readonly ICategoryAppService categoryAppService;

        public UpdateModel(ICategoryAppService categoryAppService)
        {
            this.categoryAppService = categoryAppService;
        }

        [BindProperty]
        public CategoryDto CategoryDto { get; set; }

        public async Task OnGetAsync(int id, CancellationToken cancellationToken)
        {
            CategoryDto = await categoryAppService.GetByIdAsync(id, cancellationToken);
        }

        public async Task<IActionResult> OnPutUpdateRowAsync(int id)
        {
            var command = new CategoryUpdateCommand
            {
                Id = CategoryDto.Id,
                Name = CategoryDto.Name.SafeString(),
                Weight = CategoryDto.Weight,
                Status = CategoryDto.Status
            };

            if (!command.IsVaild())
                return new Result(StateCode.Fail, "", command.GetErrors("</br>"));

            var res = await categoryAppService.UpdateAsync(command);

            if (res > 0)
            {
                return new Result(StateCode.Success, "", "修改成功");
            }
            else
                return new Result(StateCode.Fail, "", "修改失败");
        }
    }
}
