using XUCore.Template.Ddd.Domain.Core.Mappings;
using XUCore.Template.Ddd.Domain.Notifications;
using XUCore.Template.Ddd.Infrastructure.Authorization;
using XUCore.Template.Ddd.Infrastructure.Events;
using XUCore.Template.Ddd.Infrastructure.Filters;

namespace XUCore.Template.Ddd.Infrastructure
{
    public static class DependencyInjection
    {
        const string policyName = "CorsPolicy";

        public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration, IWebHostEnvironment environment, string project = "api")
        {
            services.AddSingleton(HtmlEncoder.Create(UnicodeRanges.All));

            services.AddHttpContextAccessor();

            services.AddAutoMapper(typeof(MappingProfile));

            services.AddMediatR(typeof(DomainNotificationHandler));

            services.AddRequestBehaviour(options =>
            {
                options.Logger = true;
                options.Performance = true;
                options.Validation = true;
            });

            // 命令总线Domain Bus (Mediator)
            services.AddMediatorBus<MediatorMemoryStoreHandler>();

            // 注入 基础设施层 - 事件溯源
            services.AddEventStore<SqlEventStoreService>();

            services.AddScanLifetime("XUCore.Template.Ddd");

            // 注入redis插件
            //services.AddRedisService().AddJsonRedisSerializer();

            // 注入缓存拦截器（内存缓存）
            services.AddCacheInterceptor(opt =>
            {
                opt.CacheMode = CacheMode.Memory;
                opt.RedisRead = "cache-read";
                opt.RedisWrite = "cache-write";
            });

            // 注入内存缓存
            services.AddCacheManager();

            // 注册jwt
            services.AddJwt<JwtHandler>(enableGlobalAuthorize: true);//enableGlobalAuthorize: true

            IMvcBuilder mvcBuilder;

            if (project.Equals("api"))
            {
                mvcBuilder = services.AddControllers(opts =>
                {
                    opts.Filters.Add<ApiExceptionFilter>();
                    opts.Filters.Add<ApiElapsedTimeActionFilter>();
                });

                services.AddDynamicWebApi(opt =>
                {
                    opt.IsAutoSortAction = false;
                });

                //添加跨域配置，加载进来，启用的话需要使用Configure
                services.AddCors(options =>
                    options.AddPolicy(policyName, builder =>
                    {
                        builder.AllowAnyHeader().AllowAnyOrigin().AllowAnyMethod();
                    })
                );

                services.AddSwagger(environment);
            }
            else
            {
                //services.AddRazorPages()
                mvcBuilder = services.AddControllersWithViews(opts =>
                {
                    opts.Filters.Add<ApiExceptionFilter>();
                    opts.Filters.Add<ApiElapsedTimeActionFilter>();
                });
            }

            mvcBuilder
                //如果需要messagepack格式的输出（默认大写，于前端来决定大小写输出）  在接口处可以增加标签来限定入口格式 ： [MessagePackResponseContentType]
                //客户端接入指南：
                //https://github.com/xuyiazl/XUCore.NetCore/tree/net5/src/XUCore.NetCore/MessagePack#heavy_check_mark-%E5%AE%A2%E6%88%B7%E7%AB%AF%E6%8E%A5%E5%85%A5
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
                    opt.RegisterValidatorsFromAssemblyContaining(typeof(DomainNotificationHandler));
                });

            // 注册上传服务
            services.AddUploadService();

            // 注册OSS上传服务（阿里Oss）
            services.AddOssClient(
                    (
                        "images", new OssOptions
                        {
                            AccessKey = "xxx",
                            AccessKeySecret = "xxx",
                            BluckName = "xxx",
                            EndPoint = "oss-cn-hangzhou.aliyuncs.com",
                            Domain = "https://img.xxx.com"
                        }
                    )
                );

            return services;
        }

        public static IApplicationBuilder UseInfrastructure(this IApplicationBuilder app, IWebHostEnvironment env, string project = "api")
        {
            if (project == "api")
            {
                if (env.IsDevelopment())
                {
                    app.UseDeveloperExceptionPage();
                }

                //支持跨域
                app.UseCors(policyName);
            }
            else
            {
                if (env.IsDevelopment())
                {
                    app.UseDeveloperExceptionPage();
                }
                else
                {
                    app.UseExceptionHandler("/Home/Error");
                }
            }

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseStaticHttpContext();
            app.UseStaticFiles();

            if (project == "api")
            {
                app.UseSwagger(env);

                app.UseEndpoints(endpoints =>
                {
                    endpoints.MapControllers();
                });
            }
            else
            {
                app.UseEndpoints(endpoints =>
                {
                    endpoints.MapControllerRoute(
                        name: "default",
                        pattern: "{controller=Home}/{action=Index}/{id?}");
                });
            }

            return app;
        }
    }
}
