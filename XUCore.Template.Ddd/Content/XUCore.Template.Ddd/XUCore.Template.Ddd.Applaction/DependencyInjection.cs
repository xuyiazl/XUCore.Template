namespace XUCore.Template.Ddd.Applaction
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddApplication(this IServiceCollection services, IWebHostEnvironment environment, string project = "api")
        {
            return services;
        }

        public static IApplicationBuilder UseApplication(this IApplicationBuilder app, IWebHostEnvironment environment, string project = "api")
        {
            return app;
        }
    }
}
