using XUCore.Template.Razor.Applaction.Article;
using XUCore.Template.Razor.DbService.Article;

namespace XUCore.Template.Razor.Web.Pages.Admin.Articles
{
    [Authorize]
    [AccessControl(AccessKey = "content-article-edit")]
    public class UpdateModel : PageModel
    {
        private readonly IArticleAppService articleAppService;
        private readonly ICategoryAppService categoryAppService;
        private readonly ITagAppService tagAppService;
        public UpdateModel(IArticleAppService articleAppService, ICategoryAppService categoryAppService, ITagAppService tagAppService)
        {
            this.articleAppService = articleAppService;
            this.categoryAppService = categoryAppService;
            this.tagAppService = tagAppService;
        }


        [BindProperty]
        public ArticleDto ArticleDto { get; set; }
        public IList<SelectListItem> CategorySelectItems { get; set; }
        public IList<SelectListItem> TagSelectItems { get; set; }

        public async Task OnGetAsync(int id, CancellationToken cancellationToken)
        {
            ArticleDto = await articleAppService.GetByIdAsync(id, cancellationToken);

            CategorySelectItems = await categoryAppService.GetSelectItemsCheckAsync(ArticleDto.CategoryId, cancellationToken);

            TagSelectItems = await tagAppService.GetSelectItemsCheckArrayAsync(ArticleDto.TagIds.ToArray(), cancellationToken);

            ArticleDto.TagIds = null;
        }

        public async Task<IActionResult> OnGetCategoryListAsync(CancellationToken cancellationToken)
        {
            var _categorysList = await categoryAppService.GetListAsync(new CategoryQueryCommand { Status = Status.Show }, cancellationToken);

            var res = _categorysList.ForEach(item => new Tuple<string, string>(item.Id.SafeString(), item.Name));
            return new Result(StateCode.Success, "", "", res);
        }

        public async Task<IActionResult> OnGetTagListAsync(CancellationToken cancellationToken)
        {
            var _tagsList = await tagAppService.GetListAsync(new TagQueryCommand { Status = Status.Show }, cancellationToken);

            var res = _tagsList.ForEach(item => new Tuple<string, string>(item.Id.SafeString(), item.Name));
            return new Result(StateCode.Success, "", "", res);
        }

        public async Task<IActionResult> OnPutArticleByUpdateRowAsync()
        {
            var command = new ArticleUpdateCommand
            {
                Id = ArticleDto.Id,
                TagIds = ArticleDto.TagIds?.ToArray(),
                Title = ArticleDto.Title.SafeString(),
                Weight = ArticleDto.Weight,
                Contents = ArticleDto.Contents.SafeString(),
                CategoryId = ArticleDto.CategoryId,
                Status = ArticleDto.Status,
                Picture = ArticleDto.Picture.SafeString()
            };

            if (!command.IsVaild())
                return new Result(StateCode.Fail, "", command.GetErrors("</br>"));

            var res = await articleAppService.UpdateAsync(command, CancellationToken.None);

            if (res > 0)
                return new Result(StateCode.Success, "", "修改成功");
            else
                return new Result(StateCode.Fail, "", "修改失败");
        }
    }
}