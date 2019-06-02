using System;
using System.IdentityModel.Tokens.Jwt;
using System.Threading;
using System.Threading.Tasks;
using GraduateWork.Server.Models.Request;
using GraduateWork.Server.Models.Response;
using GraduateWork.Server.Services.Abstractions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace GraduateWork.Server.Api.Controllers
{
    /// <inheritdoc />
    /// <summary>
    /// Account controller.
    /// </summary>
    [Authorize]
    [ApiController]
    [Route("v{version:apiVersion}/[controller]")]
    public class AccountController : Controller
    {
        private readonly IAccountService _accountService;
        private readonly IJwtTokenService _jwtTokenService;

        /// <summary>
        /// Base constructor. 
        /// </summary>
        /// <param name="accountService"><see cref="IAccountService"/> instance.</param>
        /// <param name="jwtTokenService"><see cref="IJwtTokenService"/> instance.</param>
        public AccountController(IAccountService accountService, IJwtTokenService jwtTokenService)
        {
            _accountService = accountService;
            _jwtTokenService = jwtTokenService;
        }

        /// <summary>
        /// Get user access token.
        /// </summary>
        /// <param name="loginModel"><see cref="UserLoginModel"/> instance.</param>
        /// <param name="cancellationToken"><see cref="CancellationToken"/> instance.</param>
        [AllowAnonymous]
        [HttpPost("token")]
        public async Task<string> LoginAsync([FromBody] UserLoginModel loginModel,
            CancellationToken cancellationToken)
        {
            var userId = await _accountService.LoginAsync(loginModel, cancellationToken).ConfigureAwait(false);

            var userToken = _jwtTokenService.GenerateJwtTokenAsync(userId);

            return JsonConvert.SerializeObject(new JwtSecurityTokenHandler().WriteToken(userToken));
        }

        /// <summary>
        /// Register new user. 
        /// </summary>
        /// <param name="registrationModel"><see cref="RegistrationModel"/> instance.</param>
        /// <param name="cancellationToken"><see cref="CancellationToken"/> instance.</param>
        [AllowAnonymous]
        [HttpPost("registration")]
        public async Task RegistrationAsync([FromBody] RegistrationModel registrationModel,
            CancellationToken cancellationToken)
        {
             await _accountService.RegisterAsync(registrationModel, cancellationToken).ConfigureAwait(false);
        }

        /// <summary>
        /// Get user info model.
        /// </summary>
        /// <param name="cancellationToken"><see cref="CancellationToken"/> instance.</param>
        [HttpGet("userInfo")]
        public async Task<UserDto> GetUserInfoAsync(CancellationToken cancellationToken)
        {
            var userModel = await _accountService.GetUserModelAsync(Guid.Parse(User.Identity.Name), cancellationToken).ConfigureAwait(false);

            return userModel;
        }

        /// <summary>
        /// Edit User Info Model.
        /// </summary>
        /// <param name="userModel"><see cref="UserDto"/> instance.</param>
        /// <param name="cancellationToken"><see cref="CancellationToken"/> instance.</param>
        [HttpPut("userInfo")]
        public async Task UpdateUserAsync([FromBody] UserDto userModel, CancellationToken cancellationToken)
        {
            await _accountService.EditUserAsync(Guid.Parse(User.Identity.Name), userModel, cancellationToken).ConfigureAwait(false);
        }

        /// <summary>
        /// Change user Password.
        /// </summary>
        /// <param name="model"><see cref="ChangeUserPasswordModel"/> instance.</param>
        /// <param name="cancellationToken"><see cref="CancellationToken"/> instance.</param>
        [HttpPut("changePassword")]
        public async Task ChangeUserPasswordAsync([FromBody] ChangeUserPasswordModel model,
            CancellationToken cancellationToken)
        {
            await _accountService.ChangeUserPasswordAsync(Guid.Parse(User.Identity.Name), model, cancellationToken)
                .ConfigureAwait(false);
        }

        /// <summary>
        /// Change user Email address.
        /// </summary>
        /// <param name="model"><see cref="ChangeUserEmailModel"/> instance.</param>
        /// <param name="cancellationToken"><see cref="CancellationToken"/> instance.</param>
        [HttpPut("changeEmail")]
        public async Task ChangeUserEmailAsync([FromBody] ChangeUserEmailModel model, CancellationToken cancellationToken)
        {
           await _accountService.ChangeUserEmailAsync(Guid.Parse(User.Identity.Name), model, cancellationToken).ConfigureAwait(false);
        }

        /// <summary>
        /// Set tracking entrant.
        /// </summary>
        /// <param name="entrantId">Entrant id.</param>
        /// <param name="cancellationToken"><see cref="CancellationToken"/> instance.</param>
        [HttpPost("tracking")]
        public async Task SetTrackingEntrantAsync([FromQuery] Guid entrantId, CancellationToken cancellationToken)
        {
            await _accountService.SetTrackingEntrantAsync(Guid.Parse(User.Identity.Name), entrantId, cancellationToken).ConfigureAwait(false);
        }
    }
}
