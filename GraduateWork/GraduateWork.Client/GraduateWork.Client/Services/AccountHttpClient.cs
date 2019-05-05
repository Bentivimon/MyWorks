using System;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using GraduateWork.Client.Models.RequestModels;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace GraduateWork.Client.Services
{
    public class AccountHttpClient
    {
        public async Task<string> LoginAsync(UserLoginModel model)
        {
            using (var client = new HttpClient())
            {
                var requestUri = Consts.BaseUrl + "v1/Account/token";
                var jsonValue = JsonConvert.SerializeObject(model, new JsonSerializerSettings
                {
                    ContractResolver = new CamelCasePropertyNamesContractResolver()
                });

                var response = await client.PostAsync(requestUri, new StringContent(jsonValue, Encoding.UTF8, "application/json")).ConfigureAwait(false);

                if (response.IsSuccessStatusCode)
                {
                    var body = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
                    var result = JsonConvert.DeserializeObject<string>(body);
                    return result;
                }

                return null;
            }
        }

        public async Task<bool> RegistrationAsync(RegistrationModel model)
        {
            using (var client = new HttpClient())
            {
                var requestUri = Consts.BaseUrl + "v1/Account/registration";
                var jsonValue = JsonConvert.SerializeObject(model, new JsonSerializerSettings
                {
                    ContractResolver = new CamelCasePropertyNamesContractResolver()
                });

                var response = await client.PostAsync(requestUri, new StringContent(jsonValue, Encoding.UTF8, "application/json")).ConfigureAwait(false);

                if (!response.IsSuccessStatusCode)
                    return false;

                return true;
            }
        }
    }
}
