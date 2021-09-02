using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XUCore.Template.FreeSql.Applaction;
using XUCore.Template.FreeSql.Persistence;

namespace XUCore.Template.FreeSql.Backstage
{
    public static class Startup
    {
        public static void ConfigureServices(HostBuilderContext hostContext, IServiceCollection services)
        {
            services.AddLogging();

            services.AddApplication(hostContext.Configuration, "backstage");

            services.AddDbContext(hostContext.Configuration);

            services.AddHostedService<MainService>();
        }
    }
}
