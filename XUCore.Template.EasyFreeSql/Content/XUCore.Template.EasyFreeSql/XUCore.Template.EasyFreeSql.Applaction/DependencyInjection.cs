using FluentValidation.AspNetCore;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using System.Text.Encodings.Web;
using System.Text.Unicode;
using XUCore.Ddd.Domain;
using XUCore.Ddd.Domain.Filters;
using XUCore.NetCore.AspectCore.Cache;
using XUCore.NetCore.Authorization.JwtBearer;
using XUCore.NetCore.DynamicWebApi;
using XUCore.NetCore.EasyQuartz;
using XUCore.NetCore.Extensions;
using XUCore.NetCore.Filters;
using XUCore.NetCore.Oss;
using XUCore.NetCore.Swagger;
using XUCore.Template.EasyFreeSql.Applaction.Events;
using XUCore.Template.EasyFreeSql.Applaction.Filters;
using XUCore.Template.EasyFreeSql.Core;

namespace XUCore.Template.EasyFreeSql.Applaction
{
    public static class DependencyInjection
    {
        const string policyName = "CorsPolicy";

        public static IServiceCollection AddApplication(this IServiceCollection services, IConfiguration configuration, string serviceMode = "api")
        {
            services.AddSingleton(HtmlEncoder.Create(UnicodeRanges.All));

            services.AddAutoMapper(typeof(MappingProfile));

            services.AddMediatR(typeof(UserCreateEvent));

            services.AddScanLifetime();

            // 注册redis插件
            //services.AddRedisService().AddJsonRedisSerializer();

            //// 注册缓存拦截器（Redis分布式缓存）
            //services.AddCacheService<RedisCacheService>((option) =>
            //{
            //    option.RedisRead = "cache-read";
            //    option.RedisWrite = "cache-write";
            //});

            // 注册缓存拦截器（内存缓存）
            services.AddCacheService<MemoryCacheService>();
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
                services.AddJwt<JwtHandler>(enableGlobalAuthorize: true);//enableGlobalAuthorize: true

                services
                    .AddControllers(opts =>
                    {
                        opts.Filters.Add<ApiExceptionFilter>();
                        opts.Filters.Add<CommandValidationActionFilter>();
                        opts.Filters.Add<ApiElapsedTimeActionFilter>();
                    })
                    //如果需要messagepack格式的输出  在接口处可以增加标签 ： [MessagePackResponseContentType]
                    //.AddMessagePackFormatters(options =>
                    //{
                    //    options.JsonSerializerSettings.DateTimeZoneHandling = DateTimeZoneHandling.Local;
                    //    options.JsonSerializerSettings.ContractResolver = new LimitPropsContractResolver();

                    //    //默认设置MessageagePack的日期序列化格式为时间戳，对外输出一致为时间戳的日期，不需要我们自己去序列化，自动操作。
                    //    //C#实体内仍旧保持DateTime。跨语言MessageagePack没有DateTime类型。
                    //    options.FormatterResolver = MessagePackSerializerResolver.UnixDateTimeFormatter;
                    //    options.Options = MessagePackSerializerResolver.UnixDateTimeOptions;
                    //})
                    .AddFluentValidation(opt =>
                    {
                        opt.ValidatorOptions.CascadeMode = FluentValidation.CascadeMode.Stop;
                        opt.DisableDataAnnotationsValidation = false;
                    });

                // 注册动态API
                services.AddDynamicWebApi();

                var env = services.BuildServiceProvider().GetService<IWebHostEnvironment>();

                services.AddMiniSwagger(swaggerGenAction: opt =>
                {
                    opt.SwaggerDoc(ApiGroup.Admin, new OpenApiInfo
                    {
                        Version = ApiGroup.Admin,
                        Title = $"用户后台API - {env.EnvironmentName}",
                        Description = "用户后台API"
                    });

                    opt.AddJwtBearerDoc();

                    opt.AddDescriptions(typeof(DependencyInjection),
                        "XUCore.Template.EasyFreeSql.Applaction.xml",
                        "XUCore.Template.EasyFreeSql.Persistence.xml",
                        "XUCore.Template.EasyFreeSql.Core.xml");

                    // TODO:一定要返回true！true 分组无效 注释掉 必须有分组才能出现api
                    //options.DocInclusionPredicate((docName, description) => true);
                });
            }

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
                opt.SwaggerEndpoint($"/swagger/{ApiGroup.Admin}/swagger.json", $"用户后台 API");
            });

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

            return app;
        }
    }
}
