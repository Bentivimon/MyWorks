using System;
using System.IdentityModel.Tokens.Jwt;
using System.Threading.Tasks;
using GraduateWorkApi.Services.Abstractions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Models.CustomExceptions;
using Models.DTOModels.UserModels;
using Models.RequestModels.UserModels;
using Newtonsoft.Json;
using NLog;

namespace GraduateWorkApi.Controllers
{
    [Authorize]
    public class AccountController : Controller
    {
        private readonly IAccountService _accountService;
        private readonly IJwtTokenService _jwtTokenService;
        private readonly Logger _logger;

        public AccountController(IAccountService accountService, IJwtTokenService jwtTokenService)
        {
            _accountService = accountService;
            _jwtTokenService = jwtTokenService;
            _logger =  LogManager.GetCurrentClassLogger();
        }

        /// <summary>
        /// Get user access token.
        /// </summary>
        /// <returns>A newly-created User Access Token</returns>
        /// <response code="200">A newly-created User Access Token</response>
        /// <response code="400">If model is Invalid</response>
        /// <response code="404">If User not found</response>
        /// <response code="500">Intenal Server Error</response>
        [AllowAnonymous]
        [HttpPost("api/Token")]
        public async Task<IActionResult> TokenAsync([FromBody] UserLoginModelRequest loginModel)
        {
            if (!ModelState.IsValid)
                return BadRequest();

            try
            {
                var userId = await _accountService.LogisTask(loginModel);

                var userToken = _jwtTokenService.GenerateJwtTokenTask(userId);

                return StatusCode(200, JsonConvert.SerializeObject(new JwtSecurityTokenHandler().WriteToken(userToken)));
            }
            catch (UserNotFoundException ex)
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
        /// Register new user. 
        /// </summary>
        /// <returns> Ok </returns>
        /// <response code="200">Ok</response>
        /// <response code="400">If model is Invalid</response>
        /// <response code="403">If Email or mobile phone is already exist</response>
        /// <response code="500">Intenal Server Error</response>
        [AllowAnonymous]
        [HttpPost("api/Register")]
        public async Task<IActionResult> RegisterAsync([FromBody] UserRegistrarionModelRequest registrarionModel)
        {
            if (!ModelState.IsValid)
                return BadRequest();

            try
            {
                var result = await _accountService.RegisterTask(registrarionModel);
                if (!result)
                    return StatusCode(403, "Email or mobile phone is already exist");

                return StatusCode(200);
            }
            catch (Exception ex)
            {
                _logger.Error(ex);
                return StatusCode(500, "Internal server error");

            }
        }

        /// <summary>
        /// Get user info model.
        /// </summary>
        /// <returns>User info model</returns>
        /// <response code="200">User info model</response>
        /// <response code="400">If model is Invalid</response>
        /// <response code="404">If User not found</response>
        /// <response code="500">Intenal Server Error</response>
        [HttpGet("api/UserInfo")]
        public async Task<IActionResult> GetUserInfoAsync()
        {
            if (!ModelState.IsValid)
                return BadRequest();

            try
            {
                var userModel = await _accountService.GetUserModelTask(User.Identity.Name);

                return StatusCode(200, userModel);
            }
            catch (UserNotFoundException ex)
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
        /// Edit User Info Model.
        /// </summary>
        /// <returns> Ok </returns>
        /// <response code="200"> Ok </response>
        /// <response code="400">If model is Invalid</response>
        /// <response code="404">If User not found</response>
        /// <response code="500">Intenal Server Error</response>
        [HttpPut("api/UserInfo")]
        public async Task<IActionResult> EditUserAsync([FromBody] UserDtoModel userModel)
        {
            if (!ModelState.IsValid)
                return BadRequest();

            try
            {
                await _accountService.EditUserTask(User.Identity.Name, userModel);

                return StatusCode(200);
            }
            catch (UserNotFoundException ex)
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
        /// Change user Password.
        /// </summary>
        /// <returns> Ok </returns>
        /// <response code="200"> Ok </response>
        /// <response code="400">If model is Invalid</response>
        /// <response code="401">If Password is already exist</response>
        /// <response code="404">If User not found</response>
        /// <response code="500">Intenal Server Error</response>
        [HttpPut("api/ChangePassword")]
        public async Task<IActionResult> ChangeUserPasswordAsync([FromBody] ChangeUserPasswordRequest model)
        {
            if (!ModelState.IsValid)
                return BadRequest();

            try
            {
                var result = await _accountService.ChangeUserPasswordTask(User.Identity.Name, model);

                if (!result)
                    return StatusCode(401, "Password is already exist");

                return StatusCode(200);
            }
            catch (UserNotFoundException ex)
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
        /// Change user Email adress.
        /// </summary>
        /// <returns> Ok </returns>
        /// <response code="200"> Ok </response>
        /// <response code="400">If model is Invalid</response>
        /// <response code="401">If Email is already exist</response>
        /// <response code="404">If User not found</response>
        /// <response code="500">Intenal Server Error</response>
        [HttpPut("api/ChangeEmail")]
        public async Task<IActionResult> ChangeUserEmailAsync([FromBody] ChangeUserEmailRequest model)
        {
            if (!ModelState.IsValid)
                return BadRequest();

            try
            {
                var result = await _accountService.ChangeUserEmailTask(User.Identity.Name, model);

                if (!result)
                    return StatusCode(401, "Email is already exist");

                return StatusCode(200);
            }
            catch (UserNotFoundException ex)
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