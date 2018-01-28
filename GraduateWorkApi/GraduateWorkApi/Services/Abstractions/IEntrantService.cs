using System.Collections.Generic;
using System.Threading.Tasks;
using Models.DTOModels.EntrantModels;
using Models.RequestModels.EntrantModels;

namespace GraduateWorkApi.Services.Abstractions
{
    public interface IEntrantService
    {
        Task AddEntrantTask(EntrantRequest request);
        Task<EntrantDto> EditEntrantTask(int entrantId, EntrantRequest request);
        Task<List<EntrantDto>> GetEntrantsTask(int skip, int take);
        Task<List<EntrantDto>> GetEntrantsByNameTask(int skip, int take, string name);
        Task<EntrantExpandDto> GetEntrantByIdTask(int entrantId);
        Task TieUpEnatrantAndUserTask(string userId, int entrantId);
    }
}
