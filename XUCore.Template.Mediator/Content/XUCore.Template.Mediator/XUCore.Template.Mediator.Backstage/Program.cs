
var builder = Host.CreateDefaultBuilder(args);

builder.ConfigureServices((HostBuilderContext hostContext, IServiceCollection services) =>
{
    services.AddLogging();

    services.AddApplication(hostContext.Configuration, hostContext.HostingEnvironment, "backstage");

    services.AddDbContext(hostContext.Configuration);

    services.AddHostedService<WorkerService1>();

    services.AddHostedService<WorkerService2>();
});

builder.UseInterceptorHostBuilder();

await builder.Build().RunAsync();

