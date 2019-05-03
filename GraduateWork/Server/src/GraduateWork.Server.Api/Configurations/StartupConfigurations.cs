using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using GraduateWork.Server.Api.Filters;
using GraduateWork.Server.Data;
using GraduateWork.Server.Models;
using GraduateWork.Server.Models.Configurations;
using GraduateWork.Server.Services;
using GraduateWork.Server.Services.Abstractions;
using GraduateWork.Server.Services.Implementations;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using Swashbuckle.AspNetCore.Swagger;
using ICryptoProvider = GraduateWork.Server.Services.Abstractions.ICryptoProvider;
using SwaggerOptions = GraduateWork.Server.Api.Options.SwaggerOptions;

namespace GraduateWork.Server.Api.Configurations
{
    /// <summary>
    /// Class witch contains method for configure our application.
    /// </summary>
    public static class StartupConfigurations
    {
        /// <summary>
        /// Method for register swagger.
        /// </summary>
        /// <param name="services"><see cref="IServiceCollection"/> instance.</param>
        /// <param name="configuration"><see cref="IConfiguration"/> instance.</param>
        public static void RegisterSwagger(IServiceCollection services, IConfiguration configuration)
        {
            var swaggerOptions = SwaggerOptions.Read(configuration);
            if (swaggerOptions?.Enabled != true)
                return;

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc(swaggerOptions.Version, new Info
                {
                    Title = "REST API",
                    Version = swaggerOptions.Version
                });

                c.AddSecurityDefinition("Bearer", new ApiKeyScheme()
                {
                    Name = "Authorization",
                    In = "header",
                    Type = "apiKey",
                });

                c.AddSecurityRequirement(new Dictionary<string, IEnumerable<string>>
                {
                    {"Bearer", Array.Empty<string>()}
                });


                if (!string.IsNullOrWhiteSpace(swaggerOptions.XmlCommentsFileName))
                {
                    var basePath = Path.GetDirectoryName(Assembly.GetEntryAssembly().Location);
                    var xmlPath = Path.Combine(basePath, swaggerOptions.XmlCommentsFileName);

                    if (File.Exists(xmlPath))
                        c.IncludeXmlComments(xmlPath);
                }

                c.DescribeAllEnumsAsStrings();
            });
        }

        /// <summary>
        /// Method for register database context in DI.
        /// </summary>
        /// <param name="services"><see cref="IServiceCollection"/> instance.</param>
        /// <param name="configuration"><see cref="IConfiguration"/> instance.</param>
        public static void RegisterDatabaseContext(IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<DatabaseContext>(
                options => options.UseNpgsql(configuration.GetConnectionString(Consts.DefaultConnection)),
                ServiceLifetime.Transient, ServiceLifetime.Transient);
        }

        /// <summary>
        /// Method for add authentication.
        /// </summary>
        /// <param name="services"><see cref="IServiceCollection"/> instance.</param>
        public static void AddAuthentication(IServiceCollection services)
        {
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
        }

        /// <summary>
        /// Method for register custom service.
        /// </summary>
        /// <param name="services"><see cref="IServiceCollection"/> instance.</param>
        public static void RegisterCustomService(IServiceCollection services)
        {
            services.AddTransient<IAccountService, AccountService>();
            services.AddTransient<IUniversityService, UniversityService>();
            services.AddTransient<ISpecialityService, SpecialityService>();
            services.AddTransient<ICryptoProvider, CryptoProvider>();
            services.AddTransient<IEntrantService, EntrantService>();
            services.AddTransient<IJwtTokenService, JwtTokenService>();
            services.AddTransient<IRegionService, RegionService>();
            services.AddTransient<DatabaseSeeder>();
        }

        /// <summary>
        /// Method for configure JSON response, MVC with exception filter and CORS.
        /// </summary>
        /// <param name="services"><see cref="IServiceCollection"/> instance.</param>
        public static void ConfigureApplication(IServiceCollection services)
        {
            services.AddMvc(options =>
            {
                options.Filters.Add(typeof(MvcGlobalExceptionFilter));
            })
                .SetCompatibilityVersion(CompatibilityVersion.Version_2_2)
                .AddJsonOptions(options =>
            {
                options.SerializerSettings.ContractResolver = new Newtonsoft.Json.Serialization.CamelCasePropertyNamesContractResolver();
                options.SerializerSettings.NullValueHandling = NullValueHandling.Ignore;
                options.SerializerSettings.DateTimeZoneHandling = DateTimeZoneHandling.Utc;
            });
            services.AddCors(o =>
                o.AddPolicy(Consts.CorsPolicy, builder => builder.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader()));

            services.AddApiVersioning(options =>
            {
                options.ReportApiVersions = true;
                options.AssumeDefaultVersionWhenUnspecified = true;
                options.DefaultApiVersion = new ApiVersion(1, 0);
            });
        }
    }
}
