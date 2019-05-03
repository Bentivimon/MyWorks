using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using GraduateWork.Client.Models.ResponseModels;
using Newtonsoft.Json;

namespace GraduateWork.Client.Services
{
    public class SpecialitiesHttpClient
    {
        public async Task<List<SpecialityDto>> GetSpecialitiesByUniversityIdAsync(string accessToken, Guid universityId)
        {
            using (var client = new HttpClient())
            {
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
                var requestUri = Consts.BaseUrl + $"v1/Speciality/pagination/byId?universityId={universityId}";

                var response = await client.GetAsync(requestUri).ConfigureAwait(false);

                if (response.IsSuccessStatusCode)
                {
                    var body = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
                    var result = JsonConvert.DeserializeObject<List<SpecialityDto>>(body);
                    return result;
                }

                return null;
            }
        }
    }
}
