
using System;
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
    /// Controller for manage entrant.
    /// </summary>
    [Authorize]
    [ApiController]
    [Route("v{version:apiVersion}/[controller]")]
    public class EntrantController : Controller
    {
        private readonly IEntrantService _entrantService;

        /// <summary>
        /// Basic constructor.
        /// </summary>
        /// <param name="entrantService"><see cref="IEntrantService"/> instance.</param>
        public EntrantController(IEntrantService entrantService)
        {
            _entrantService = entrantService;
        }

        /// <summary>
        /// Get Entrants by rang.
        /// </summary>
        /// <param name="skip">Count of element for skip.</param>
        /// <param name="take">Count of element for take.</param>
        /// <param name="cancellationToken"><see cref="CancellationToken"/> instance.</param>
        [HttpGet("pagination")]
        public async Task<List<EntrantDto>> GetEntrantsAsync([FromQuery] int skip, [FromQuery] int take, CancellationToken cancellationToken)
        {
            var result = await _entrantService.GetEntrantsAsync(skip, take, cancellationToken).ConfigureAwait(false);

            return result;
        }

        /// <summary>
        /// Get Entrants by rang and name.
        /// </summary>
        /// <param name="skip">Count of element for skip.</param>
        /// <param name="take">Count of element for take.</param>
        /// <param name="name">Part of name.</param>
        /// <param name="cancellationToken"><see cref="CancellationToken"/> instance.</param>
        [HttpGet("pagination/filter")]
        public async Task<List<EntrantExtendDto>> GetEntrantsByNameAsync([FromQuery] int skip, [FromQuery] int take,
            [FromQuery] string name, CancellationToken cancellationToken)
        {
            var result = await _entrantService.GetEntrantsByNameAsync(skip, take, name, cancellationToken).ConfigureAwait(false);

            return result;
        }

        /// <summary>
        /// Get Entrant by id.
        /// </summary>
        /// <param name="entrantId">Present entrant.</param>
        /// <param name="cancellationToken"><see cref="CancellationToken"/> instance.</param>
        [HttpGet("byId")]
        public async Task<EntrantExtendDto> GetEntrantByIdAsync([FromQuery] Guid entrantId, CancellationToken cancellationToken)
        {
            var result = await _entrantService.GetEntrantByIdAsync(entrantId, cancellationToken).ConfigureAwait(false);

            return result;
        }

        /// <summary>
        /// Get Entrant by id.
        /// </summary>
        /// <param name="specialityId">Speciality identifier.</param>
        /// <param name="cancellationToken"><see cref="CancellationToken"/> instance.</param>
        [HttpGet("speciality")]
        public async Task<List<EntrantExtendDto>> GetEntrantBySpecialityIdAsync([FromQuery] Guid specialityId, CancellationToken cancellationToken)
        {
            var result = await _entrantService.GetEntrantBySpecialityIdAsync(specialityId, cancellationToken).ConfigureAwait(false);

            return result;
        }

        /// <summary>
        /// Tie up entrant and user.
        /// </summary>
        /// <param name="entrantId"> Entrant identifier.</param>
        /// <param name="cancellationToken"><see cref="CancellationToken"/> instance.</param>
        [HttpPut("combine")]
        public async Task CombineEntrantAndUserAsync([FromQuery] Guid entrantId,
            CancellationToken cancellationToken)
        {
            await _entrantService.TieUpEntrantAndUserAsync(Guid.Parse(User.Identity.Name), entrantId, cancellationToken)
                .ConfigureAwait(false);
        }
    }
}
