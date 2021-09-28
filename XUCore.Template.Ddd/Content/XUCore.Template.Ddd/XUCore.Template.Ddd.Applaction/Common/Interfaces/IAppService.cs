using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XUCore.NetCore.DynamicWebApi;
using XUCore.Ddd.Domain;

namespace XUCore.Template.Ddd.Applaction.Common.Interfaces
{
    [DynamicWebApi]
    public interface IAppService : IDynamicWebApi, IScoped
    {

    }
}
