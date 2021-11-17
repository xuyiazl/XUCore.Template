using XUCore.Template.EasyFreeSql.Core;
using XUCore.Template.EasyFreeSql.Persistence.Entities;
using XUCore.Template.EasyFreeSql.Persistence.Entities.User;

namespace XUCore.Template.EasyFreeSql.Persistence
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
            List<string> assemblyNames = new List<string>()
            {
                "XUCore.Template.EasyFreeSql.Persistence"
            };

            List<Type> entityTypes = new List<Type>();

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
                            e.Value = user.GetId<long>();
                        break;

                    case nameof(EntityAdd.CreatedAtUserName):
                        if (e.Value.IsNull())
                            e.Value = user?.UserName;
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
                            e.Value = user.GetId<long>();
                        break;

                    case nameof(EntityUpdate.ModifiedAtUserName):
                        if (e.Value.IsNull())
                            e.Value = user?.UserName;
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
            var initData = Path.Combine(Directory.GetCurrentDirectory(), "InitData");
            {
                if (Directory.Exists(initData))
                {
                    var any = db.Select<ChinaAreaEntity>().Any();
                    if (!any)
                    {
                        var data = FileHelper.ReadAllTextAsync(Path.Combine(initData, "sys_china_area.json")).GetAwaiter().GetResult();

                        var list = data.ToObject<List<ChinaAreaEntity>>();

                        var res = db.Insert(list).ExecuteAffrows();

                        Console.WriteLine($"初始化数据：省份城市数据 {res} 条...");
                    }
                    else
                        Console.WriteLine($"初始化数据：省份城市数据已存在，无需同步...");
                }
            }
            {
                var repo = db.GetRepository<MenuEntity>();
                if (!repo.Select.Any())
                {
                    var menus = new List<MenuEntity>();
                    menus.Add(new MenuEntity
                    {
                        Name = "系统设置",
                        Icon = "fa fa-cogs text-danger",
                        Url = "#",
                        OnlyCode = "sys",
                        Sort = 99,
                        IsMenu = true,
                        IsExpress = false,
                        Enabled = true,
                        Childs = new List<MenuEntity>
                    {
                        new MenuEntity {
                            Name = "导航设置",
                            Icon = "fa fa-chain text-primary",
                            Url = "/admin/sys/admin/menu/list",
                            OnlyCode = "sys-menus",
                            Sort = 99,
                            IsMenu = true,
                            IsExpress = false,
                            Enabled = true,
                            Childs = new List<MenuEntity>{
                                new MenuEntity {Name = "添加",Url = "#",OnlyCode = "sys-menus-add",Sort = 9,IsMenu = false,IsExpress = false,Enabled = true},
                                new MenuEntity {Name = "修改",Url = "#",OnlyCode = "sys-menus-edit",Sort = 8,IsMenu = false,IsExpress = false,Enabled = true},
                                new MenuEntity {Name = "删除",Url = "#",OnlyCode = "sys-menus-delete",Sort = 7,IsMenu = false,IsExpress = false,Enabled = true},
                                new MenuEntity {Name = "列表",Url = "#",OnlyCode = "sys-menus-list",Sort = 6,IsMenu = false,IsExpress = false,Enabled = true}
                            }
                        },
                        new MenuEntity {
                            Name = "角色管理",
                            Icon = "icon-lock text-danger",
                            Url = "/admin/sys/admin/role/list",
                            OnlyCode = "sys-role",
                            Sort = 99,
                            IsMenu = true,
                            IsExpress = false,
                            Enabled = true,
                            Childs = new List<MenuEntity>{
                                new MenuEntity {Name = "添加",Url = "#",OnlyCode = "sys-role-add",Sort = 9,IsMenu = false,IsExpress = false,Enabled = true},
                                new MenuEntity {Name = "修改",Url = "#",OnlyCode = "sys-role-edit",Sort = 8,IsMenu = false,IsExpress = false,Enabled = true},
                                new MenuEntity {Name = "删除",Url = "#",OnlyCode = "sys-role-delete",Sort = 7,IsMenu = false,IsExpress = false,Enabled = true},
                                new MenuEntity {Name = "列表",Url = "#",OnlyCode = "sys-role-list",Sort = 6,IsMenu = false,IsExpress = false,Enabled = true}
                            }
                        },
                        new MenuEntity {
                            Name = "帐号管理",
                            Icon = "icon-users text-success",
                            Url = "/admin/sys/admin/user/list",
                            OnlyCode = "sys-admin",
                            Sort = 99,
                            IsMenu = true,
                            IsExpress = false,
                            Enabled = true,
                            Childs = new List<MenuEntity>{
                                new MenuEntity {Name = "添加",Url = "#",OnlyCode = "sys-admin-add",Sort = 9,IsMenu = false,IsExpress = false,Enabled = true},
                                new MenuEntity {Name = "修改",Url = "#",OnlyCode = "sys-admin-edit",Sort = 8,IsMenu = false,IsExpress = false,Enabled = true},
                                new MenuEntity {Name = "删除",Url = "#",OnlyCode = "sys-admin-delete",Sort = 7,IsMenu = false,IsExpress = false,Enabled = true},
                                new MenuEntity {Name = "列表",Url = "#",OnlyCode = "sys-admin-list",Sort = 6,IsMenu = false,IsExpress = false,Enabled = true},
                                new MenuEntity {Name = "禁止登录",Url = "#",OnlyCode = "sys-admin-login",Sort = 5,IsMenu = false,IsExpress = false,Enabled = true},
                                new MenuEntity {Name = "授权",Url = "#",OnlyCode = "sys-admin-accredit",Sort = 4,IsMenu = false,IsExpress = false,Enabled = true}
                            }
                        },
                        new MenuEntity {
                            Name = "登录记录",
                            Icon = "fa fa-eye text-dark",
                            Url = "/admin/sys/admin/record",
                            OnlyCode = "sys-loginrecord",
                            Sort = 99,
                            IsMenu = true,
                            IsExpress = false,
                            Enabled = true
                        },
                        new MenuEntity {
                            Name = "公告管理",
                            Icon = "fa fa-bullhorn text-success",
                            Url = "/admin/sys/admin/notices/list",
                            OnlyCode = "sys-notice",
                            Sort = 99,
                            IsMenu = true,
                            IsExpress = false,
                            Enabled = true,
                            Childs = new List<MenuEntity>{
                                new MenuEntity {Name = "添加",Url = "#",OnlyCode = "sys-notice-add",Sort = 9,IsMenu = false,IsExpress = false,Enabled = true},
                                new MenuEntity {Name = "修改",Url = "#",OnlyCode = "sys-notice-edit",Sort = 8,IsMenu = false,IsExpress = false,Enabled = true},
                                new MenuEntity {Name = "删除",Url = "#",OnlyCode = "sys-notice-delete",Sort = 7,IsMenu = false,IsExpress = false,Enabled = true},
                                new MenuEntity {Name = "列表",Url = "#",OnlyCode = "sys-notice-list",Sort = 6,IsMenu = false,IsExpress = false,Enabled = true}
                            }
                        }
                    }
                    });
                    menus.Add(new MenuEntity
                    {
                        Name = "内容管理",
                        Icon = "fa fa-book text-success",
                        Url = "#",
                        OnlyCode = "content",
                        Sort = 98,
                        IsMenu = true,
                        IsExpress = false,
                        Enabled = true,
                        Childs = new List<MenuEntity>
                    {
                        new MenuEntity {
                            Name = "文章管理",
                            Icon = "fa fa-folder-open-o text-warning",
                            Url = "/admin/articles/list",
                            OnlyCode = "content-article",
                            Sort = 99,
                            IsMenu = true,
                            IsExpress = false,
                            Enabled = true,
                            Childs = new List<MenuEntity>{
                                new MenuEntity {Name = "添加",Url = "#",OnlyCode = "content-article-add",Sort = 9,IsMenu = false,IsExpress = false,Enabled = true},
                                new MenuEntity {Name = "修改",Url = "#",OnlyCode = "content-article-edit",Sort = 8,IsMenu = false,IsExpress = false,Enabled = true},
                                new MenuEntity {Name = "删除",Url = "#",OnlyCode = "content-article-delete",Sort = 7,IsMenu = false,IsExpress = false,Enabled = true},
                                new MenuEntity {Name = "列表",Url = "#",OnlyCode = "content-article-list",Sort = 6,IsMenu = false,IsExpress = false,Enabled = true}
                            }
                        }
                    }
                    });

                    Action<MenuEntity> recursionMenu = null;
                    recursionMenu = menu =>
                    {
                        if (menu.Childs == null) return;

                        foreach (var child in menu.Childs)
                        {
                            child.ParentId = menu.Id;

                            repo.Insert(child);

                            recursionMenu(child);
                        }
                    };

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
                        Enabled = true,
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
                        Name = "管理员",
                        UserName = "admin",
                        Mobile = "13500000000",
                        Password = Encrypt.Md5By32("123456"),
                        Company = "后台超级管理员",
                        Enabled = true,
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
