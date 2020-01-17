using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using ChatBot.Models.Options;
using ChatBot.Models.Requests.Viber;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;

namespace ChatBot.Logic.RestClients
{
    public class ViberRestClient
    {
        private readonly ViberApiOptions _apiOptions;

        public ViberRestClient(IOptions<ViberApiOptions> apiOptions)
        {
            _apiOptions = apiOptions.Value;
        }

        public async Task<bool> SendMessage(string message, string receiverId)
        {
            var model = new SendMessageModel()
            {
                Receiver = receiverId,
                Sender = new ViberSenderModel { Name = "FCIT Computer Science Bot" },
                Text = message,
                Type = "text"
            };

            using (var client = new HttpClient())
            {
                client.DefaultRequestHeaders.Add("X-Viber-Auth-Token", _apiOptions.AccessKey);

                var content = new StringContent(JsonConvert.SerializeObject(model), Encoding.UTF8);
                var response = await client.PostAsync(_apiOptions.Url + "/pa/send_message", content);

                //TODO Chat it!!!
                if (!response.IsSuccessStatusCode)
                    return false;

                return true;
            }
        }
    }
}
