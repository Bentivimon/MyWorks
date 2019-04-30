using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using GraduateWork.Server.Data;
using GraduateWork.Server.Data.Entities;
using GraduateWork.Server.Models.CustomExceptions;
using GraduateWork.Server.Models.Request;
using GraduateWork.Server.Models.Response;
using GraduateWork.Server.Services.Abstractions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace GraduateWork.Server.Services.Implementations
{
    /// <inheritdoc/>
    public class UniversityService : IUniversityService
    {
        private readonly IServiceProvider _serviceProvider;

        public UniversityService(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        /// <inheritdoc/>
        public async Task AddUniversityAsync(UniversityRequest request, CancellationToken cancellationToken)
        {
            using (var scope = _serviceProvider.CreateScope())
            using (var context = scope.ServiceProvider.GetRequiredService<DatabaseContext>())
            {   
                await context.AddAsync(new UniversityEntity(request), cancellationToken).ConfigureAwait(false);
                await context.SaveChangesAsync(cancellationToken).ConfigureAwait(false);
            }
        }

        /// <inheritdoc/>
        public async Task<List<UniversityDto>> GetUniversitiesAsync(int skip, int take, CancellationToken cancellationToken)
        {
            using (var scope = _serviceProvider.CreateScope())
            using (var context = scope.ServiceProvider.GetRequiredService<DatabaseContext>())
            {
                var listOfUniversityModels = await context.Universities
                    .AsNoTracking()
                    .TakeLast(take + skip)
                    .Take(take)
                    .Select(x => x.ToDto())
                    .ToListAsync(cancellationToken).ConfigureAwait(false);

                return listOfUniversityModels;
            }
        }

        /// <inheritdoc/>
        public async Task<List<UniversityDto>> GetUniversitiesByNameAsync(int skip, int take, string name, CancellationToken cancellationToken)
        {
            using (var scope = _serviceProvider.CreateScope())
            using (var context = scope.ServiceProvider.GetRequiredService<DatabaseContext>())
            {
                var listOfUniversityModels = await context.Universities
                    .AsNoTracking()
                    .Where(x => x.FullName.ToLower(CultureInfo.InvariantCulture).Contains(name.ToLower(CultureInfo.InvariantCulture), StringComparison.InvariantCulture))
                    .TakeLast(take + skip)
                    .Take(take)
                    .Select(x => x.ToDto())
                    .ToListAsync(cancellationToken).ConfigureAwait(false);

                return listOfUniversityModels;
            }
        }

        /// <inheritdoc/>
        public async Task<UniversityDto> UpdateUniversitiesAsync(UniversityRequest request, CancellationToken cancellationToken)
        {
            using (var scope = _serviceProvider.CreateScope())
            using (var context = scope.ServiceProvider.GetRequiredService<DatabaseContext>())
            {
                var universityEntity = await context.Universities
                    .FirstOrDefaultAsync(x => x.FullName == request.FullName, cancellationToken).ConfigureAwait(false);

                if (universityEntity == null)
                    throw new NotFoundException("University not found.");

                universityEntity.LevelOfAccreditation = request.LevelOfAccreditation;

                await context.SaveChangesAsync(cancellationToken).ConfigureAwait(false);

                return universityEntity.ToDto();
            }
        }
    }
}
