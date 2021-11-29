using Nigel.Application.Interfaces;
using Nigel.Domain.Sys.Base.Category;

namespace Nigel.Web.Pages.Admin.Sys.Base.Category
{
    [Authorize]
    [AccessControl(AccessKey = "base-category-edit")]
    public class UpdateModel : PageModel
    {
        private readonly IBasicsAppService basicsAppService;

        public UpdateModel(IBasicsAppService basicsAppService)
        {
            this.basicsAppService = basicsAppService;
        }

        [BindProperty]
        public CategoryDto Category { get; set; }
        public IList<SelectListItem> DropDownTreeList { get; set; } = new List<SelectListItem>();

        public async Task OnGetAsync(int id)
        {
            Category = await basicsAppService.GetCategoryByIdAsync(id);

            DropDownTreeList = basicsAppService.GetCategoryDropdownByTree();
        }

        public async Task<IActionResult> OnPutBaseCategoryByUpdateRowAsync(int Id)
        {
            CategoryUpdateCommand command = new CategoryUpdateCommand();

            command.Id = Category.Id;
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

            var res = await basicsAppService.UpdateCategoryAsync(command);

            if (res > 0)
            {
                return new Result(StateCode.Success, "", "修改成功");
            }
            else
                return new Result(StateCode.Fail, "", "修改失败");
        }
    }
}