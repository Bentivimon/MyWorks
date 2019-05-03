using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using GraduateWork.Client.Models.ResponseModels;
using Newtonsoft.Json;

namespace GraduateWork.Client.Services
{
    public class RegionsHttpClient
    {
        public async Task<List<RegionDto>> GetAllRegionsAsync(string accessToken)
        {
            using (var client = new HttpClient())
            {
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
                var requestUri = Consts.BaseUrl + "v1/Region";

                var response = await client.GetAsync(requestUri).ConfigureAwait(false);

                if (response.IsSuccessStatusCode)
                {
                    var body = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
                    var result = JsonConvert.DeserializeObject<List<RegionDto>>(body);
                    return result;
                }

                return null;
            }
        }
    }
}
