using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using GraduateWork.Server.Models.Request;
using GraduateWork.Server.Models.Response;
using GraduateWork.Server.Services.Abstractions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GraduateWork.Server.Api.Controllers
{
    /// <summary>
    /// Controller for manage speciality.
    /// </summary>
    [Authorize]
    [ApiController]
    [Route("v{version:apiVersion}/[controller]")]
    public class SpecialityController : Controller
    {
        private readonly ISpecialityService _specialityService;

        /// <summary>
        /// Basic constructor.
        /// </summary>
        /// <param name="specialityService"><see cref="ISpecialityService"/> instance.</param>
        public SpecialityController(ISpecialityService specialityService)
        {
            _specialityService = specialityService;          
        }

        /// <summary>
        /// Add Speciality.
        /// </summary>
        /// <param name="request"><see cref="SpecialityRequest"/> instance.</param>
        /// <param name="cancellationToken"><see cref="CancellationToken"/> instance.</param>
        [HttpPost]
        public async Task AddSpecialityAsync([FromBody] SpecialityRequest request, CancellationToken cancellationToken)
        {
            await _specialityService.AddSpecialityAsync(request, cancellationToken).ConfigureAwait(false);
        }

        /// <summary>
        /// Update Speciality.
        /// </summary>
        /// <param name="request"><see cref="SpecialityRequest"/> instance.</param>
        /// <param name="cancellationToken"><see cref="CancellationToken"/> instance.</param>
        [HttpPut]
        public async Task<SpecialityDto> UpdateSpecialityAsync([FromBody] SpecialityRequest request,
            CancellationToken cancellationToken)
        {
            var result = await _specialityService.UpdateSpecialityAsync(request, cancellationToken)
                .ConfigureAwait(false);

            return result;
        }

        /// <summary>
        /// Get Specialities by range.
        /// </summary>
        /// <param name="skip">Count of element for skip.</param>
        /// <param name="take">Count of element for take.</param>
        /// <param name="cancellationToken"><see cref="CancellationToken"/> instance.</param>
        [HttpGet("pagination")]
        public async Task<List<SpecialityDto>> GetSpecialitiesAsync([FromQuery] int skip, [FromQuery] int take,
            CancellationToken cancellationToken)
        {
            var result = await _specialityService.GetSpecialitiesAsync(skip, take, cancellationToken).ConfigureAwait(false);

            return result;
        }

        /// <summary>
        /// Get Specialities by range and name.
        /// </summary>
        /// <param name="skip">Count of element for skip.</param>
        /// <param name="take">Count of element for take.</param>
        /// <param name="name">Name of speciality.</param>
        /// <param name="cancellationToken"><see cref="CancellationToken"/> instance.</param>
        [HttpGet("pagination/filter")]
        public async Task<List<SpecialityDto>> GetSpecialitiesByNameAsync([FromQuery] int skip, [FromQuery] int take,
            [FromQuery] string name, CancellationToken cancellationToken)
        {
            var result = await _specialityService.GetSpecialitiesByNameAsync(skip, take, name, cancellationToken)
                .ConfigureAwait(false);

            return result;
        }

        /// <summary>
        /// Get specialities by range and university Id.
        /// </summary>
        /// <param name="skip">Count of element for skip.</param>
        /// <param name="take">Count of element for take.</param>
        /// <param name="universityId"> Represent university id.</param>
        /// <param name="cancellationToken"><see cref="CancellationToken"/> instance.</param>
        [HttpGet("pagination/byId")]
        public async Task<List<SpecialityDto>> GetSpecialitiesInUniversityAsync([FromQuery] int skip, [FromQuery] int take,
            [FromQuery] Guid universityId, CancellationToken cancellationToken)
        {
            var result = await _specialityService.GetSpecialitiesInUniversityAsync(skip, take, universityId, cancellationToken).ConfigureAwait(false);

            return result;
        }
    }
}
