using System.Collections.Generic;
using System.Threading.Tasks;
using Models.DTOModels.UniverisyModels;
using Models.RequestModels.UniversityModels;

namespace GraduateWorkApi.Services.Abstractions
{
    public interface IUniversityService
    {
        Task AddUniversityTask(UnivesityModelRequest request);
        Task<List<UniversityDto>> GetUniversitysTask(int skip, int take);
        Task<List<UniversityDto>> GetUniversitysByNameTask(int skip, int take, string name);
        Task<UniversityDto> EditUniversityTask(UnivesityModelRequest request);
    }
}
