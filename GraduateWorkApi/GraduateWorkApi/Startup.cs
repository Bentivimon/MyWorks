using GraduateWorkApi.Abstractions;
using GraduateWorkApi.Configurations;
using GraduateWorkApi.Context;
using GraduateWorkApi.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using Swashbuckle.AspNetCore.Swagger;

namespace GraduateWorkApi
{
    public class Startup
    {
        public IConfiguration Configuration { get; }
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }
        
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<DatabaseContext>(options => options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")), ServiceLifetime.Transient);
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    options.RequireHttpsMetadata = true;
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = true,
                        ValidIssuer = JwtTokenConfiguration.Issuer,

                        ValidateAudience = true,
                        ValidAudience = JwtTokenConfiguration.Audience,
                        ValidateLifetime = true,

                        IssuerSigningKey = JwtTokenConfiguration.GetSymmetricSecurityKey(),
                        ValidateIssuerSigningKey = true,
                    };
                });

            services.AddMvc();
            services.AddCors(o => o.AddPolicy("CorsPolicy", builder => builder.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader()));
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new Info { Title = "Graduate Work API", Version = "v1" });
            });
            services.AddSwaggerGen(sg =>
            {
                {
                    sg.AddSecurityDefinition("Bearer",
                        new ApiKeyScheme()
                        {
                            In = "header",
                            Description = "Please insert JWT with Bearer into field",
                            Name = "Authorization",
                            Type = "apiKey"
                        });
                };
            });

            services.AddTransient<IAccountService, AccountService>();
            services.AddTransient<IUniversityService, UniversityService>();
        }
        
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseAuthentication();

            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "Desc");
            });

            app.UseMvc();
            app.UseCors("CorsPolicy");
        }
    }
}
