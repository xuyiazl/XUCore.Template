using XUCore.Template.WeChat.Core;
using XUCore.Template.WeChat.Persistence.Entities.Auth;
using XUCore.Template.WeChat.Persistence.Enums;
using XUCore.Template.WeChat.Persistence.Entities.User;

namespace XUCore.Template.WeChat.Persistence
{
    public class DbHelper
    {
        /// <summary>
        /// 偏移时间
        /// </summary>
        public static TimeSpan TimeOffset;

        /// <summary>
        /// 创建数据库
        /// </summary>
        /// <param name="dbConfig"></param>
        /// <returns></returns>
        public async static Task CreateDatabaseAsync(ConnectionSettings dbConfig)
        {
            if (!dbConfig.CreateDb || dbConfig.Type == DataType.Sqlite)
            {
                return;
            }

            var db = new FreeSqlBuilder()
                    .UseConnectionString(dbConfig.Type, dbConfig.CreateDbConnectionString)
                    .Build();

            try
            {
                Console.WriteLine("\r\n create database started");
                await db.Ado.ExecuteNonQueryAsync(dbConfig.CreateDbSql);
                Console.WriteLine(" create database succeed");
            }
            catch (Exception e)
            {
                Console.WriteLine($" create database failed.\n {e.Message}");
            }
        }

        /// <summary>
        /// 获得指定程序集表实体
        /// </summary>
        /// <returns></returns>
        public static Type[] GetEntityTypes()
        {
            var assemblyNames = new List<string>()
            {
                "XUCore.Template.WeChat.Persistence"
            };

            var entityTypes = new List<Type>();

            foreach (var assemblyName in assemblyNames)
            {
                foreach (Type type in Assembly.Load(assemblyName).GetExportedTypes())
                {
                    foreach (Attribute attribute in type.GetCustomAttributes())
                    {
                        if (attribute is TableAttribute tableAttribute)
                        {
                            if (tableAttribute.DisableSyncStructure == false)
                            {
                                entityTypes.Add(type);
                            }
                        }
                    }
                }
            }

            return entityTypes.ToArray();
        }

        /// <summary>
        /// 配置实体
        /// </summary>
        public static void ConfigEntity(IFreeSql db)
        {
            //获得指定程序集表实体
            var entityTypes = GetEntityTypes();

            foreach (var entityType in entityTypes)
            {
                db.CodeFirst.Entity(entityType, a =>
                {
                    //a.Ignore(tenantId);
                });
            }
        }

        /// <summary>
        /// 审计数据
        /// </summary>
        /// <param name="e"></param>
        /// <param name="timeOffset"></param>
        /// <param name="user"></param>
        public static void AuditValue(AuditValueEventArgs e, TimeSpan timeOffset, IUserInfo user)
        {
            if (e.Property.GetCustomAttribute<ServerTimeAttribute>(false) != null
                   && (e.Column.CsType == typeof(DateTime) || e.Column.CsType == typeof(DateTime?))
                   && (e.Value.IsNull() || (DateTime)e.Value == default || (DateTime?)e.Value == default))
            {
                e.Value = DateTime.Now.Subtract(timeOffset);
            }

            //if (e.Column.CsType == typeof(long)
            //&& e.Property.GetCustomAttribute<SnowflakeAttribute>(false) is SnowflakeAttribute snowflakeAttribute
            //&& snowflakeAttribute.Enable && e.Value.IsNull())
            //{
            //    e.Value = Id.SnowflakeId;
            //}


            if (e.AuditValueType == AuditValueType.Insert)
            {
                switch (e.Property.Name)
                {
                    case nameof(EntityAdd.CreatedAtUserId):
                        if (e.Value.IsNull())
                            e.Value = user.UserId;
                        break;

                    case nameof(EntityAdd.CreatedAtUserName):
                        if (e.Value.IsNull())
                            e.Value = user?.Name;
                        break;

                    case nameof(EntityAdd.CreatedAt):
                        if (e.Value.IsNull())
                            e.Value = DateTime.Now.Subtract(timeOffset);
                        break;
                }
            }
            else if (e.AuditValueType == AuditValueType.Update)
            {
                switch (e.Property.Name)
                {
                    case nameof(EntityUpdate.ModifiedAtUserId):
                        if (e.Value.IsNull())
                            e.Value = user.UserId;
                        break;

                    case nameof(EntityUpdate.ModifiedAtUserName):
                        if (e.Value.IsNull())
                            e.Value = user?.Name;
                        break;

                    case nameof(EntityUpdate.ModifiedAt):
                        if (e.Value.IsNull())
                            e.Value = DateTime.Now.Subtract(timeOffset);
                        break;
                }
            }
        }

        /// <summary>
        /// 同步结构
        /// </summary>
        public static void SyncStructure(IFreeSql db, string msg = null, ConnectionSettings dbConfig = null)
        {
            //打印结构比对脚本
            //var dDL = db.CodeFirst.GetComparisonDDLStatements<PermissionEntity>();
            //Console.WriteLine("\r\n " + dDL);

            //打印结构同步脚本
            //db.Aop.SyncStructureAfter += (s, e) =>
            //{
            //    if (e.Sql.NotNull())
            //    {
            //        Console.WriteLine(" sync structure sql:\n" + e.Sql);
            //    }
            //};

            // 同步结构
            var dbType = dbConfig.Type.ToString();

            Console.WriteLine($"\r\n {(msg.NotEmpty() ? msg : $"sync {dbType} structure")} started");

            if (dbConfig.Type == DataType.Oracle)
            {
                db.CodeFirst.IsSyncStructureToUpper = true;
            }

            //获得指定程序集表实体
            var entityTypes = GetEntityTypes();

            db.CodeFirst.SyncStructure(entityTypes);

            Console.WriteLine($" {(msg.NotEmpty() ? msg : $"sync {dbType} structure")} succeed");
        }

        /// <summary>
        /// 同步基础数据
        /// </summary>
        /// <param name="db"></param>
        public static void SyncData(IFreeSql db)
        {
            {
                var repo = db.GetRepository<MenuEntity>();
                if (!repo.Select.Any())
                {
                    var menus = new List<MenuEntity>
                    {
                        new MenuEntity
                        {
                            Name = "系统设置",
                            Icon = "fa fa-cogs text-danger",
                            Url = "#",
                            OnlyCode = "sys",
                            Weight = 99,
                            IsMenu = true,
                            IsExpress = false,
                             Status =  Status.Show,
                            Childs = new List<MenuEntity>
                    {
                        new MenuEntity {
                            Name = "导航设置",
                            Icon = "fa fa-chain text-primary",
                            Url = "/admin/sys/admin/menu/list",
                            OnlyCode = "sys-menus",
                            Weight = 99,
                            IsMenu = true,
                            IsExpress = false,
                             Status =  Status.Show,
                            Childs = new List<MenuEntity>{
                                new MenuEntity {Name = "添加",Url = "#",OnlyCode = "sys-menus-add",Weight = 9,IsMenu = false,IsExpress = false,Status =  Status.Show},
                                new MenuEntity {Name = "修改",Url = "#",OnlyCode = "sys-menus-edit",Weight = 8,IsMenu = false,IsExpress = false,Status =  Status.Show},
                                new MenuEntity {Name = "删除",Url = "#",OnlyCode = "sys-menus-delete",Weight = 7,IsMenu = false,IsExpress = false,Status =  Status.Show},
                                new MenuEntity {Name = "列表",Url = "#",OnlyCode = "sys-menus-list",Weight = 6,IsMenu = false,IsExpress = false,Status =  Status.Show}
                            }
                        },
                        new MenuEntity {
                            Name = "角色管理",
                            Icon = "icon-lock text-danger",
                            Url = "/admin/sys/admin/role/list",
                            OnlyCode = "sys-role",
                            Weight = 99,
                            IsMenu = true,
                            IsExpress = false,
                             Status =  Status.Show,
                            Childs = new List<MenuEntity>{
                                new MenuEntity {Name = "添加",Url = "#",OnlyCode = "sys-role-add",Weight = 9,IsMenu = false,IsExpress = false,Status =  Status.Show},
                                new MenuEntity {Name = "修改",Url = "#",OnlyCode = "sys-role-edit",Weight = 8,IsMenu = false,IsExpress = false,Status =  Status.Show},
                                new MenuEntity {Name = "删除",Url = "#",OnlyCode = "sys-role-delete",Weight = 7,IsMenu = false,IsExpress = false,Status =  Status.Show},
                                new MenuEntity {Name = "列表",Url = "#",OnlyCode = "sys-role-list",Weight = 6,IsMenu = false,IsExpress = false,Status =  Status.Show}
                            }
                        },
                        new MenuEntity {
                            Name = "帐号管理",
                            Icon = "icon-users text-success",
                            Url = "/admin/sys/admin/user/list",
                            OnlyCode = "sys-admin",
                            Weight = 99,
                            IsMenu = true,
                            IsExpress = false,
                             Status =  Status.Show,
                            Childs = new List<MenuEntity>{
                                new MenuEntity {Name = "添加",Url = "#",OnlyCode = "sys-admin-add",Weight = 9,IsMenu = false,IsExpress = false,Status =  Status.Show},
                                new MenuEntity {Name = "修改",Url = "#",OnlyCode = "sys-admin-edit",Weight = 8,IsMenu = false,IsExpress = false,Status =  Status.Show},
                                new MenuEntity {Name = "删除",Url = "#",OnlyCode = "sys-admin-delete",Weight = 7,IsMenu = false,IsExpress = false,Status =  Status.Show},
                                new MenuEntity {Name = "列表",Url = "#",OnlyCode = "sys-admin-list",Weight = 6,IsMenu = false,IsExpress = false,Status =  Status.Show},
                                new MenuEntity {Name = "禁止登录",Url = "#",OnlyCode = "sys-admin-login",Weight = 5,IsMenu = false,IsExpress = false,Status =  Status.Show},
                                new MenuEntity {Name = "授权",Url = "#",OnlyCode = "sys-admin-accredit",Weight = 4,IsMenu = false,IsExpress = false,Status =  Status.Show}
                            }
                        },
                        new MenuEntity {
                            Name = "登录记录",
                            Icon = "fa fa-eye text-dark",
                            Url = "/admin/sys/admin/record",
                            OnlyCode = "sys-loginrecord",
                            Weight = 99,
                            IsMenu = true,
                            IsExpress = false,
                             Status =  Status.Show,
                        },
                        new MenuEntity {
                            Name = "公告管理",
                            Icon = "fa fa-bullhorn text-success",
                            Url = "/admin/sys/admin/notices/list",
                            OnlyCode = "sys-notice",
                            Weight = 99,
                            IsMenu = true,
                            IsExpress = false,
                             Status =  Status.Show,
                            Childs = new List<MenuEntity>{
                                new MenuEntity {Name = "添加",Url = "#",OnlyCode = "sys-notice-add",Weight = 9,IsMenu = false,IsExpress = false,Status =  Status.Show},
                                new MenuEntity {Name = "修改",Url = "#",OnlyCode = "sys-notice-edit",Weight = 8,IsMenu = false,IsExpress = false,Status =  Status.Show},
                                new MenuEntity {Name = "删除",Url = "#",OnlyCode = "sys-notice-delete",Weight = 7,IsMenu = false,IsExpress = false,Status =  Status.Show},
                                new MenuEntity {Name = "列表",Url = "#",OnlyCode = "sys-notice-list",Weight = 6,IsMenu = false,IsExpress = false,Status =  Status.Show}
                            }
                        },
                        new MenuEntity {
                            Name = "微信用户",
                            Icon = "fa fa-user text-dark",
                            Url = "/admin/wechat/list",
                            OnlyCode = "wechat-user",
                            Weight = 98,
                            IsMenu = true,
                            IsExpress = false,
                             Status =  Status.Show,
                            Childs = new List<MenuEntity>{
                                new MenuEntity {Name = "手机号码",Url = "#",OnlyCode = "wechat-mobile",Weight = 9,IsMenu = false,IsExpress = false,Status =  Status.Show},
                                new MenuEntity {Name = "用户跟进",Url = "#",OnlyCode = "wechat-genjin",Weight = 8,IsMenu = false,IsExpress = false,Status =  Status.Show}
                            }
                        }
                    }
                        },
                        new MenuEntity
                        {
                            Name = "内容管理",
                            Icon = "fa fa-book text-success",
                            Url = "#",
                            OnlyCode = "content",
                            Weight = 98,
                            IsMenu = true,
                            IsExpress = false,
                             Status =  Status.Show,
                            Childs = new List<MenuEntity>
                    {
                        new MenuEntity {
                            Name = "文章管理",
                            Icon = "fa fa-folder-open-o text-warning",
                            Url = "/admin/articles/list",
                            OnlyCode = "content-article",
                            Weight = 99,
                            IsMenu = true,
                            IsExpress = false,
                             Status =  Status.Show,
                            Childs = new List<MenuEntity>{
                                new MenuEntity {Name = "添加",Url = "#",OnlyCode = "content-article-add",Weight = 9,IsMenu = false,IsExpress = false,Status =  Status.Show},
                                new MenuEntity {Name = "修改",Url = "#",OnlyCode = "content-article-edit",Weight = 8,IsMenu = false,IsExpress = false,Status =  Status.Show},
                                new MenuEntity {Name = "删除",Url = "#",OnlyCode = "content-article-delete",Weight = 7,IsMenu = false,IsExpress = false,Status =  Status.Show},
                                new MenuEntity {Name = "列表",Url = "#",OnlyCode = "content-article-list",Weight = 6,IsMenu = false,IsExpress = false,Status =  Status.Show}
                            }
                        }
                    }
                        }
                    };

                    void recursionMenu(MenuEntity menu)
                    {
                        if (menu.Childs == null) return;

                        foreach (var child in menu.Childs)
                        {
                            child.ParentId = menu.Id;

                            repo.Insert(child);

                            recursionMenu(child);
                        }
                    }

                    foreach (var menu in menus)
                    {
                        repo.Insert(menu);
                        recursionMenu(menu);
                    }
                }
            }
            {
                var repo = db.GetRepository<RoleEntity>();
                if (!repo.Select.Any())
                {
                    var roleMenuRepo = db.GetRepository<RoleMenuEntity>();
                    var role = new RoleEntity
                    {
                        Status = Status.Show,
                        IsDeleted = false,
                        Name = "超级管理员"
                    };
                    repo.Insert(role);
                    var roleMenus = db.Select<MenuEntity>().ToList().Select(c => new RoleMenuEntity { RoleId = role.Id, MenuId = c.Id }).ToList();
                    roleMenuRepo.Insert(roleMenus);
                }
            }
            {
                var repo = db.GetRepository<UserEntity>();
                if (!repo.Select.Any())
                {
                    var userRoleRepo = db.GetRepository<UserRoleEntity>();
                    var user = new UserEntity
                    {
                        Name = "超级管理员",
                        UserName = "admin",
                        Mobile = "13500000000",
                        Password = Encrypt.Md5By32("123456"),
                        Company = "超级管理员",
                        Status = Status.Show,
                        IsDeleted = false,
                        Location = "",
                        LoginCount = 0,
                        LoginLastIp = "",
                        LoginLastTime = DateTime.Now,
                        Picture = "",
                        Position = ""
                    };
                    repo.Insert(user);
                    var userRoles = db.Select<RoleEntity>().ToList().Select(c => new UserRoleEntity { UserId = user.Id, RoleId = c.Id }).ToList();
                    userRoleRepo.Insert(userRoles);
                }
            }
        }
    }

}

