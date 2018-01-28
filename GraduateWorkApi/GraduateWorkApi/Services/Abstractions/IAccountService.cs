using System.IdentityModel.Tokens.Jwt;
using System.Threading.Tasks;
using Models.DTOModels.UserModels;
using Models.RequestModels.UserModels;

namespace GraduateWorkApi.Services.Abstractions
{
    public interface IAccountService
    {
        Task<string> LogisTask(UserLoginModelRequest loginModel);
        Task<bool> RegisterTask(UserRegistrarionModelRequest registrarionModel);
        Task<UserDtoModel> GetUserModelTask(string identityName);
        Task EditUserTask(string identityName, UserDtoModel userModel);
        Task<bool> ChangeUserPasswordTask(string identityName, ChangeUserPasswordRequest model);
        Task<bool> ChangeUserEmailTask(string identityName, ChangeUserEmailRequest model);
    }
}
