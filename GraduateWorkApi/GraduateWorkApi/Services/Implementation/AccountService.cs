using System;
using Microsoft.Extensions.DependencyInjection;
using System.Threading.Tasks;
using EntityModels.Entitys;
using GraduateWorkApi.Context;
using Microsoft.EntityFrameworkCore;
using Models.RequestModels.UserModels;
using GraduateWorkApi.Services.Abstractions;
using Models.CustomExceptions;
using Models.DTOModels.UserModels;

namespace GraduateWorkApi.Services.Implementation
{
    public class AccountService: IAccountService
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly IMD5CryptoProvider _cryptoProvider;

        public AccountService(IServiceProvider serviceProvider, IMD5CryptoProvider cryptoProvider)
        {
            _serviceProvider = serviceProvider;
            _cryptoProvider = cryptoProvider;
        }

        public async Task<string> LogisTask(UserLoginModelRequest loginModel)
        {
            using (var contex = _serviceProvider.GetService<DatabaseContext>())
            {
                var user = await contex.Users
                    .AsNoTracking()
                    .FirstOrDefaultAsync(x => x.Email == loginModel.Login && x.Password == _cryptoProvider.Encoding(loginModel.Password));

                if(user == null)
                    throw new UserNotFoundException();

                return user.Id;
            }
        }

        public async Task<bool> RegisterTask(UserRegistrarionModelRequest registrarionModel)
        {
            using (var context = _serviceProvider.GetService<DatabaseContext>())
            {
                var isEmailOrPhoneUsed = await context.Users
                    .AsNoTracking()
                    .AnyAsync(x => x.Email == registrarionModel.Email || x.MobileNumber == registrarionModel.MobileNumber);

                if (isEmailOrPhoneUsed)
                    return false;

                var userEntity = new UserEntity
                {
                    Birthday = registrarionModel.Birthday,
                    Email = registrarionModel.Email,
                    FirstName = registrarionModel.FirstName,
                    LastName = registrarionModel.LastName,
                    MobileNumber = registrarionModel.MobileNumber,
                    Password = _cryptoProvider.Encoding(registrarionModel.Password)
                };

                await context.AddAsync(userEntity);
                await context.SaveChangesAsync();
            }

            return true;
        }
        
        public async Task<UserDtoModel> GetUserModelTask(string identityName)
        {
            using (var context = _serviceProvider.GetService<DatabaseContext>())
            {
                var userEntity = await context.Users
                    .AsNoTracking()
                    .FirstOrDefaultAsync(x => x.Id == identityName);

                if (userEntity == null)
                    throw new UserNotFoundException();

                return new UserDtoModel()
                {
                    Birthday = userEntity.Birthday,
                    Email = userEntity.Email,
                    FirstName = userEntity.FirstName,
                    LastName = userEntity.LastName,
                    MobileNumber = userEntity.MobileNumber
                };
            }
        }

        public async Task EditUserTask(string identityName, UserDtoModel userModel)
        {
            using (var context = _serviceProvider.GetService<DatabaseContext>())
            {
                var user = await context.Users.FirstOrDefaultAsync(x => x.Id == identityName);

                if(user == null)
                    throw new UserNotFoundException();

                user.Birthday = userModel.Birthday;
                user.FirstName = userModel.FirstName;
                user.MobileNumber = userModel.MobileNumber;
                user.LastName = userModel.LastName;

                await context.SaveChangesAsync();
            }
        }

        public async Task<bool> ChangeUserPasswordTask(string identityName, ChangeUserPasswordRequest model)
        {
            using (var context = _serviceProvider.GetService<DatabaseContext>())
            {
                var user = await context.Users.FirstOrDefaultAsync(x => x.Id == identityName);

                if(user == null)
                    throw new UserNotFoundException();

                if(_cryptoProvider.Encoding(model.OldPassword) != user.Password || model.OldPassword == model.NewPassword)
                    return false;

                user.Password = _cryptoProvider.Encoding(model.NewPassword);
                await context.SaveChangesAsync();
            }

            return true;
        }

        public async Task<bool> ChangeUserEmailTask(string identityName, ChangeUserEmailRequest model)
        {
            using (var context = _serviceProvider.GetService<DatabaseContext>())
            {
                var user = await context.Users.FirstOrDefaultAsync(x => x.Id == identityName);

                if (user == null)
                    throw new UserNotFoundException();

                if (model.NewEmail == user.Email || model.OldEmail == model.NewEmail)
                    return false;

                user.Email = model.NewEmail;
                await context.SaveChangesAsync();
            }

            return true;
        }
    }
}
