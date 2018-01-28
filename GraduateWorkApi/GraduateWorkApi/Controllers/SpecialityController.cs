using System;
using System.Threading.Tasks;
using GraduateWorkApi.Services.Abstractions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Models.CustomExceptions;
using Models.RequestModels.SpecialityModels;
using NLog;

namespace GraduateWorkApi.Controllers
{
    [Authorize]
    public class SpecialityController : Controller
    {
        private readonly ISpecialityService _specialityService;
        private readonly Logger _logger;

        public SpecialityController(ISpecialityService specialityService)
        {
            _specialityService = specialityService;
            _logger = LogManager.GetCurrentClassLogger();
        }

        /// <summary>
        /// Add Speciality.
        /// </summary>
        /// <returns>Ok</returns>
        /// <response code="200">Ok</response>
        /// <response code="500">Intenal Server Error</response>
        [HttpPost("api/Speciality")]
        public async Task<IActionResult> AddSpecialityAsync([FromBody] SpecialityRequest request)
        {
            if (!ModelState.IsValid)
                return BadRequest();

            try
            {
                await _specialityService.AddSpecialityTask(request);

                return StatusCode(200);
            }
            catch (Exception ex)
            {
                _logger.Error(ex);
                return StatusCode(500, "Internal server error");
            }
        }

        /// <summary>
        /// Edit Speciality.
        /// </summary>
        /// <returns>Speciality Dto model</returns>
        /// <response code="200">Speciality Dto model</response>
        /// <response code="404">Speciality not found</response>
        /// <response code="500">Intenal Server Error</response>
        [HttpPut("api/Speciality")]
        public async Task<IActionResult> EditSpecialityAsync([FromBody] SpecialityRequest request)
        {
            if (!ModelState.IsValid)
                return BadRequest();

            try
            {
                var result = await _specialityService.EditSpecialityTask(request);

                return StatusCode(200, result);
            }
            catch (SpecialityNotFoundException ex)
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
        /// Get Specialitys by range.
        /// </summary>
        /// <returns>Speciality Dto models</returns>
        /// <response code="200">list Specialitys</response>
        /// <response code="500">Intenal Server Error</response>
        [HttpGet("api/Specialitys&skip={skip}&take={take}")]
        public async Task<IActionResult> GetSpecialitysAsync([FromRoute] int skip, [FromRoute] int take)
        {
            try
            {
                var result = await _specialityService.GetSpecialitysTask(skip, take);

                return StatusCode(200, result);
            }
            catch (Exception ex)
            {
                _logger.Error(ex);
                return StatusCode(500, "Internal server error");
            }
        }

        /// <summary>
        /// Get Specialitys by range and name.
        /// </summary>
        /// <returns>Speciality Dto models</returns>
        /// <response code="200">list Specialitys</response>
        /// <response code="500">Intenal Server Error</response>
        [HttpGet("api/Specialitys&skip={skip}&take={take}&name={name}")]
        public async Task<IActionResult> GetSpecialitysByNameAsync([FromRoute] int skip, [FromRoute] int take, [FromRoute] string name)
        {
            try
            {
                var result = await _specialityService.GetSpecialitysByNameTask(skip, take, name);

                return StatusCode(200, result);
            }
            catch (Exception ex)
            {
                _logger.Error(ex);
                return StatusCode(500, "Internal server error");
            }
        }

        /// <summary>
        /// Get Specialitys by range and univesity Id.
        /// </summary>
        /// <returns>Speciality Dto models</returns>
        /// <response code="200">list Specialitys</response>
        /// <response code="500">Intenal Server Error</response>
        [HttpGet("api/Specialitys&skip={skip}&take={take}&univerisity={universityId}")]
        public async Task<IActionResult> GetSpecialitysInUniversityAsync([FromRoute] int skip, [FromRoute] int take, [FromRoute] int universityId)
        {
            try
            {
                var result = await _specialityService.GetSpecialitysInUniversity(skip, take, universityId);

                return StatusCode(200, result);
            }
            catch (Exception ex)
            {
                _logger.Error(ex);
                return StatusCode(500, "Internal server error");
            }
        }
    }
}