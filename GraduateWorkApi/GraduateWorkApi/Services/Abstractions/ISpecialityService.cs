using System.Collections.Generic;
using System.Threading.Tasks;
using Models.DTOModels.SpecialityModels;
using Models.RequestModels.SpecialityModels;

namespace GraduateWorkApi.Services.Abstractions
{
    public interface ISpecialityService
    {
        Task AddSpecialityTask(SpecialityRequest request);
        Task<SpecialityDto> EditSpecialityTask(SpecialityRequest request);
        Task<List<SpecialityDto>> GetSpecialitysTask(int skip, int take);
        Task<List<SpecialityDto>> GetSpecialitysByNameTask(int skip, int take, string name);
        Task<List<SpecialityDto>> GetSpecialitysInUniversity(int skip, int take, int universityId);
    }
}
