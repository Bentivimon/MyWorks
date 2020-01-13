using ChatBot.Data.Options;
using ChatBot.Logic.RestClients;
using ChatBot.Logic.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json.Serialization;
using Swashbuckle.AspNetCore.Swagger;

namespace ChatBot.Api
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc()
                .SetCompatibilityVersion(CompatibilityVersion.Version_2_2)
                .AddJsonOptions(x =>
                {
                    x.SerializerSettings.ContractResolver = new DefaultContractResolver
                    {
                        NamingStrategy = new SnakeCaseNamingStrategy()
                    };
                });


            services.Configure<ViberApiOptions>(Configuration.GetSection("ViberApiOptions"));

            services.AddTransient<ViberRestClient>();

            services.AddTransient<IViberCallbackService, ViberCallbackService>();

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

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseMvc();

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
    }
}
