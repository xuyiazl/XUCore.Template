using XUCore.Template.Razor.Applaction.Article;
using XUCore.Template.Razor.DbService.Article;

namespace XUCore.Template.Razor.Web.Pages.Admin
{
    [Authorize]
    [NoAccessControl]
    public class IndexModel : PageModel
    {
        public IndexModel()
        {

        }
    }
}