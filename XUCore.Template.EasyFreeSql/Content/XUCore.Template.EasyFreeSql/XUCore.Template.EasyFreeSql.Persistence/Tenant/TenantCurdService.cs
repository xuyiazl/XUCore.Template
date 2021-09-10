using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XUCore.Template.EasyFreeSql.Core.Auth;
using XUCore.Template.EasyFreeSql.Persistence.Entities;

namespace XUCore.Template.EasyFreeSql.Persistence
{
    public class TenantCurdService<TEntity> : CurdService<TEntity, long> where TEntity : EntityFull<long>, new()
    {
        protected TenantCurdService(IdleBusUnitOfWorkManager muowm, IMapper mapper, IUser user) : base(muowm.Orm, mapper)
        {
            muowm.Binding(this);

            User = user;
        }
    }
}
