using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using GraduateWork.Server.Data;
using GraduateWork.Server.Models.Response;
using GraduateWork.Server.Services.Abstractions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace GraduateWork.Server.Services.Implementations
{
    /// <inheritdoc/>
    public class RegionService : IRegionService
    {
        private readonly IServiceProvider _serviceProvider;

        public RegionService(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        /// <inheritdoc/>
        public async Task<List<RegionDto>> GetAllRegionsAsync(CancellationToken cancellationToken)
        {
            using (var scope = _serviceProvider.CreateScope())
            using(var context = scope.ServiceProvider.GetRequiredService<DatabaseContext>())
            {
                var entities = await context.Regions.ToListAsync(cancellationToken).ConfigureAwait(false);

                return entities.Select(x => x.ToDto()).ToList();
            }
        }
    }
}
