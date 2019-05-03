using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using GraduateWork.Server.Models.Response;

namespace GraduateWork.Server.Services.Abstractions
{
    /// <summary>
    /// Represent for communicate with regions in database.
    /// </summary>
    public interface IRegionService
    {
        /// <summary>
        /// Method return all regions from database.
        /// </summary>
        /// <param name="cancellationToken"><see cref="CancellationToken"/> instance.</param>
        Task<List<RegionDto>> GetAllRegionsAsync(CancellationToken cancellationToken);
    }
}
