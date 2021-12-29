namespace XUCore.Template.Mediator2.Applaction;

/// <summary>
/// 注册服务
/// </summary>
public static class DependencyInjection
{
    const string policyName = "CorsPolicy";

    public static IServiceCollection AddApplication(this IServiceCollection services, IConfiguration configuration, IHostEnvironment environment, string serviceMode = "api")
    {
        // AutoMapper
        services.AddAutoMapper(typeof(IMapFrom<>));

        // Mediator
        services.AddMediatR(typeof(IMapFrom<>));

        // Mediator 请求设置
        services.AddRequestBehaviour(options =>
        {
            options.Logger = true;
            options.Performance = true;
            options.Validation = true;
        });

        // 扫描
        services.AddScanLifetime("XUCore.Template.Mediator2");

        // 注册redis插件
        //services.AddRedisService().AddJsonRedisSerializer();

        // 注册缓存拦截器（内存缓存）
        services.AddCacheInterceptor(opt =>
        {
            opt.CacheMode = CacheMode.Memory;
            opt.RedisRead = "cache-read";
            opt.RedisWrite = "cache-write";
        });

        // 注册内存缓存
        services.AddCacheManager();

        // 注册Quartz服务
        services.AddEasyQuartzService();

        if (serviceMode == "api")
        {
            //添加跨域配置，加载进来，启用的话需要使用Configure
            services.AddCors(options =>
                options.AddPolicy(policyName, builder =>
                {
                    builder.AllowAnyHeader().AllowAnyOrigin().AllowAnyMethod();
                })
            );

            // 注册jwt
            services.AddJwt<JwtHandler>(enableGlobalAuthorize: true);

            services
                .AddControllers(opts =>
                {
                    opts.Filters.Add<ApiExceptionFilter>();
                    opts.Filters.Add<ApiElapsedTimeActionFilter>();
                })
                //如果需要messagepack格式的输出（默认大写，于前端来决定大小写输出）  在接口处可以增加标签来限定入口格式 ： [MessagePackResponseContentType]
                //客户端接入指南：
                //https://github.com/xuyiazl/XUCore.NetCore/tree/net6/src/XUCore.NetCore/MessagePack#heavy_check_mark-%E5%AE%A2%E6%88%B7%E7%AB%AF%E6%8E%A5%E5%85%A5
                .AddFormatters(options =>
                {
                    options.JsonSerializerSettings.DateTimeZoneHandling = DateTimeZoneHandling.Local;
                    options.JsonSerializerSettings.ContractResolver = new LimitPropsContractResolver();

                        //默认设置MessageagePack的日期序列化格式为时间戳，对外输出一致为时间戳的日期，不需要我们自己去序列化，自动操作。
                        //C#实体内仍旧保持DateTime。跨语言MessageagePack没有DateTime类型。
                        options.FormatterResolver = MessagePackSerializerResolver.UnixDateTimeFormatter;
                    options.Options = MessagePackSerializerResolver.UnixDateTimeOptions;
                })
                .AddFluentValidation(opt =>
                {
                    opt.ValidatorOptions.CascadeMode = FluentValidation.CascadeMode.Stop;
                    opt.DisableDataAnnotationsValidation = false;
                    opt.RegisterValidatorsFromAssemblyContaining(typeof(IMapFrom<>));
                });

            // 注册动态API
            services.AddDynamicWebApi();

            // 注册上传服务
            services.AddUploadService();

            //services.AddEndpointsApiExplorer();
            services.AddMiniSwagger(swaggerGenAction: opt =>
            {
                opt.SwaggerDoc(ApiGroup.Admin, new OpenApiInfo
                {
                    Version = ApiGroup.Admin,
                    Title = $"后台API - {environment.EnvironmentName}",
                    Description = "后台API"
                });

                opt.AddJwtBearerDoc();

                opt.AddDescriptions(typeof(DependencyInjection),
                    "XUCore.Template.Mediator2.Applaction.xml",
                    "XUCore.Template.Mediator2.Persistence.xml",
                    "XUCore.Template.Mediator2.Core.xml");

                    // TODO:一定要返回true！true 分组无效 注释掉 必须有分组才能出现api
                    //options.DocInclusionPredicate((docName, description) => true);
                });
        }

        return services;
    }

    public static IApplicationBuilder UseApplication(this IApplicationBuilder app, IWebHostEnvironment env)
    {
        if (env.IsDevelopment())
        {
            app.UseDeveloperExceptionPage();
        }

        app.UseStaticFiles();

        //支持跨域
        app.UseCors(policyName);

        app.UseRouting();
        app.UseRealIp();

        app.UseAuthentication();
        app.UseAuthorization();

        app.UseStaticHttpContext();
        app.UseStaticFiles();

        app.UseMiniSwagger(swaggerUIAction: (opt) =>
        {
            opt.SwaggerEndpoint($"/swagger/{ApiGroup.Admin}/swagger.json", $"后台 API");
        });

        app.UseEndpoints(endpoints =>
        {
            endpoints.MapControllers();
        });

        return app;
    }
}