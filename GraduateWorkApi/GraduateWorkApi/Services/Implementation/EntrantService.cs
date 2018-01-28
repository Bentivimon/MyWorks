using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EntityModels.Entitys;
using Microsoft.Extensions.DependencyInjection;
using GraduateWorkApi.Context;
using GraduateWorkApi.Services.Abstractions;
using Microsoft.EntityFrameworkCore;
using Models.CustomExceptions;
using Models.DTOModels.EntrantModels;
using Models.RequestModels.EntrantModels;

namespace GraduateWorkApi.Services.Implementation
{
    public class EntrantService: IEntrantService
    {
        private readonly IServiceProvider _serviceProvider;

        public EntrantService(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public async Task AddEntrantTask(EntrantRequest request)
        {
            using (var context = _serviceProvider.GetService<DatabaseContext>())
            {
                context.Entrants.Add(new EntrantEntity(request));

                await context.SaveChangesAsync();
            }
        }

        public async Task<EntrantDto> EditEntrantTask(int entrantId, EntrantRequest request)
        {
            using (var context = _serviceProvider.GetService<DatabaseContext>())
            {
                var entrantEntity = await context.Entrants
                    .FirstOrDefaultAsync(x => x.Id == entrantId);

                if(entrantEntity == null)
                    throw new EntrantNotFoundException();

                entrantEntity.Name = request.Name;
                entrantEntity.Surname = request.Surname;
                entrantEntity.BDay = request.BDay;

                await context.SaveChangesAsync();

                return new EntrantDto(entrantEntity);
            }
        }

        public async Task<List<EntrantDto>> GetEntrantsTask(int skip, int take)
        {
            using (var context = _serviceProvider.GetService<DatabaseContext>())
            {
                var result = await context.Entrants
                    .Skip(skip)
                    .Take(take)
                    .ToListAsync();

                return result.Select(x => new EntrantDto(x)).ToList();
            }
        }

        public async Task<List<EntrantDto>> GetEntrantsByNameTask(int skip, int take, string name)
        {
            using (var context = _serviceProvider.GetService<DatabaseContext>())
            {
                var result = await context.Entrants
                    .Where(x=> x.Name.ToLower().Contains(name.ToLower()) || x.Surname.ToLower().Contains(name.ToLower()))
                    .Skip(skip)
                    .Take(take)
                    .ToListAsync();

                return result.Select(x => new EntrantDto(x)).ToList();
            }
        }

        public async Task<EntrantExpandDto> GetEntrantByIdTask(int entrantId)
        {
            using (var context = _serviceProvider.GetService<DatabaseContext>())
            {
                var result = await context.Entrants
                    .FirstOrDefaultAsync(x => x.Id == entrantId);

                if(result == null)
                    throw new EntrantNotFoundException();

                return new EntrantExpandDto(result);
            }
        }

        public async Task TieUpEnatrantAndUserTask(string userId, int entrantId)
        {
            using (var context = _serviceProvider.GetService<DatabaseContext>())
            {
                var entrantEntity = await context.Entrants
                    .FirstOrDefaultAsync(x => x.Id == entrantId);

                if(entrantEntity == null)
                    throw new EntrantNotFoundException();

                entrantEntity.UserId = userId;
                await context.SaveChangesAsync();
            }
        }
    }
}
