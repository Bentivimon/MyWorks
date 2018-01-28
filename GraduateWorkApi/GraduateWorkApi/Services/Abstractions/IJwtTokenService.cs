using System.IdentityModel.Tokens.Jwt;

namespace GraduateWorkApi.Services.Abstractions
{
    public interface IJwtTokenService
    {
        JwtSecurityToken GenerateJwtTokenTask(string userId);
    }
}
