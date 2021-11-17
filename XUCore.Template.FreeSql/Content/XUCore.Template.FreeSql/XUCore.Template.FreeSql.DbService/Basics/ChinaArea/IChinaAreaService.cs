using XUCore.Template.FreeSql.Persistence.Entities;

namespace XUCore.Template.FreeSql.DbService.Basics.ChinaArea
{
    public interface IChinaAreaService : ICurdService<long, ChinaAreaEntity, ChinaAreaDto, ChinaAreaCreateCommand, ChinaAreaUpdateCommand, ChinaAreaQueryCommand, ChinaAreaQueryPagedCommand>, IScoped
    {
        Task<IList<ChinaAreaTreeDto>> GetListByTreeAsync(ChinaAreaQueryTreeCommand request, CancellationToken cancellationToken);
    }
}