using System.Text;
using Microsoft.IdentityModel.Tokens;

namespace GraduateWork.Server.Models.Configurations
{
    public static class JwtTokenConfiguration
    {
        public const string Issuer = "Graduate Work";
        public const string Audience = "Graduate Work Users";
        private const string Key = "1qitydntj4683QWQEH23fdd";
        public const int LifeTime = 360;

        public static SymmetricSecurityKey GetSymmetricSecurityKey()
        {
            return new SymmetricSecurityKey(Encoding.ASCII.GetBytes(Key));
        }
    }
}
