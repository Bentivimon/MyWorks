using Microsoft.AspNetCore.Builder;
using GraduateWork.Server.Data;
using GraduateWork.Server.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace GraduateWork.Server.Api.Configurations
{
    /// <summary>
    /// Extensions for <see cref="IApplicationBuilder"/>.
    /// </summary>
    public static class ApplicationBuilderExtensions
    {
        /// <summary>
        /// Ensure context ready.
        /// </summary>
        /// <param name="builder"><see cref="IApplicationBuilder"/> instance.</param>
        public static void EnsureContext(this IApplicationBuilder builder)
        {
            using (var scope = builder.ApplicationServices.CreateScope())
            {
                var context = scope.ServiceProvider.GetService<DatabaseContext>();
                if (context == null)
                    return;

                using (context)
                {
                    context.Database.MigrateAsync().GetAwaiter().GetResult();
                }
                //TODO Check this.
                var seeder = scope.ServiceProvider.GetService<DatabaseSeeder>();
                if (seeder != null)
                {
                    var services = scope.ServiceProvider;
                    seeder.SeedAsync(services).GetAwaiter().GetResult();
                }
            }
        }
    }
}
