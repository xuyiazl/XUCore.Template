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
                    //options.Conventions.AddPageRoute("/admin/login", "");
                    //忽略XSRF验证
                    options.Conventions.ConfigureFilter(new IgnoreAntiforgeryTokenAttribute());
                })
                .AddFluentValidation(opt =>
                {
                    opt.ValidatorOptions.CascadeMode = CascadeMode.Stop;
                    opt.DisableDataAnnotationsValidation = false;
                });


            var appSettings = services.BindSection<AppSettings>(configuration, "AppSettings");

            Root.DomainUrl = appSettings.RootUrl;

            #region [ 注册登录Cookie ]

            //cookie写入需要注意浏览器的SameSiteMode，会随着浏览器版本不同，当前在IE和Edge无法登陆

            void CheckSameSite(HttpContext httpContext, CookieOptions options)
            {
                if (options.SameSite == SameSiteMode.None)
                {
                    //var userAgent = Web.Browser;
                    //if (userAgent.IndexOf("like Gecko") > -1)
                    //    options.SameSite = SameSiteMode.None;
                    //if (userAgent.IndexOf("Edge") > -1)
                    //    options.SameSite = SameSiteMode.None;
                    //else
                    options.SameSite = SameSiteMode.Unspecified;
                }
            }

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

            #region [ 注册资源权限控制 ]

            //注册资源权限控制
            services.AddAccessControl(options =>
            {
                options.UseAsDefaultPolicy = true;

                #region [ 重写API 403的输出操作，默认是浏览器403 ]

                //options.DefaultUnauthorizedOperation = (context) =>
                //{
                //    var res = new Result(StateCode.Fail, context.User.Identity.IsAuthenticated ? "403" : "401", "无权限执行此操作", null);
                //    context.Response.StatusCode = 200;
                //    context.Response.ContentType = "application/json";
                //    context.Response.WriteAsync(new
                //    {
                //        res.code,
                //        res.subCode,
                //        res.message,
                //        res.elapsedTime,
                //        res.operationTime,
                //        res.data
                //    }.ToJson(), Encoding.UTF8);
                //    return Task.CompletedTask;
                //};

                #endregion

            })
            .AddControlAccessStrategy<ControlAccessStrategy>()
            .AddResourceAccessStrategy<ResourceAccessStrategy>();

            #endregion

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
            app.UseRealIp();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseStaticHttpContext();
            app.UseStaticFiles();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapRazorPages();
            });

            return app;
        }
    }
}
