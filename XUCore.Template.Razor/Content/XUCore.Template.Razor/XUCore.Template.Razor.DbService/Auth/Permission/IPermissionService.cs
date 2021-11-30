namespace XUCore.Template.Razor.DbService.Auth.Permission
{
    public interface IPermissionService : IScoped
    {
        Task<bool> ExistsAsync(long userId, string onlyCode, CancellationToken cancellationToken);
        Task<IList<PermissionMenuDto>> GetMenuExpressAsync(long userId, CancellationToken cancellationToken);
        Task<IList<PermissionMenuDto>> GetMenusAsync(long userId, CancellationToken cancellationToken);
    }
}