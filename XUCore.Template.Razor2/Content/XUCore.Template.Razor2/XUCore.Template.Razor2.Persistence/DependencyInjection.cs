using XUCore.Template.Razor2.Core;
using XUCore.Template.Razor2.Persistence.Entities;

namespace XUCore.Template.Razor2.Persistence
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddDbContext(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddFreeSqlUnitOfWorkManager();

            var connection = configuration.GetSection<ConnectionSettings>("ConnectionSettings");

            //创建数据库
            if (connection.CreateDb)
                DbHelper.CreateDatabaseAsync(connection).Wait();

            var freeSqlBuilder = new FreeSqlBuilder()
                    .UseConnectionString(connection.Type, connection.ConnectionString)
                    .UseAutoSyncStructure(false)
                    .UseLazyLoading(false)
                    .UseNoneCommandParameter(true);

            //监听所有命令
            if (connection.MonitorCommand)
            {
                freeSqlBuilder.UseMonitorCommand(cmd => { }, (cmd, traceLog) =>
                {
                    Console.WriteLine($"{cmd.CommandText}\r\n");
                });
            }

            var fsql = freeSqlBuilder.Build();

            //监听Curd操作
            if (connection.Curd)
            {
                fsql.Aop.CurdBefore += (s, e) =>
                {
                    Console.WriteLine($"{e.Sql}\r\n");
                };
            }

            //全局过滤
            fsql.GlobalFilter.Apply<IEntitySoftDelete>("SoftDelete", a => a.IsDeleted == false);

            //配置实体

            DbHelper.ConfigEntity(fsql);

            //同步结构
            if (connection.SyncStructure)
                DbHelper.SyncStructure(fsql, dbConfig: connection);

            if (connection.SyncData)
                DbHelper.SyncData(fsql);

            //计算服务器时间（时间偏移）
            var serverTime = fsql.Select<DualEntity>().Limit(1).First(a => DateTime.UtcNow);
            var timeOffset = DateTime.UtcNow.Subtract(serverTime);
            DbHelper.TimeOffset = timeOffset;

            var user = services.BuildServiceProvider().GetRequiredService<IUserInfo>();

            //审计数据
            fsql.Aop.AuditValue += (s, e) =>
            {
                DbHelper.AuditValue(e, timeOffset, user);
            };

            services.AddSingleton(new AspectCoreFreeSql { Orm = fsql });

            //导入多数据库
            if (!connection.Dbs.IsNull())
            {
                foreach (var multiDb in connection.Dbs)
                {
                    switch (multiDb.Name)
                    {
                        case nameof(MySqlDb):
                            services.AddSingleton(new AspectCoreFreeSql<MySqlDb> { Orm = CreateMultiDbBuilder(multiDb).Build<MySqlDb>() });
                            break;
                        default:
                            break;
                    }
                }
            }

            //添加IdleBus单例（管理租户用）
            services.AddSingleton(new AspectCoreIdleBus { IBus = new IdleBus<IFreeSql>(connection.IdleTime > 0 ? TimeSpan.FromMinutes(connection.IdleTime) : TimeSpan.MaxValue) });

            return services;
        }


        /// <summary>
        /// 创建多数据库构建器
        /// </summary>
        /// <param name="multiDb"></param>
        /// <returns></returns>
        private static FreeSqlBuilder CreateMultiDbBuilder(MultiDb multiDb) =>
            new FreeSqlBuilder()
                    .UseConnectionString(multiDb.Type, multiDb.ConnectionString)
                    .UseAutoSyncStructure(false)
                    .UseLazyLoading(false)
                    .UseNoneCommandParameter(true);
    }
}