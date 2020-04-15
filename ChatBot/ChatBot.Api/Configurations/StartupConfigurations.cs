using ChatBot.Data;
using ChatBot.Data.DataStores;
using ChatBot.Logic.RestClients;
using ChatBot.Logic.Services;
using ChatBot.Models.Options;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Swashbuckle.AspNetCore.Swagger;

namespace ChatBot.Api
{
    public static class StartupConfigurations
    {
        public static void RegisterDatabase(IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseNpgsql(configuration.GetConnectionString("Default")));

            EnsureDbCreated(services);
        }

        public static void AddSwagger(IServiceCollection services)
        {
            services.AddSwaggerGen(options =>
            {
                options.SwaggerDoc("v1", new Info { Title = "Identity Server Api", Version = "v1" });

                options.AddSecurityDefinition("Bearer", new ApiKeyScheme
                {
                    Name = "Authorization",
                    Description = "JWT Authorization header using the Bearer scheme. Example: \"Authorization: Bearer {token}\"",
                    In = "header",
                    Type = "apiKey"
                });
            });
        }

        public static void RegisterOptions(IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<ViberApiOptions>(configuration.GetSection("ViberApiOptions"));
            services.Configure<DialogflowApiOptions>(configuration.GetSection("DialogflowApiOptions"));
        }

        public static void AddCustomServices(IServiceCollection services)
        {
            services.AddTransient<ViberRestClient>();
            services.AddTransient<DialogflowRestClient>();
            services.AddTransient<DialogflowResultDataStore>();
            services.AddTransient<ViberUserDataStore>();
            services.AddTransient<ViberUserMessageDataStore>();
            services.AddTransient<IViberCallbackService, ViberCallbackService>();
        }


        public static void ConfigureCORS(IApplicationBuilder app)
        {
            app.UseCors(opt =>
            {
                opt.AllowAnyOrigin();
                opt.AllowAnyMethod();
                opt.AllowAnyHeader();
            });
        }

        public static void ConfigureSwagger(IApplicationBuilder app)
        {
            var prefix = string.Empty;
            app.UseSwagger(options => options.RouteTemplate = prefix + "/swagger/{documentName}/swagger.json");
            app.UseSwaggerUI(options =>
            {
                if (string.IsNullOrEmpty(prefix))
                {
                    options.RoutePrefix = prefix + "swagger";
                    options.SwaggerEndpoint("/swagger/v1/swagger.json", "V1");
                }
                else
                {
                    options.RoutePrefix = prefix + "/swagger";
                    options.SwaggerEndpoint($"/{prefix}/swagger/v1/swagger.json", "V1");
                }
            });
        }

        private static void EnsureDbCreated(IServiceCollection services)
        {
            using (var provider = services.BuildServiceProvider())
            {
                var context = provider.GetRequiredService<ApplicationDbContext>();

                context.Database.EnsureCreated();
            }
        }
    }
}
