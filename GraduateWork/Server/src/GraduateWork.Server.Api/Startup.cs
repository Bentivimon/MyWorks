using GraduateWork.Server.Api.Configurations;
using GraduateWork.Server.Models;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace GraduateWork.Server.Api
{
    /// <summary>
    /// Startup class
    /// </summary>
    public class Startup
    {
        /// <summary>
        /// Base constructor.
        /// </summary>
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        /// <summary>
        /// <see cref="IConfiguration"/> instance.
        /// </summary>
        private  IConfiguration Configuration { get; set; }

        /// <summary>
        /// Method for configure service.
        /// </summary>
        public void ConfigureServices(IServiceCollection services)
        {
            StartupConfigurations.RegisterDatabaseContext(services, Configuration);
            StartupConfigurations.ConfigureApplication(services);
            StartupConfigurations.RegisterSwagger(services, Configuration);
            StartupConfigurations.RegisterCustomService(services);
        }

        /// <summary>
        /// Method for configure app.
        /// </summary>
        public static void Configure(IApplicationBuilder app)
        {
            app.UseAuthentication();

            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "Desc");
            });

            app.UseMvc();
            app.UseCors(Consts.CorsPolicy);

            app.EnsureContext();
        }
        
    }
}
