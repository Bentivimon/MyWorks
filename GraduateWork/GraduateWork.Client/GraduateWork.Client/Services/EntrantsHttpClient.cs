using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using GraduateWork.Client.Models.ResponseModels;
using Newtonsoft.Json;

namespace GraduateWork.Client.Services
{
    public class EntrantsHttpClient
    {
        public async Task<List<EntrantDto>> GetEntrantsBySpecialityIdAsync(string accessToken, Guid specialityId)
        {
            using (var client = new HttpClient())
            {
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
                var requestUri = Consts.BaseUrl + $"v1/Entrant/speciality?specialityId={specialityId}";

                var response = await client.GetAsync(requestUri).ConfigureAwait(false);

                if (response.IsSuccessStatusCode)
                {
                    var body = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
                    var result = JsonConvert.DeserializeObject<List<EntrantDto>>(body);
                    return result;
                }

                return null;
            }
        }

        public async Task<EntrantDto> GetEntrantByIdAsync(string accessToken, Guid entrantId)
        {
            using (var client = new HttpClient())
            {
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
                var requestUri = Consts.BaseUrl + $"v1/Entrant/byId?entrantId={entrantId}";

                var response = await client.GetAsync(requestUri).ConfigureAwait(false);

                if (response.IsSuccessStatusCode)
                {
                    var body = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
                    var result = JsonConvert.DeserializeObject<EntrantDto>(body);
                    return result;
                }

                return null;
            }
        }


        public async Task<List<ShortEntrantDto>> GetEntrantsAsync(string accessToken)
        {
            using (var client = new HttpClient())
            {
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
                var requestUri = Consts.BaseUrl + $"v1/Entrant/pagination?skip=0&take=1000";

                var response = await client.GetAsync(requestUri).ConfigureAwait(false);

                if (response.IsSuccessStatusCode)
                {
                    var body = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
                    var result = JsonConvert.DeserializeObject<List<ShortEntrantDto>>(body);
                    return result;
                }

                return null;
            }
        }

        public async Task<List<EntrantDto>> GetEntrantsByNameAsync(string accessToken, string name)
        {
            using (var client = new HttpClient())
            {
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
                var requestUri = Consts.BaseUrl + $"v1/Entrant/pagination/filter?skip=0&take=1000&name={name}";

                var response = await client.GetAsync(requestUri).ConfigureAwait(false);

                if (response.IsSuccessStatusCode)
                {
                    var body = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
                    var result = JsonConvert.DeserializeObject<List<EntrantDto>>(body);
                    return result;
                }

                return null;
            }
        }

        public async Task CombineEntrantAndUserAsync(string accessToken, Guid entrantId)
        {
            using (var client = new HttpClient())
            {
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
                var requestUri = Consts.BaseUrl + $"v1/Entrant/combine?entrantId={entrantId}";

                await client.PutAsync(requestUri, null).ConfigureAwait(false);
            }
        }
    }
}
