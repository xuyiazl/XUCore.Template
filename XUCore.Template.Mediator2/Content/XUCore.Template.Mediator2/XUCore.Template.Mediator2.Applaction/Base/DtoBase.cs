namespace XUCore.Template.Mediator2.Applaction;

public abstract class DtoBase<T> : IMapFrom<T>
{
    /// <summary>
    /// 主键Id
    /// </summary>
    public long Id { get; set; }
    ///// <summary>
    ///// 数据状态
    ///// </summary>
    //public Status Status { get; set; }
    ///// <summary>
    ///// 创建日期
    ///// </summary>
    //public DateTime CreatedAt { get; set; }
    ///// <summary>
    ///// 更新日期
    ///// </summary>
    //public DateTime? UpdatedAt { get; set; }
    ///// <summary>
    ///// 删除日期
    ///// </summary>
    //public DateTime? DeletedAt { get; set; }

    public virtual void Mapping(Profile profile) => profile.CreateMap(typeof(T), GetType());
}

public abstract class DtoKeyBase<T> : IMapFrom<T>
{
    /// <summary>
    /// 主键Id
    /// </summary>
    public long Id { get; set; }

    public virtual void Mapping(Profile profile) => profile.CreateMap(typeof(T), GetType());
}
