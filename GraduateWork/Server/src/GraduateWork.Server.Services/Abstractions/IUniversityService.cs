using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using GraduateWork.Server.Models.Request;
using GraduateWork.Server.Models.Response;

namespace GraduateWork.Server.Services.Abstractions
{
    /// <summary>
    /// Represent university service.
    /// </summary>
    public interface IUniversityService
    {
        /// <summary>
        /// Method for add university.
        /// </summary>
        /// <param name="request"><see cref="UniversityRequest"/> instance.</param>
        /// <param name="cancellationToken"><see cref="CancellationToken"/> instance.</param>
        Task AddUniversityAsync(UniversityRequest request, CancellationToken cancellationToken);

        /// <summary>
        /// Method for get university with page pagination.
        /// </summary>
        /// <param name="skip">How many records skip.</param>
        /// <param name="take">How many records take.</param>
        /// <param name="cancellationToken"><see cref="CancellationToken"/> instance.</param>
        Task<List<UniversityDto>> GetUniversitiesAsync(int skip, int take, CancellationToken cancellationToken);


        /// <summary>
        /// Method for add university.
        /// </summary>
        /// <param name="name">Name of university.</param>
        /// <param name="skip">How many records skip.</param>
        /// <param name="take">How many records take.</param>
        /// <param name="cancellationToken"><see cref="CancellationToken"/> instance.</param>
        Task<List<UniversityDto>> GetUniversitiesByNameAsync(int skip, int take, string name, CancellationToken cancellationToken);


        /// <summary>
        /// Method for add university.
        /// </summary>
        /// <param name="request"><see cref="UniversityRequest"/> instance.</param>
        /// <param name="cancellationToken"><see cref="CancellationToken"/> instance.</param>
        Task<UniversityDto> UpdateUniversitiesAsync(UniversityRequest request, CancellationToken cancellationToken);
    }
}
