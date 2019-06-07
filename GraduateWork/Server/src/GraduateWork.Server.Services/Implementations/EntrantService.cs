using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using GraduateWork.Server.Data;
using GraduateWork.Server.Data.Entities;
using GraduateWork.Server.Models.CustomExceptions;
using GraduateWork.Server.Models.Enums;
using GraduateWork.Server.Models.Request;
using GraduateWork.Server.Models.Response;
using GraduateWork.Server.Services.Abstractions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace GraduateWork.Server.Services.Implementations
{
    public class EntrantService : IEntrantService
    {
        private readonly IServiceProvider _serviceProvider;

        public EntrantService(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public async Task AddEntrantAsync(EntrantModel request, CancellationToken cancellationToken)
        {
            using(var scope = _serviceProvider.CreateScope())
            using (var context = scope.ServiceProvider.GetRequiredService<DatabaseContext>())
            {
                context.Entrants.Add(new EntrantEntity(request));

                await context.SaveChangesAsync(cancellationToken).ConfigureAwait(false);
            }
        }

        public async Task<EntrantDto> UpdateEntrantAsync(Guid entrantId, EntrantModel request, CancellationToken cancellationToken)
        {
            using(var scope = _serviceProvider.CreateScope())
            using (var context = scope.ServiceProvider.GetRequiredService<DatabaseContext>())
            {
                var entrantEntity = await context.Entrants
                    .FirstOrDefaultAsync(x => x.Id == entrantId, cancellationToken).ConfigureAwait(false);

                if (entrantEntity == null)
                    throw new NotFoundException("Entrant not found");

                entrantEntity.Update(request);

                await context.SaveChangesAsync(cancellationToken).ConfigureAwait(false);

                return entrantEntity.ToDto();
            }
        }

        public async Task<List<EntrantDto>> GetEntrantsAsync(int skip, int take, CancellationToken cancellationToken)
        {
            using (var scope = _serviceProvider.CreateScope())
            using (var context = scope.ServiceProvider.GetRequiredService<DatabaseContext>())
            {
                var result = await context.Entrants
                    .Skip(skip)
                    .Take(take)
                    .ToListAsync(cancellationToken).ConfigureAwait(false);

                return result.Select(x => x.ToDto()).ToList();
            }
        }

        public async Task<List<EntrantExtendDto>> GetEntrantsByNameAsync(int skip, int take, string name, CancellationToken cancellationToken)
        {
            using (var scope = _serviceProvider.CreateScope())
            using (var context = scope.ServiceProvider.GetRequiredService<DatabaseContext>())
            {
                var result = await context.Entrants.Include(x=> x.CertificateOfSecondaryEducation).Include(x=>x.CertificateOfTesting).Include(x=> x.Statements)
                    .Where(x =>
                        x.FirstName.ToLower(CultureInfo.InvariantCulture)
                            .Contains(name.ToLower(CultureInfo.InvariantCulture), StringComparison.InvariantCulture) ||
                        x.LastName.ToLower(CultureInfo.InvariantCulture)
                            .Contains(name.ToLower(CultureInfo.InvariantCulture), StringComparison.InvariantCulture))
                    .Skip(skip)
                    .Take(take)
                    .ToListAsync(cancellationToken).ConfigureAwait(false);

                return result.Select(x => x.ToExtendedDto()).ToList();
            }
        }

        public async Task<EntrantExtendDto> GetEntrantByIdAsync(Guid entrantId, CancellationToken cancellationToken)
        {
            using (var scope = _serviceProvider.CreateScope())
            using (var context = scope.ServiceProvider.GetRequiredService<DatabaseContext>())
            {
                var result = await context.Entrants.Include(x=> x.CertificateOfSecondaryEducation).Include(x=>x.CertificateOfTesting).Include(x=> x.Statements)
                    .FirstOrDefaultAsync(x => x.Id == entrantId, cancellationToken).ConfigureAwait(false);

                if (result == null)
                    throw new NotFoundException("Entrant not found.");

                return result.ToExtendedDto();
            }
        }

        public async Task<List<EntrantExtendDto>> GetEntrantBySpecialityIdAsync(Guid specialityId, CancellationToken cancellationToken)
        {
            using (var scope = _serviceProvider.CreateScope())
            using (var context = scope.ServiceProvider.GetRequiredService<DatabaseContext>())
            {
                var result = await context.Entrants
                    .Include(x => x.CertificateOfTesting)
                    .Include(x => x.CertificateOfSecondaryEducation)
                    .Include(x => x.Statements)
                    .Where(x => x.Statements.Any(y => y.SpecialityId == specialityId && y.Status == StatementStatus.Accepted))
                    .ToListAsync(cancellationToken)
                    .ConfigureAwait(false);

                if (result == null)
                    throw new NotFoundException("Entrant not found.");

                return result.Select(x => x.ToExtendedDto()).OrderByDescending(x=> x.TotalScore).ToList();
            }
        }

        public async Task TieUpEntrantAndUserAsync(Guid userId, Guid entrantId, CancellationToken cancellationToken)
        {
            using (var scope = _serviceProvider.CreateScope())
            using (var context = scope.ServiceProvider.GetRequiredService<DatabaseContext>())
            {
                var entrantEntity = await context.Entrants
                    .FirstOrDefaultAsync(x => x.Id == entrantId, cancellationToken).ConfigureAwait(false);

                if (entrantEntity == null)
                    throw new NotFoundException("Entrant not found.");

                var user = await context.Users.FirstOrDefaultAsync(x => x.Id == userId, cancellationToken)
                    .ConfigureAwait(false);

                if (user.EntrantId.HasValue)
                {
                    user.EntrantId = null;
                    await context.SaveChangesAsync(cancellationToken).ConfigureAwait(false);
                }

                user.EntrantId = entrantId;
                await context.SaveChangesAsync(cancellationToken).ConfigureAwait(false);
            }
        }
    }
}
