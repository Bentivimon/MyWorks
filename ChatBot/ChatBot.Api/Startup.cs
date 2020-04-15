using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json.Serialization;

namespace ChatBot.Api
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

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

            StartupConfigurations.RegisterDatabase(services, Configuration);
            StartupConfigurations.RegisterOptions(services, Configuration);
            StartupConfigurations.AddCustomServices(services);
            StartupConfigurations.AddSwagger(services);
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            app.UseMvc();

            StartupConfigurations.ConfigureCORS(app);
            StartupConfigurations.ConfigureSwagger(app);
        }
    }
}
