using System;
using System.Threading;
using System.Threading.Tasks;
using GraduateWork.Server.Models.Request;
using GraduateWork.Server.Models.Response;

namespace GraduateWork.Server.Services.Abstractions
{
    /// <summary>
    /// Describe account service
    /// </summary>
    public interface IAccountService
    {
        /// <summary>
        /// Method for user login.
        /// </summary>
        /// <param name="loginModel">User login model.</param>
        /// <param name="cancellationToken">Token for cancel operation.</param>
        Task<Guid> LoginAsync(UserLoginModel loginModel, CancellationToken cancellationToken);

        /// <summary>
        /// Method for register user.
        /// </summary>
        /// <param name="registrationModel">User registration model.</param>
        /// <param name="cancellationToken">Token for cancel operation.</param>
        Task RegisterAsync(RegistrationModel registrationModel, CancellationToken cancellationToken);

        /// <summary>
        /// Method for get user model.
        /// </summary>
        /// <param name="id">User id.</param>
        /// <param name="cancellationToken">Token for cancel operation.</param>
        Task<UserDto> GetUserModelAsync(Guid id, CancellationToken cancellationToken);

        /// <summary>
        /// Method for update user info.
        /// </summary>
        /// <param name="id">User id.</param>
        /// <param name="userModel">User dro model.</param>
        /// <param name="cancellationToken">Token for cancel operation.</param>
        Task EditUserAsync(Guid id, UserDto userModel, CancellationToken cancellationToken);

        /// <summary>
        /// Method for change user password.
        /// </summary>
        /// <param name="id">User id.</param>
        /// <param name="model">Change user password model.</param>
        /// <param name="cancellationToken">Token for cancel operation.</param>
        Task ChangeUserPasswordAsync(Guid id, ChangeUserPasswordModel model, CancellationToken cancellationToken);

        /// <summary>
        /// Method for change user email.
        /// </summary>
        /// <param name="id">User id.</param>
        /// <param name="model">Change user email model.</param>
        /// <param name="cancellationToken">Token for cancel operation.</param>
        Task ChangeUserEmailAsync(Guid id, ChangeUserEmailModel model, CancellationToken cancellationToken);

        /// <summary>
        /// Set tracking entrant.
        /// </summary>
        /// <param name="userId">User id.</param>
        /// <param name="entrantId">Entrant id.</param>
        /// <param name="cancellationToken"><see cref="CancellationToken"/> instance.</param>
        /// <returns></returns>
        Task SetTrackingEntrantAsync(Guid userId, Guid entrantId, CancellationToken cancellationToken);
    }
}
