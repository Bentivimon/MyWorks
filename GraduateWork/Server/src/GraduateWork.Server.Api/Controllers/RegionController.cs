using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using GraduateWork.Server.Models.Response;
using GraduateWork.Server.Services.Abstractions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GraduateWork.Server.Api.Controllers
{
    /// <summary>
    /// Represent controller for communicate with region on server.
    /// </summary>
    [Authorize]
    [ApiController]
    [Route("v{version:apiVersion}/[controller]")]
    public class RegionController : Controller
    {
        private readonly IRegionService _regionService;

        /// <summary>
        /// Basic constructor.
        /// </summary>
        /// <param name="regionService"><see cref="IRegionService"/> instance.</param>
        public RegionController(IRegionService regionService)
        {
            _regionService = regionService;
        }

        /// <summary>
        /// Method for get all regions.
        /// </summary>
        /// <param name="cancellationToken"><see cref="CancellationToken"/> instance.</param>
        [HttpGet]
        public async Task<List<RegionDto>> GetAllRegionAsync(CancellationToken cancellationToken)
        {
            var result = await _regionService.GetAllRegionsAsync(cancellationToken).ConfigureAwait(false);
            return result;
        }
    }
}
