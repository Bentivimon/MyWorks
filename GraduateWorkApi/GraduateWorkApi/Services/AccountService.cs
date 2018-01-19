using System;
using System.Collections.Generic;
using Microsoft.Extensions.DependencyInjection;
using System.Threading.Tasks;
using EntityModels.Entitys;
using GraduateWorkApi.Abstractions;
using GraduateWorkApi.Context;
using Microsoft.EntityFrameworkCore;
using Models.RequestModels.UserModels;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using GraduateWorkApi.Configurations;
using Microsoft.IdentityModel.Tokens;
using Models.CustomExceptions;
using Models.DTOModels.UserModels;

namespace GraduateWorkApi.Services
{
    public class AccountService: IAccountService
    {
        private readonly IServiceProvider _serviceProvider;

        public AccountService(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public async Task<JwtSecurityToken> LogisTask(UserLoginModelRequest loginModel)
        {
            using (var contex = _serviceProvider.GetService<DatabaseContext>())
            {
                var user = await contex.Users
                    .AsNoTracking()
                    .FirstOrDefaultAsync(x => x.Email == loginModel.Login && x.Password == Md5CryptoProvider.Encoding(loginModel.Password));

                if(user == null)
                    throw new UserNotFoundException();

                var claims = new List<Claim>
                {
                    new Claim(ClaimsIdentity.DefaultNameClaimType, user.Id),
                   // new Claim(ClaimsIdentity.DefaultRoleClaimType, user.Role.ToString())
                };

                var identity = new ClaimsIdentity
                    (claims  , "Token", ClaimsIdentity.DefaultNameClaimType, ClaimsIdentity.DefaultRoleClaimType);
                return new JwtSecurityToken(
                    issuer: JwtTokenConfiguration.Issuer,
                    audience: JwtTokenConfiguration.Audience,
                    notBefore: DateTime.UtcNow,
                    claims: identity.Claims,
                    expires: DateTime.UtcNow.Add(TimeSpan.FromDays(JwtTokenConfiguration.LifeTime)),
                    signingCredentials: new SigningCredentials(JwtTokenConfiguration.GetSymmetricSecurityKey(), SecurityAlgorithms.HmacSha256));

            }
        }

        public async Task<bool> RegisterTask(UserRegistrarionModelRequest registrarionModel)
        {
            using (var context = _serviceProvider.GetService<DatabaseContext>())
            {
                var isEmailUsed = await context.Users
                    .AsNoTracking()
                    .AnyAsync(x => x.Email == registrarionModel.Email && x.MobileNumber == registrarionModel.MobileNumber);

                if (isEmailUsed)
                    return false;

                var userEntity = new UserEntity
                {
                    Birthday = registrarionModel.Birthday,
                    Email = registrarionModel.Email,
                    FirstName = registrarionModel.FirstName,
                    LastName = registrarionModel.LastName,
                    MobileNumber = registrarionModel.MobileNumber,
                    Password = Md5CryptoProvider.Encoding(registrarionModel.Password)
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

                if(Md5CryptoProvider.Encoding(model.NewPassword) == user.Password || model.OldPassword == model.NewPassword)
                    return false;

                user.Password = Md5CryptoProvider.Encoding(model.NewPassword);
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
