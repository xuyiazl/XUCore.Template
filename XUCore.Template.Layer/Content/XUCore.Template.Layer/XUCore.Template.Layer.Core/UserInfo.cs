using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XUCore.Ddd.Domain;
using XUCore.NetCore;

namespace XUCore.Template.Layer.Core
{
    public interface IUserInfo : IUser, IScoped
    {

    }

    public class UserInfo : User, IUserInfo
    {
        public UserInfo(IServiceProvider serviceProvider) : base(serviceProvider)
        {

        }
    }
}
