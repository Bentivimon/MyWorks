using System;
using System.Threading.Tasks;
using GraduateWorkApi.Services.Abstractions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Models.CustomExceptions;
using NLog;

namespace GraduateWorkApi.Controllers
{
    [Authorize]
    public class EntrantController : Controller
    {
        private readonly IEntrantService _entrantService;
        private readonly Logger _logger;

        public EntrantController(IEntrantService entrantService)
        {
            _entrantService = entrantService;
            _logger = LogManager.GetCurrentClassLogger();
        }

        /// <summary>
        /// Get Entrants by rang.
        /// </summary>
        /// <returns>List of entrants dto</returns>
        /// <response code="200">List of entrants dto</response>
        /// <response code="500">Intenal Server Error</response>
        [HttpGet("api/Entrants&skip={skip}&take={take}")]
        public async Task<IActionResult> GetEntrantsAsync([FromRoute] int skip, [FromRoute] int take)
        {
            try
            {
                var result = await _entrantService.GetEntrantsTask(skip, take);

                return StatusCode(200, result);
            }
            catch (Exception ex)
            {
                _logger.Error(ex);
                return StatusCode(500, "Internal server error");
            }
        }

        /// <summary>
        /// Get Entrants by rang and name.
        /// </summary>
        /// <returns>List of entrants dto</returns>
        /// <response code="200">List of entrants dto</response>
        /// <response code="500">Intenal Server Error</response>
        [HttpGet("api/Entrants&skip={skip}&take={take}&name={name}")]
        public async Task<IActionResult> GetEntrantsByNameAsync([FromRoute] int skip, [FromRoute] int take, [FromRoute] string name)
        {
            try
            {
                var result = await _entrantService.GetEntrantsByNameTask(skip, take, name);

                return StatusCode(200, result);
            }
            catch (Exception ex)
            {
                _logger.Error(ex);
                return StatusCode(500, "Internal server error");
            }
        }

        /// <summary>
        /// Get Entrant by id.
        /// </summary>
        /// <returns>Entrant expand dto</returns>
        /// <response code="200">Entrant expand dto</response>
        /// <response code="404">Entrant not found</response>
        /// <response code="500">Intenal Server Error</response>
        [HttpGet("api/Entrant&id={entrantId}")]
        public async Task<IActionResult> GetEntrantByIdAsync([FromRoute] int entrantId)
        {
            try
            {
                var result = await _entrantService.GetEntrantByIdTask(entrantId);

                return StatusCode(200, result);
            }
            catch (EntrantNotFoundException ex)
            {
                return StatusCode(404, ex.Message);
            }
            catch (Exception ex)
            {
                _logger.Error(ex);
                return StatusCode(500, "Internal server error");
            }
        }

        /// <summary>
        /// Tie up enatrant and user.
        /// </summary>
        /// <returns>Ok</returns>
        /// <response code="200">Ok</response>
        /// <response code="404">Entrant not found</response>
        /// <response code="500">Intenal Server Error</response>
        [HttpPut("api/Entrant={entrantId}&User")]
        public async Task<IActionResult> TieUpEnatrantAndUserAsync([FromRoute] int entrantId)
        {
            try
            {
                await _entrantService.TieUpEnatrantAndUserTask(User.Identity.Name, entrantId);

                return StatusCode(200);
            }
            catch (EntrantNotFoundException ex)
            {
                return StatusCode(404, ex.Message);
            }
            catch (Exception ex)
            {
                _logger.Error(ex);
                return StatusCode(500, "Internal server error");
            }
        }
    }
}