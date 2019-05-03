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
    /// Controller for manage university.
    /// </summary>
    [Authorize]
    [ApiController]
    [Route("v{version:apiVersion}/[controller]")]
    public class UniversityController : Controller
    {
        private readonly IUniversityService _universityService;

        /// <summary>
        /// Basic constructor.
        /// </summary>
        /// <param name="universityService"><see cref="IUniversityService"/> instance.</param>
        public UniversityController(IUniversityService universityService)
        {
            _universityService = universityService;
        }

        /// <summary>
        /// Get Universities by range.
        /// </summary>
        /// <param name="skip">Count of element for skip.</param>
        /// <param name="take">Count of element for take.</param>
        /// <param name="cancellationToken"><see cref="CancellationToken"/> instance.</param>
        [HttpGet("pagination")]
        public async Task<List<UniversityDto>> GetUniversitiesAsync([FromQuery] int skip, [FromQuery] int take,
            CancellationToken cancellationToken)
        {
            var result = await _universityService.GetUniversitiesAsync(skip, take, cancellationToken)
                .ConfigureAwait(false);

            return result;
        }

        /// <summary>
        /// Get Universities by range and name.
        /// </summary>
        /// <param name="skip">Count of element for skip.</param>
        /// <param name="take">Count of element for take.</param>
        /// <param name="name"> University name.</param>
        /// <param name="cancellationToken"><see cref="CancellationToken"/> instance.</param>
        [HttpGet("pagination/filter")]
        public async Task<List<UniversityDto>> GetUniversitiesByNameAsync([FromQuery] int skip, [FromQuery] int take,
            [FromQuery] string name, CancellationToken cancellationToken)
        {
            var result = await _universityService.GetUniversitiesByNameAsync(skip, take, name, cancellationToken)
                .ConfigureAwait(false);

            return result;
        }

        /// <summary>
        /// Get Universities by range and name.
        /// </summary>
        /// <param name="regionId">Region identifier.</param>
        /// <param name="cancellationToken"><see cref="CancellationToken"/> instance.</param>
        [HttpGet("region")]
        public async Task<List<UniversityDto>> GetUniversitiesByRegionAsync([FromQuery] int regionId, CancellationToken cancellationToken)
        {
            var result = await _universityService.GetUniversitiesByrRegionIdAsync(regionId, cancellationToken)
                .ConfigureAwait(false);

            return result;
        }

        /// <summary>
        /// Add University.
        /// </summary>
        /// <param name="request"><see cref="UniversityRequest"/> instance.</param>
        /// <param name="cancellationToken"><see cref="CancellationToken"/> instance.</param>
        [HttpPost]
        public async Task AddUniversityAsync([FromBody] UniversityRequest request, CancellationToken cancellationToken)
        {
            await _universityService.AddUniversityAsync(request,cancellationToken).ConfigureAwait(false);
        }

        /// <summary>
        /// Update University info.
        /// </summary>
        /// <param name="request"><see cref="UniversityRequest"/> instance.</param>
        /// <param name="cancellationToken"><see cref="CancellationToken"/> instance.</param>
        [HttpPut]
        public async Task<UniversityDto> UpdateUniversityAsync([FromBody] UniversityRequest request,
            CancellationToken cancellationToken)
        {
            var result = await _universityService.UpdateUniversitiesAsync(request, cancellationToken).ConfigureAwait(false);

            return result;
        }
    }
}
