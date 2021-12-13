namespace XUCore.Template.Razor2.Core
{
    public interface IMapFrom<T>
    {
        void Mapping(Profile profile) => profile.CreateMap(typeof(T), GetType());
    }

    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            Reflection.GetCurrentProjectAssemblies("XUCore.Template.Razor2")
                .ForEach(a => ApplyMappingsFromAssembly(a));
        }

        private void ApplyMappingsFromAssembly(Assembly assembly)
        {
            var types = assembly.GetTypes(type => type.IsAbstract == false && type.GetInterfaces().Any(i => i.IsParticularGeneric(typeof(IMapFrom<>))));

            foreach (var type in types)
            {
                var instance = Activator.CreateInstance(type);
                var methodInfo = type.GetMethod("Mapping");
                methodInfo?.Invoke(instance, new object[] { this });
            }
        }
    }

    public abstract class DtoBase<T> : IMapFrom<T>
    {
        /// <summary>
        /// 主键Id
        /// </summary>
        public long Id { get; set; }
        /// <summary>
        /// 创建者
        /// </summary>
        public string CreatedAtUserName { get; set; }
        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime? CreatedAt { get; set; }
        /// <summary>
        /// 修改者
        /// </summary>
        public string ModifiedAtUserName { get; set; }
        /// <summary>
        /// 修改时间
        /// </summary>
        public DateTime? ModifiedAt { get; set; }

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
}
