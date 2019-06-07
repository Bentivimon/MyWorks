using System;
using System.Threading;
using System.Threading.Tasks;
using GraduateWork.Server.Services.Abstractions;
using Microsoft.Extensions.Hosting;

namespace GraduateWork.Server.Api.HostedServices
{
    /// <summary>
    /// Base hosted service for schedule.
    /// </summary>
    public class CalculationHostedService : IHostedService
    {
        private readonly IApplicationLifetime _applicationLifetime;
        private readonly IRatingCalculationService _ratingCalculationService;

        /// <summary>
        /// Basic constructor
        /// </summary>
        /// <param name="applicationLifetime"><see cref="IApplicationLifetime"/> instance.</param>
        /// <param name="ratingCalculationService"><see cref="IRatingCalculationService"/> instance.</param>
        public CalculationHostedService(IApplicationLifetime applicationLifetime, IRatingCalculationService ratingCalculationService)
        {
            _applicationLifetime = applicationLifetime;
            _ratingCalculationService = ratingCalculationService;
        }

        /// <inheritdoc/>
        public async Task StartAsync(CancellationToken cancellationToken)
        {
            while (cancellationToken.IsCancellationRequested)
            {
                await Task.Delay(new TimeSpan(23, 0, 0)).ConfigureAwait(false);
                //await _ratingCalculationService.CalculateAsync(cancellationToken).ConfigureAwait(false);
            }
        }

        /// <inheritdoc/>
        public Task StopAsync(CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }
    }
}
