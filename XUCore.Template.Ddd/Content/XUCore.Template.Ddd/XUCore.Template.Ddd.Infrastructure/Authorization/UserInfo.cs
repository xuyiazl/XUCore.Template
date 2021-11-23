using XUCore.Template.Ddd.Domain.Core;

namespace XUCore.Template.Ddd.Infrastructure.Authorization
{
    public class UserInfo : User, IUserInfo
    {
        public UserInfo(IServiceProvider serviceProvider) : base(serviceProvider)
        {

        }
    }
}
