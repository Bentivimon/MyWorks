using System;
using System.Threading.Tasks;
using GraduateWorkApi.Services.Abstractions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Models.CustomExceptions;
using Models.RequestModels.UniversityModels;
using NLog;

namespace GraduateWorkApi.Controllers
{
    [Authorize]
    public class UniversityController : Controller
    {
        private readonly IUniversityService _universityService;
        private readonly Logger _logger;

        public UniversityController(IUniversityService universityService)
        {
            _universityService = universityService;
            _logger = LogManager.GetCurrentClassLogger();
        }

        /// <summary>
        /// Get Universitys by range.
        /// </summary>
        /// <returns>University Dto models</returns>
        /// <response code="200">list Universitys</response>
        /// <response code="500">Intenal Server Error</response>
        [HttpGet("api/Universitys&skip={skip}&take={take}")]
        public async Task<IActionResult> GetUniversitysAsync([FromRoute] int skip, [FromRoute] int take)
        {
            try
            {
                var result = await _universityService.GetUniversitysTask(skip, take);

                return StatusCode(200, result);
            }
            catch (Exception ex)
            {
                _logger.Error(ex);
                return StatusCode(500, "Internal server error");
            }
        }

        /// <summary>
        /// Get Universitys by range and name.
        /// </summary>
        /// <returns>University Dto models</returns>
        /// <response code="200">list Universitys</response>
        /// <response code="500">Intenal Server Error</response>
        [HttpGet("api/Universitys&skip={skip}&take={take}&name={name}")]
        public async Task<IActionResult> GetUniversitysByNameAsync([FromRoute] int skip, [FromRoute] int take,
            [FromRoute] string name)
        {
            try
            {
                var result = await _universityService.GetUniversitysByNameTask(skip, take, name);

                return StatusCode(200, result);
            }
            catch (Exception ex)
            {
                _logger.Error(ex);
                return StatusCode(500, "Internal server error");
            }
        }

        /// <summary>
        /// Add University.
        /// </summary>
        /// <returns>Ok</returns>
        /// <response code="200">Ok</response>
        /// <reaponse code="400">Invalid Model</reaponse>
        /// <response code="500">Intenal Server Error</response>
        [HttpPost("api/University")]
        public async Task<IActionResult> AddUniversityAsync([FromBody] UnivesityModelRequest request)
        {
            if (!ModelState.IsValid)
                return BadRequest();

            try
            {
                await _universityService.AddUniversityTask(request);

                return StatusCode(200);
            }
            catch (Exception ex)
            {
                _logger.Error(ex);
                return StatusCode(500, "Internal server error");
            }
        }

        /// <summary>
        /// Edit University info.
        /// </summary>
        /// <returns>University Dto model</returns>
        /// <response code="200">Edited university model</response>
        /// <reaponse code="400">Invalid Model</reaponse>
        /// <reaponse code="404">Unoversity not found</reaponse>
        /// <response code="500">Intenal Server Error</response>
        [HttpPut("api/University")]
        public async Task<IActionResult> EditUniversityAsync([FromBody] UnivesityModelRequest request)
        {
            if (!ModelState.IsValid)
                return BadRequest();

            try
            {
                var result = await _universityService.EditUniversityTask(request);

                return StatusCode(200, result);
            }
            catch (UniversityNotFoundException ex)
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
