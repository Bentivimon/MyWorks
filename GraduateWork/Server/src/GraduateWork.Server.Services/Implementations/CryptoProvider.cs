using System;
using System.Globalization;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using GraduateWork.Server.Models;
using Microsoft.Extensions.Configuration;
using ICryptoProvider = GraduateWork.Server.Services.Abstractions.ICryptoProvider;

namespace GraduateWork.Server.Services.Implementations
{
    public class CryptoProvider : ICryptoProvider , IDisposable
    {
        private readonly HMACMD5 _crypto;

        public CryptoProvider(IConfiguration configuration)
        {
            _crypto = new HMACMD5(Encoding.UTF8.GetBytes(configuration.GetValue<string>(Consts.EncryptKey)));
        }

        public string EncodeValue(string value)
        {
            var byteArr = Encoding.UTF8.GetBytes(value);

            return string.Concat(_crypto.ComputeHash(byteArr).Select(x => x.ToString("x2", CultureInfo.InvariantCulture).ToLower(CultureInfo.InvariantCulture)));
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                _crypto?.Dispose();
            }
        }
    }
}
