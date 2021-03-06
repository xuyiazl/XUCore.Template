using Microsoft.AspNetCore.Mvc.ApplicationModels;

namespace XUCore.Template.Razor.Applaction
{
    public class DefaultRouteRemovalPageRouteModelConvention : IPageRouteModelConvention
    {
        private readonly string routeToRemove;

        public DefaultRouteRemovalPageRouteModelConvention(string pageRoute)
        {
            this.routeToRemove = pageRoute;
        }

        public void Apply(PageRouteModel model)
        {
            for (int i = 0; i < model.Selectors.Count; i++)
            {
                var selector = model.Selectors[i];
                for (int j = 0; j < selector.EndpointMetadata.Count; j++)
                {
                    if ((selector.EndpointMetadata[j] as PageRouteMetadata)?.PageRoute == routeToRemove)
                    {
                        model.Selectors.Remove(selector);
                        return;
                    }
                }
            }
        }
    }
}
