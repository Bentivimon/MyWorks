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
    ///<inheritdoc/>
    public class SpecialityService : ISpecialityService
    {
        private readonly IServiceProvider _serviceProvider;

        public SpecialityService(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }
        
        ///<inheritdoc/>
        public async Task AddSpecialityAsync(SpecialityRequest request, CancellationToken cancellationToken)
        {
            using (var scope = _serviceProvider.CreateScope())
            using (var context = scope.ServiceProvider.GetRequiredService<DatabaseContext>())
            {
                var specialityEntity = new SpecialityEntity(request);

                await context.Specialties.AddAsync(specialityEntity, cancellationToken).ConfigureAwait(false);
                await context.SaveChangesAsync(cancellationToken).ConfigureAwait(false);
            }
        }

        ///<inheritdoc/>
        public async Task<SpecialityDto> UpdateSpecialityAsync(SpecialityRequest request, CancellationToken cancellationToken)
        {
            using (var scope = _serviceProvider.CreateScope())
            using (var context = scope.ServiceProvider.GetRequiredService<DatabaseContext>())
            {
                var specialityEntity = await context.Specialties
                    .FirstOrDefaultAsync(x => x.Code == request.Code, cancellationToken).ConfigureAwait(false);

                if (specialityEntity == null)
                    throw new NotFoundException("Speciality not found.");

                specialityEntity.Update(request);
               
                await context.SaveChangesAsync(cancellationToken).ConfigureAwait(false);

                return specialityEntity.ToDto();
            }
        }

        ///<inheritdoc/>
        public async Task<List<SpecialityDto>> GetSpecialitiesAsync(int skip, int take, CancellationToken cancellationToken)
        {
            using (var scope = _serviceProvider.CreateScope())
            using (var context = scope.ServiceProvider.GetRequiredService<DatabaseContext>())
            {
                var listOfSpecialities = await context.Specialties
                    .AsNoTracking()
                    .Skip(skip)
                    .Take(take)
                    .Select(x => x.ToDto())
                    .ToListAsync(cancellationToken).ConfigureAwait(false);

                return listOfSpecialities;
            }
        }

        ///<inheritdoc/>
        public async Task<List<SpecialityDto>> GetSpecialitiesByNameAsync(int skip, int take, string name, CancellationToken cancellationToken)
        {
            using (var scope = _serviceProvider.CreateScope())
            using (var context = scope.ServiceProvider.GetRequiredService<DatabaseContext>())
            {
                var listOfSpecialities = await context.Specialties
                    .AsNoTracking()
                    .Where(x => x.Name.ToLower(CultureInfo.InvariantCulture).Contains(name.ToLower(CultureInfo.InvariantCulture), StringComparison.InvariantCulture))
                    .Skip(skip)
                    .Take(take)
                    .Select(x => x.ToDto())
                    .ToListAsync(cancellationToken).ConfigureAwait(false);

                return listOfSpecialities;
            }
        }

        ///<inheritdoc/>
        public async Task<List<SpecialityDto>> GetSpecialitiesInUniversityAsync(int skip, int take, Guid universityId, CancellationToken cancellationToken)
        {
            using (var scope = _serviceProvider.CreateScope())
            using (var context = scope.ServiceProvider.GetRequiredService<DatabaseContext>())
            {
                var listOfSpecialities = await context.Specialties
                    .AsNoTracking()
                    .Include(x => x.UniversitySpecialities)
                    .Where(x => x.UniversitySpecialities.Any(us => us.UniversityId == universityId))
                    .Skip(skip)
                    .Take(take)
                    .Select(x => x.ToDto())
                    .ToListAsync(cancellationToken).ConfigureAwait(false);

                return listOfSpecialities;
            }
        }
    }
}
