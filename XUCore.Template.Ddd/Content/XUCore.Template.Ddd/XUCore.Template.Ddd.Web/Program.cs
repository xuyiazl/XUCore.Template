var project = "mvc";

var builder = WebApplication.CreateBuilder(args);

builder.WebHost
    .UseRealIp(); //"X-Real-IP";

builder.Host.UseInterceptorHostBuilder();

// Add services to the container.

builder.Services.AddInfrastructure(builder.Configuration, builder.Environment, project);
builder.Services.AddPersistence(builder.Configuration);
builder.Services.AddApplication(builder.Environment, project);

var app = builder.Build();

// Configure the HTTP request pipeline.

app.UseInfrastructure(builder.Environment, project);
app.UsePersistence();
app.UseApplication(builder.Environment, project);

app.Run();