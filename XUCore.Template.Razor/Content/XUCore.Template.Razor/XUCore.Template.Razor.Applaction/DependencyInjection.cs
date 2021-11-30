using Microsoft.AspNetCore.Antiforgery;
using XUCore.Template.Razor.Core;

namespace XUCore.Template.Razor.Applaction
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddApplication(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddSingleton(HtmlEncoder.Create(UnicodeRanges.All));

            services.AddHttpContextAccessor();

            services.AddAutoMapper(typeof(MappingProfile));

            services.AddScanLifetime("XUCore.Template.Razor");

            // 注册缓存拦截器（内存缓存）
            services.AddCacheInterceptor(opt =>
            {
                opt.CacheMode = CacheMode.Memory;
                opt.RedisRead = "cache-read";
                opt.RedisWrite = "cache-write";
            });

            // 注册内存缓存
            services.AddCacheManager();

            // 注册上传服务
            services.AddUploadService();

            services
                .AddRazorPages(options =>
                {
                    options.Conventions.Add(new DefaultRouteRemovalPageRouteModelConvention(string.Empty));
                    options.Conventions.AddPageRoute("/admin/login", "");
                    //忽略XSRF验证
                    //options.Conventions.ConfigureFilter(new IgnoreAntiforgeryTokenAttribute());
                })
                //全局配置Json序列化处理  3.0默认移除了NewtonsoftJson，使用微软自己的json，继续使用NewtonsoftJson
                .AddNewtonsoftJson(options =>
                {
                    //EF Core中默认为驼峰样式序列化处理key
                    //options.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();
                    //使用默认方式，不更改元数据的key的大小写
                    options.SerializerSettings.ContractResolver = new DefaultContractResolver();
                    options.SerializerSettings.DateTimeZoneHandling = DateTimeZoneHandling.Local;
                    //options.SerializerSettings.DateFormatString = "yyyy-MM-dd HH:mm:ss";
                    options.SerializerSettings.MissingMemberHandling = MissingMemberHandling.Ignore;
                    options.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
                })
                .AddFluentValidation(opt =>
                {
                    opt.ValidatorOptions.CascadeMode = CascadeMode.Stop;
                    opt.DisableDataAnnotationsValidation = false;
                });

            var appSettings = services.BindSection<AppSettings>(configuration, "AppSettings");

            GlobalStatic.RootUrl = appSettings.RootUrl;

            #region [ 注册登录Cookie ]

            //cookie写入需要注意浏览器的SameSiteMode，会随着浏览器版本不同，当前在IE和Edge无法登陆

            //void CheckSameSite(HttpContext httpContext, CookieOptions options)
            //{
            //    if (options.SameSite == SameSiteMode.None)
            //    {
            //        //var userAgent = Web.Browser;
            //        //if (userAgent.IndexOf("like Gecko") > -1)
            //        //    options.SameSite = SameSiteMode.None;
            //        //if (userAgent.IndexOf("Edge") > -1)
            //        //    options.SameSite = SameSiteMode.None;
            //        //else
            //        options.SameSite = SameSiteMode.Unspecified;
            //    }
            //}

            services.Configure<CookiePolicyOptions>(options =>
            {
                // This lambda determines whether user consent for non-essential cookies is needed for a given request.
                options.CheckConsentNeeded = context => true;
                options.MinimumSameSitePolicy = SameSiteMode.None;
                //options.OnAppendCookie = cookieContext => CheckSameSite(cookieContext.Context, cookieContext.CookieOptions);
                //options.OnDeleteCookie = cookieContext => CheckSameSite(cookieContext.Context, cookieContext.CookieOptions);
            });

            services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
                .AddCookie(CookieAuthenticationDefaults.AuthenticationScheme, options =>
                {
                    options.LoginPath = new PathString("/admin/login");
                    //options.LogoutPath = new PathString("/admin/login?handler=loginout");
                    options.AccessDeniedPath = new PathString("/admin/login");
                    //options.ExpireTimeSpan = TimeSpan.FromDays(7);
                    //options.SlidingExpiration = true;
                    //options.ClaimsIssuer = CookieAuthenticationDefaults.AuthenticationScheme;

                    options.Cookie.Path = "/";
                    options.Cookie.HttpOnly = true;
                    //options.Cookie.SameSite = SameSiteMode.None;
                    options.Cookie.Domain = appSettings.Domain;
                });

            #endregion

            //注册资源权限控制
            services
                .AddAccessControl()
                .AddControlAccessStrategy<ControlAccessStrategy>()
                .AddResourceAccessStrategy<ResourceAccessStrategy>();

            return services;
        }

        public static IApplicationBuilder UseApplication(this IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
                app.UseDeveloperExceptionPage();
            else
                app.UseExceptionHandler("/Error");

            app.UseCookiePolicy();


            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            //注册真实IP中间件
            app.UseRealIp();

            //启用静态请求上下文
            app.UseStaticHttpContext();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();

                endpoints.MapRazorPages();
            });

            return app;
        }
    }
}
