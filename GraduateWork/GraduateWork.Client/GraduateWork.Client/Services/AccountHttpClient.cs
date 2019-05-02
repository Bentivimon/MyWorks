using System;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using GraduateWork.Client.Models.RequestModels;
using Newtonsoft.Json;

namespace GraduateWork.Client.Services
{
    public class AccountHttpClient
    {
        private const string BaseUrl = "https://123.495.23.45:5000/";

        public async Task<string> LoginAsync(UserLoginModel model)
        {
            using (var client = new HttpClient())
            {
                var requestUri = BaseUrl + "v1/Account/token";
                var jsonValue = JsonConvert.SerializeObject(model);

                var response = await client.PostAsync(requestUri, new StringContent(jsonValue)).ConfigureAwait(false);

                if (response.IsSuccessStatusCode)
                {
                    var body = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
                    var result = JsonConvert.DeserializeObject<string>(body);
                    return result;
                }

                return null;
            }
        }

        public async Task RegistrationAsync(RegistrationModel model)
        {
            using (var client = new HttpClient())
            {
                var requestUri = BaseUrl + "v1/Account/registration";
                var jsonValue = JsonConvert.SerializeObject(model);

                var response = await client.PostAsync(requestUri, new StringContent(jsonValue)).ConfigureAwait(false);

                if (!response.IsSuccessStatusCode)
                    throw new Exception();
            }
        }
    }
}
