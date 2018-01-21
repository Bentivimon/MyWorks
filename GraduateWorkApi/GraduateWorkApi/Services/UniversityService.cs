using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Extensions.DependencyInjection;
using System.Threading.Tasks;
using EntityModels.Entitys;
using GraduateWorkApi.Abstractions;
using GraduateWorkApi.Context;
using Microsoft.EntityFrameworkCore;
using Models.DTOModels.UniverisyModels;
using Models.RequestModels.UniversityModels;

namespace GraduateWorkApi.Services
{
    public class UniversityService : IUniversityService
    {
        private readonly IServiceProvider _serviceProvider;

        public UniversityService(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public async Task AddUniversityTask(UnivesityModelRequest request)
        {
            using (var context = _serviceProvider.GetService<DatabaseContext>())
            {
                var model = new UniversityEntity()
                {
                    FullName = request.FullName,
                    LevelOfAccreditation = request.LevelOfAccreditation
                };

                await context.AddAsync(model);
                await context.SaveChangesAsync();
            }
        }

        public async Task<UniversityDto> EditUniversityTask(UnivesityModelRequest request)
        {
            using (var context = _serviceProvider.GetService<DatabaseContext>())
            {
                var model = await context.Universitys
                    .FirstOrDefaultAsync(x => x.FullName == request.FullName);
                model.LevelOfAccreditation = request.LevelOfAccreditation;

                await context.SaveChangesAsync();
            }

            return new UniversityDto(request);
        }

        public async Task<List<UniversityDto>> GetUniversitysByNameTask(int skip, int take, string name)
        {
            var listOfUniversityModels = new List<UniversityDto>();

            using (var context = _serviceProvider.GetService<DatabaseContext>())
            {
                listOfUniversityModels = await context.Universitys
                    .AsNoTracking()
                    .Where(x=> x.FullName.ToLower().Contains(name.ToLower()))
                    .TakeLast(take + skip)
                    .Take(take)
                    .Select(x => new UniversityDto(x))
                    .ToListAsync();
            }

            return listOfUniversityModels;
        }

        public async Task<List<UniversityDto>> GetUniversitysTask(int skip, int take)
        {
            var listOfUniversityModels = new List<UniversityDto>();

            using (var context = _serviceProvider.GetService<DatabaseContext>())
            {
                listOfUniversityModels = await context.Universitys
                    .AsNoTracking()
                    .TakeLast(take + skip)
                    .Take(take)
                    .Select(x => new UniversityDto(x))
                    .ToListAsync();
            }

            return listOfUniversityModels;
        }
    }
}
