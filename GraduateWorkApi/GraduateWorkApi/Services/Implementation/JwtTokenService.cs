using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using GraduateWorkApi.Configurations;
using GraduateWorkApi.Services.Abstractions;
using Microsoft.IdentityModel.Tokens;

namespace GraduateWorkApi.Services.Implementation
{
    public class JwtTokenService : IJwtTokenService
    {
        public JwtSecurityToken GenerateJwtTokenTask(string userId)
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimsIdentity.DefaultNameClaimType, userId),
            };

            var identity = new ClaimsIdentity
                (claims, "Token", ClaimsIdentity.DefaultNameClaimType, ClaimsIdentity.DefaultRoleClaimType);

            return new JwtSecurityToken(
                issuer: JwtTokenConfiguration.Issuer,
                audience: JwtTokenConfiguration.Audience,
                notBefore: DateTime.UtcNow,
                claims: identity.Claims,
                expires: DateTime.UtcNow.Add(TimeSpan.FromDays(JwtTokenConfiguration.LifeTime)),
                signingCredentials: new SigningCredentials(JwtTokenConfiguration.GetSymmetricSecurityKey(), SecurityAlgorithms.HmacSha256));
        }
    }
}
