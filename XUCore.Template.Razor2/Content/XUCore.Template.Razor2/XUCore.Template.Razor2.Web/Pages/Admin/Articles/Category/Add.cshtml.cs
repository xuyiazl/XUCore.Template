using XUCore.Template.Razor2.Applaction.Article;
using XUCore.Template.Razor2.DbService.Article;

namespace XUCore.Template.Razor2.Web.Pages.Admin.Articles.Category
{
    [Authorize]
    [AccessControl(AccessKey = "content-article-add")]
    public class AddModel : PageModel
    {
        private readonly ICategoryAppService categoryAppService;

        public AddModel(ICategoryAppService categoryAppService)
        {
            this.categoryAppService = categoryAppService;
        }

        [BindProperty]
        public CategoryDto CategoryDto { get; set; } = new CategoryDto()
        {
            Status = Status.Show
        };

        public async Task<IActionResult> OnPostAddAsync(CancellationToken cancellationToken)
        {
            var command = new CategoryCreateCommand
            {
                Name = CategoryDto.Name.SafeString(),
                Weight = CategoryDto.Weight,
                Status = CategoryDto.Status
            };

            if (!command.IsVaild())
                return new Result(StateCode.Fail, "", command.GetErrors("</br>"));

            var res = await categoryAppService.CreateAsync(command, cancellationToken);

            if (res > 0)
                return new Result(StateCode.Success, "", "添加成功");
            else
                return new Result(StateCode.Fail, "", "添加失败");
        }
    }
}