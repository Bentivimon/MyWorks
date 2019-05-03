using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using GraduateWork.Client.Models.ResponseModels;
using Newtonsoft.Json;

namespace GraduateWork.Client.Services
{
    public class UniversitiesHttpClient
    {
        public async Task<List<UniversityDto>> GetUniversitiesByRegionIdAsync(string accessToken, int regionId)
        {
            using (var client = new HttpClient())
            {
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
                var requestUri = Consts.BaseUrl + $"v1/University/region?regionId={regionId}";

                var response = await client.GetAsync(requestUri).ConfigureAwait(false);

                if (response.IsSuccessStatusCode)
                {
                    var body = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
                    var result = JsonConvert.DeserializeObject<List<UniversityDto>>(body);
                    return result;
                }

                return null;
            }
        }
    }
}
