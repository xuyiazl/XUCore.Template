using Nigel.Application.Interfaces;
using Nigel.Domain.Sys.Base.Category;

namespace Nigel.Web.Pages.Admin.Sys.Base.Category
{
    [Authorize]
    [AccessControl(AccessKey = "base-category-add")]
    public class AddModel : PageModel
    {
        private readonly IBasicsAppService basicsAppService;

        public AddModel(IBasicsAppService basicsAppService)
        {
            this.basicsAppService = basicsAppService;
        }

        [BindProperty]
        public CategoryDto Category { get; set; } = new CategoryDto()
        {
            Status = Domain.Core.Enums.Status.Show
        };
        public IList<SelectListItem> DropDownTreeList { get; set; } = new List<SelectListItem>();

        public void OnGet()
        {
            Category.Guid = Guid.NewGuid().ToString();

            DropDownTreeList = basicsAppService.GetCategoryDropdownByTree();
        }

        public async Task<IActionResult> OnPostBaseCategoryByAddAsync()
        {
            CategoryCreateCommand command = new CategoryCreateCommand();

            command.Name = Category.Name.SafeString();
            command.Content = Category.Content.SafeString();
            command.Code = Category.Code.SafeString();
            command.ParentGuid = Category.ParentGuid.SafeString();
            command.Remark = Category.Remark.SafeString();
            command.Weight = Category.Weight;
            command.Css = Category.Css.SafeString();

            command.Status = Category.Status;

            if (!command.IsVaild())
                return new Result(StateCode.Fail, "", command.GetErrors("</br>"));

            var res = await basicsAppService.CreateCategoryAsync(command);

            if (res > 0)
                return new Result(StateCode.Success, "", "添加成功");
            else
                return new Result(StateCode.Fail, "", "添加失败");
        }
    }
}