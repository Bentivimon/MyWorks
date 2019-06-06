using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using GraduateWork.Server.Data;
using GraduateWork.Server.Data.Entities;
using GraduateWork.Server.Models.Request;
using GraduateWork.Server.Models.Response;
using GraduateWork.Server.Services.Abstractions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace GraduateWork.Server.Services.Implementations
{
    /// <inheritdoc/>
    public class AccountService : IAccountService
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly ICryptoProvider _cryptoProvider;

        public AccountService(IServiceProvider serviceProvider, ICryptoProvider cryptoProvider)
        {
            _serviceProvider = serviceProvider;
            _cryptoProvider = cryptoProvider;
        }

        /// <inheritdoc/>
        public async Task<Guid> LoginAsync(UserLoginModel loginModel, CancellationToken cancellationToken)
        {
            using (var scope = _serviceProvider.CreateScope())
            using (var context = scope.ServiceProvider.GetRequiredService<DatabaseContext>())
            {
                var user = await context.Users
                    .AsNoTracking()
                    .FirstOrDefaultAsync(
                        x => x.Email == loginModel.Login &&
                             x.Password == _cryptoProvider.EncodeValue(loginModel.Password), cancellationToken)
                    .ConfigureAwait(false);

                if (user == null)
                    throw new InvalidDataException("Invalid email and password");

                return user.Id;
            }
        }

        /// <inheritdoc/>
        public async Task RegisterAsync(RegistrationModel registrationModel, CancellationToken cancellationToken)
        {
            using (var scope = _serviceProvider.CreateScope())
            using (var context = scope.ServiceProvider.GetRequiredService<DatabaseContext>())
            {
                if (await context.Users
                    .AnyAsync(x => x.Email == registrationModel.Email || x.Phone == registrationModel.MobileNumber,
                        cancellationToken).ConfigureAwait(false))
                    throw new InvalidDataException("User with the same email or phone number already exist");

                var userEntity = UserEntity.CreateEntity(registrationModel,
                    _cryptoProvider.EncodeValue(registrationModel.Password));

                await context.AddAsync(userEntity, cancellationToken).ConfigureAwait(false);
                await context.SaveChangesAsync(cancellationToken).ConfigureAwait(false);
            }
        }

        /// <inheritdoc/>
        public async Task<UserDto> GetUserModelAsync(Guid id, CancellationToken cancellationToken)
        {
            using (var scope = _serviceProvider.CreateScope())
            using (var context = scope.ServiceProvider.GetRequiredService<DatabaseContext>())
            {
                var userEntity = await context.Users
                    .Include(x => x.Entrant)
                    .ThenInclude(x => x.Statements)
                    .ThenInclude(x => x.Speciality).ThenInclude(x => x.UniversitySpecialities)
                    .ThenInclude(x => x.University)
                    .AsNoTracking()
                    .FirstOrDefaultAsync(x => x.Id == id, cancellationToken).ConfigureAwait(false);

                if (userEntity == null)
                    throw new ArgumentException("User not found");

                return userEntity.ToDto();
            }
        }

        /// <inheritdoc/>
        public async Task EditUserAsync(Guid id, UserDto userModel, CancellationToken cancellationToken)
        {
            using (var scope = _serviceProvider.CreateScope())
            using (var context = scope.ServiceProvider.GetRequiredService<DatabaseContext>())
            {
                var user = await context.Users.FirstOrDefaultAsync(x => x.Id == id, cancellationToken).ConfigureAwait(false);

                if (user == null)
                    throw new ArgumentException("User not found");

                user.Update(userModel);

                await context.SaveChangesAsync(cancellationToken).ConfigureAwait(false);
            }
        }

        /// <inheritdoc/>
        public async Task ChangeUserPasswordAsync(Guid id, ChangeUserPasswordModel model, CancellationToken cancellationToken)
        {
            using (var scope = _serviceProvider.CreateScope())
            using (var context = scope.ServiceProvider.GetRequiredService<DatabaseContext>())
            {
                var user = await context.Users.FirstOrDefaultAsync(x => x.Id == id, cancellationToken).ConfigureAwait(false);

                if (user == null)
                    throw new ArgumentException("User not found");
                
                user.Password = _cryptoProvider.EncodeValue(model.NewPassword);
                await context.SaveChangesAsync(cancellationToken).ConfigureAwait(false);
            }
        }

        /// <inheritdoc/>
        public async Task ChangeUserEmailAsync(Guid id, ChangeUserEmailModel model, CancellationToken cancellationToken)
        {
            using (var scope = _serviceProvider.CreateScope())
            using (var context = scope.ServiceProvider.GetRequiredService<DatabaseContext>())
            {
                var user = await context.Users.FirstOrDefaultAsync(x => x.Id == id, cancellationToken)
                    .ConfigureAwait(false);

                if (user == null)
                    throw new ArgumentException("User not found");

                if (model.NewEmail == user.Email || model.OldEmail == model.NewEmail)
                    throw new InvalidDataException("Emails are equal.");

                user.Email = model.NewEmail;
                await context.SaveChangesAsync(cancellationToken).ConfigureAwait(false);
            }
        }
    }
}
