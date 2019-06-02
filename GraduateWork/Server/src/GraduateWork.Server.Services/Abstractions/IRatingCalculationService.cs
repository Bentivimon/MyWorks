using System.Threading;
using System.Threading.Tasks;

namespace GraduateWork.Server.Services.Abstractions
{
    /// <summary>
    /// Service for calculate entrants ratings.
    /// </summary>
    public interface IRatingCalculationService
    {
        /// <summary>
        /// Method for calculate entrant ratings.
        /// </summary>
        /// <param name="cancellationToken"><see cref="CancellationToken"/> instance.</param>
        Task CalculateAsync(CancellationToken cancellationToken);
    }
}
