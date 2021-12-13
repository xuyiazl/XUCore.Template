using XUCore.Template.Razor2.Applaction.Article;
using XUCore.Template.Razor2.DbService.Article;

namespace XUCore.Template.Razor2.Web.Pages.Admin.Articles
{
    [Authorize]
    [AccessControl(AccessKey = "content-article-add")]
    public class AddModel : PageModel
    {
        private readonly IArticleAppService articleAppService;
        private readonly ICategoryAppService categoryAppService;
        private readonly ITagAppService tagAppService;
        public AddModel(IArticleAppService articleAppService, ICategoryAppService categoryAppService, ITagAppService tagAppService)
        {
            this.articleAppService = articleAppService;
            this.categoryAppService = categoryAppService;
            this.tagAppService = tagAppService;
        }

        [BindProperty]
        public ArticleDto ArticleDto { get; set; }

        public IList<SelectListItem> CategorySelectItems { get; set; }
        public IList<SelectListItem> TagSelectItems { get; set; }

        public async void OnGetAsync(CancellationToken cancellationToken)
        {
            ArticleDto = new ArticleDto() { Status = Status.Show };

            CategorySelectItems = await categoryAppService.GetSelectItemsCheckAsync(0, cancellationToken);

            TagSelectItems = await tagAppService.GetSelectItemsCheckAsync(0, cancellationToken);
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

        public async Task<IActionResult> OnPostArticleByAddAsync()
        {
            var command = new ArticleCreateCommand
            {
                Picture = ArticleDto.Picture.SafeString(),
                Contents = ArticleDto.Contents.SafeString(),
                CategoryId = ArticleDto.CategoryId,
                Title = ArticleDto.Title.SafeString(),
                Weight = ArticleDto.Weight,
                TagIds = ArticleDto.TagIds?.ToArray(),
                Status = ArticleDto.Status
            };

            if (!command.IsVaild())
                return new Result(StateCode.Fail, "", command.GetErrors("</br>"));

            var res = await articleAppService.CreateAsync(command, CancellationToken.None);

            if (res > 0)
                return new Result(StateCode.Success, "", "添加成功");
            else
                return new Result(StateCode.Fail, "", "添加失败");
        }

    }
}