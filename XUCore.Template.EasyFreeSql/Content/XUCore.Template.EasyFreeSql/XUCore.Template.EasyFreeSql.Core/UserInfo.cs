﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XUCore.Ddd.Domain;

namespace XUCore.Template.EasyFreeSql.Core
{
    public interface IUserInfo : IUser
    {

    }

    public class UserInfo : User, IUserInfo
    {
        public UserInfo(IServiceProvider serviceProvider) : base(serviceProvider)
        {

        }
    }
}