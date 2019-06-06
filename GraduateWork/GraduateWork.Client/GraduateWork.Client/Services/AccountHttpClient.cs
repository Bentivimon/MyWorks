using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using GraduateWork.Client.Models.RequestModels;
using GraduateWork.Client.Models.ResponseModels;
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

        public async Task<UserInfo> GetUserInfo(string token)
        {
            using (var client = new HttpClient())
            {
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                var requestUri = Consts.BaseUrl + "v1/Account/userInfo";

                var response = await client.GetAsync(requestUri).ConfigureAwait(false);

                if (response.IsSuccessStatusCode)
                {
                    var body = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
                    var result = JsonConvert.DeserializeObject<UserInfo>(body);
                    return result;
                }

                return null;
            }
        }
    }
}
