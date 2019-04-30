using System;
using System.IdentityModel.Tokens.Jwt;

namespace GraduateWork.Server.Services.Abstractions
{
    public interface IJwtTokenService
    {
        JwtSecurityToken GenerateJwtTokenAsync (Guid userId);
    }
}