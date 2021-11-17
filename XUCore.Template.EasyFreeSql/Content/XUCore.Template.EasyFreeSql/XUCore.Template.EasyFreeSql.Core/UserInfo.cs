namespace XUCore.Template.EasyFreeSql.Core
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
