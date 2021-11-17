namespace XUCore.Template.FreeSql.Core
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
