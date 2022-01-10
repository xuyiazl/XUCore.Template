var builder = WebApplication.CreateBuilder(args);

builder.WebHost
    .UseRealIp(); //"X-Real-IP";

builder.Host.UseInterceptorHostBuilder();

// Add services to the container.

builder.Services.AddApplication(builder.Configuration);
builder.Services.AddDbContext(builder.Configuration);

var app = builder.Build();

// Configure the HTTP request pipeline.

app.UseApplication(app.Environment);

app.Run();
