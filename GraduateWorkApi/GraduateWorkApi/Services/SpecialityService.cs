using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EntityModels.Entitys;
using GraduateWorkApi.Abstractions;
using GraduateWorkApi.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Models.DTOModels.SpecialityModels;
using Models.RequestModels.SpecialityModels;

namespace GraduateWorkApi.Services
{
    public class SpecialityService : ISpecialityService
    {
        private readonly IServiceProvider _serviceProvider;

        public SpecialityService(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public async Task AddSpecialityTask(SpecialityRequest request)
        {
            using (var context = _serviceProvider.GetService<DatabaseContext>())
            {
                var specialityEntity = new SpecialityEntity(request);

                await context.Specialtys.AddAsync(specialityEntity);
                await context.SaveChangesAsync();
            }
        }

        public async Task<SpecialityDto> EditSpecialityTask(SpecialityRequest request)
        {
            using (var context = _serviceProvider.GetService<DatabaseContext>())
            {
                var specialityEntity = await context.Specialtys
                    .FirstOrDefaultAsync(x => x.Code == request.Code);

                specialityEntity.AdditionalFactor = request.AdditionalFactor;
                specialityEntity.CountOfStatePlaces = request.CountOfStatePlaces;
                specialityEntity.Name = request.Name;

                await context.SaveChangesAsync();

                return new SpecialityDto(specialityEntity);
            }
        }

        public async Task<List<SpecialityDto>> GetSpecialitysTask(int skip, int take)
        {
            var listOfSpecialitys = new List<SpecialityDto>();

            using (var context = _serviceProvider.GetService<DatabaseContext>())
            {
                 listOfSpecialitys = await context.Specialtys
                    .AsNoTracking()
                    .Skip(skip)
                    .Take(take)
                    .Select(x => new SpecialityDto(x))
                    .ToListAsync();
            }

            return listOfSpecialitys;
        }

        public async Task<List<SpecialityDto>> GetSpecialitysByNameTask(int skip, int take, string name)
        {
            var listOfSpecialitys = new List<SpecialityDto>();

            using (var context = _serviceProvider.GetService<DatabaseContext>())
            {
                listOfSpecialitys = await context.Specialtys
                    .AsNoTracking()
                    .Where(x=> x.Name.ToLower().Contains(name.ToLower()))
                    .Skip(skip)
                    .Take(take)
                    .Select(x => new SpecialityDto(x))
                    .ToListAsync();
            }

            return listOfSpecialitys;
        }

        public async Task<List<SpecialityDto>> GetSpecialitysInUniversity(int skip, int take, int universityId)
        {
            var listOfSpecialitys = new List<SpecialityDto>();

            using (var context = _serviceProvider.GetService<DatabaseContext>())
            {
                listOfSpecialitys = await context.Specialtys
                    .AsNoTracking()
                    .Include(x=> x.UniversitySpecialities)
                    .Where(x =>x.UniversitySpecialities.Exists(us => us.UniversityId == universityId))
                    .Skip(skip)
                    .Take(take)
                    .Select(x => new SpecialityDto(x))
                    .ToListAsync();
            }

            return listOfSpecialitys;
        }
    }
}
