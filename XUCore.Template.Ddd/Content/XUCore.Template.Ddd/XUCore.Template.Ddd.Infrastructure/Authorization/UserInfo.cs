﻿using XUCore.Template.Ddd.Domain.Core;

namespace XUCore.Template.Ddd.Infrastructure.Authorization
{
    public class UserInfo : User, IUserInfo
    {
        public UserInfo(IServiceProvider serviceProvider) : base(serviceProvider)
        {

        }

        public override void SetToken(string id, string token)
        {

        }

        public override void RemoveToken()
        {

        }

        public override bool VaildToken(string token)
        {
            return true;
        }
    }
}
