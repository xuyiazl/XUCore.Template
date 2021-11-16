
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using XUCore.NetCore.AspectCore.Cache;
using XUCore.Template.FreeSql.Applaction;
using XUCore.Template.FreeSql.Backstage;
using XUCore.Template.FreeSql.Persistence;

var builder = Host.CreateDefaultBuilder(args);

builder.ConfigureHostConfiguration(configHost =>
{
    //配置根目录
    configHost.SetBasePath(Path.GetDirectoryName(typeof(Program).Assembly.Location));
    //读取环境变量，Asp.Net core默认的环境变量是以ASPNETCORE_作为前缀的，这里也采用此前缀以保持一致
    configHost.AddEnvironmentVariables("ASPNETCORE_");
    //可以在启动host的时候之前可传入参数，暂不需要先注释掉，可根据需要开启
    //configHost.AddCommandLine(args);
});

builder.ConfigureAppConfiguration((hostContext, configApp) =>
{
    //读取应用特定环境下的配置json
    configApp.AddJsonFile($"appsettings.json", optional: true);
    configApp.AddJsonFile($"appsettings.{hostContext.HostingEnvironment.EnvironmentName}.json", optional: true);
    //读取环境变量
    configApp.AddEnvironmentVariables();
    //可以在启动host的时候之前可传入参数，暂不需要先注释掉，可根据需要开启
    //configApp.AddCommandLine(args);
});

builder.ConfigureServices((HostBuilderContext hostContext, IServiceCollection services) =>
{
    services.AddLogging();

    services.AddApplication(hostContext.Configuration, hostContext.HostingEnvironment, "backstage");

    services.AddDbContext(hostContext.Configuration);

    services.AddHostedService<MainService>();
});

builder.UseInterceptorHostBuilder();

builder.Build().Run();

