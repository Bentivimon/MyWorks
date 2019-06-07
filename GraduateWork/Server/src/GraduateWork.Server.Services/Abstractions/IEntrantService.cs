using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using GraduateWork.Server.Models.Request;
using GraduateWork.Server.Models.Response;

namespace GraduateWork.Server.Services.Abstractions
{
    public interface IEntrantService
    {
        Task AddEntrantAsync(EntrantModel request, CancellationToken cancellationToken);
        Task<EntrantDto> UpdateEntrantAsync(Guid entrantId, EntrantModel request, CancellationToken cancellationToken);
        Task<List<EntrantDto>> GetEntrantsAsync(int skip, int take, CancellationToken cancellationToken);
        Task<List<EntrantExtendDto>> GetEntrantsByNameAsync(int skip, int take, string name, CancellationToken cancellationToken);
        Task<EntrantExtendDto> GetEntrantByIdAsync(Guid entrantId, CancellationToken cancellationToken);
        Task<List<EntrantExtendDto>> GetEntrantBySpecialityIdAsync(Guid specialityId, CancellationToken cancellationToken);
        Task TieUpEntrantAndUserAsync(Guid userId, Guid entrantId, CancellationToken cancellationToken);
    }
}
