using XUCore.Template.Ddd.Domain.Core;
using XUCore.Template.Ddd.Domain.Core.Entities;
using XUCore.Template.Ddd.Domain.Core.Entities.Auth;
using XUCore.Template.Ddd.Domain.Core.Entities.User;
using XUCore.Template.Ddd.Domain.Core.Events;

namespace XUCore.Template.Ddd.Persistence
{
    public class DefaultDbRepository : DbContextRepository<IDefaultDbContext>, IDefaultDbRepository
    {
        public DefaultDbRepository(IDefaultDbContext context, IMapper mapper) : base(context, mapper) { }
    }

    public class DefaultDbContext : DBContextFactory, IDefaultDbContext
    {
        public DefaultDbContext(DbContextOptions<DefaultDbContext> options) : base(options)
        {
            SavingChanges += DefaultDbContext_SavingChanges;
        }

        private void DefaultDbContext_SavingChanges(object sender, SavingChangesEventArgs e)
        {
            var userInfo = Web.GetService<IUserInfo>();

            ChangeTracker.Entries().Where(e => e.Entity is BaseEntity).ToList().ForEach(e =>
            {
                //添加操作
                if (e.State == EntityState.Added)
                {
                    if (e.Entity is BaseEntity)
                    {
                        var entity = e.Entity as BaseEntity;
                        entity.CreatedAt = DateTime.Now;
                        entity.CreatedAtUserId = userInfo.Id;
                    }
                }
                //修改操作
                if (e.State == EntityState.Modified)
                {
                    if (e.Entity is BaseEntity)
                    {
                        var entity = e.Entity as BaseEntity;
                        switch (entity.Status)
                        {
                            case Status.Default:
                            case Status.Show:
                            case Status.SoldOut:
                                entity.UpdatedAt = DateTime.Now;
                                entity.UpdatedAtUserId = userInfo.Id;
                                break;
                            case Status.Trash:
                                entity.DeletedAt = DateTime.Now;
                                entity.DeletedAtUserId = userInfo.Id;
                                break;
                        }
                    }
                }
            });
        }

        public override Assembly[] Assemblies => new Assembly[] { Assembly.GetExecutingAssembly() };
        /// <summary>
        /// 事件存储模型
        /// </summary>
        public DbSet<StoredEvent> StoredEvent => Set<StoredEvent>();
        /// <summary>
        /// 用户
        /// </summary>
        public DbSet<UserEntity> User => Set<UserEntity>();
        /// <summary>
        /// 用户角色关联
        /// </summary>
        public DbSet<UserRoleEntity> UserRole => Set<UserRoleEntity>();
        /// <summary>
        /// 用户登录记录
        /// </summary>
        public DbSet<UserLoginRecordEntity> UserLoginRecord => Set<UserLoginRecordEntity>();
        /// <summary>
        /// 角色菜单关联
        /// </summary>
        public DbSet<RoleMenuEntity> RoleMenu => Set<RoleMenuEntity>();
        /// <summary>
        /// 角色
        /// </summary>
        public DbSet<RoleEntity> Role => Set<RoleEntity>();
        /// <summary>
        /// 菜单
        /// </summary>
        public DbSet<MenuEntity> Menu => Set<MenuEntity>();
    }
}