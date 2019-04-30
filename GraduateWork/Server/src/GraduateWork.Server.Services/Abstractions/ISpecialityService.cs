using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using GraduateWork.Server.Models.Request;
using GraduateWork.Server.Models.Response;

namespace GraduateWork.Server.Services.Abstractions
{
    /// <summary>
    /// Represent speciality service.
    /// </summary>
    public interface ISpecialityService
    {
        /// <summary>
        /// Method for add speciality.
        /// </summary>
        /// <param name="request"><see cref="SpecialityRequest"/> instance.</param>
        /// <param name="cancellationToken"><see cref="CancellationToken"/> instance.</param>
        Task AddSpecialityAsync(SpecialityRequest request, CancellationToken cancellationToken);

        /// <summary>
        /// Method for update speciality.
        /// </summary>
        /// <param name="request"><see cref="SpecialityRequest"/> instance.</param>
        /// <param name="cancellationToken"><see cref="CancellationToken"/> instance.</param>
        Task<SpecialityDto> UpdateSpecialityAsync(SpecialityRequest request, CancellationToken cancellationToken);

        /// <summary>
        /// Method for get speciality by range.
        /// </summary>
        /// <param name="skip">How many records skip.</param>
        /// <param name="take">How many records take.</param>
        /// <param name="cancellationToken"><see cref="CancellationToken"/> instance.</param>
        Task<List<SpecialityDto>> GetSpecialitiesAsync(int skip, int take, CancellationToken cancellationToken);

        /// <summary>
        /// Method for get speciality by range and the same name.
        /// </summary>
        /// <param name="skip">How many records skip.</param>
        /// <param name="take">How many records take.</param>
        /// <param name="name">Part of name.</param>
        /// <param name="cancellationToken"><see cref="CancellationToken"/> instance.</param>
        Task<List<SpecialityDto>> GetSpecialitiesByNameAsync(int skip, int take, string name, CancellationToken cancellationToken);

        /// <summary>
        /// Method for get speciality by range and university id.
        /// </summary>
        /// <param name="skip">How many records skip.</param>
        /// <param name="take">How many records take.</param>
        /// <param name="universityId">University id.</param>
        /// <param name="cancellationToken"><see cref="CancellationToken"/> instance.</param>
        Task<List<SpecialityDto>> GetSpecialitiesInUniversityAsync(int skip, int take, Guid universityId, CancellationToken cancellationToken);
    }
}
