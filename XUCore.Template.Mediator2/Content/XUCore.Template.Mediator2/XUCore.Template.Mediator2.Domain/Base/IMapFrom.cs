namespace XUCore.Template.Mediator2.Domain;

public interface IMapFrom<T>
{
    void Mapping(Profile profile) => profile.CreateMap(typeof(T), GetType());
}

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        Reflection.GetCurrentProjectAssemblies("XUCore.Template.Mediator2")
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