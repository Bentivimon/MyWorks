using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using ChatBot.Models.Options;
using ChatBot.Models.Requests.Dialogflow;
using ChatBot.Models.Responses.Dialogflow;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;

namespace ChatBot.Logic.RestClients
{
    public class DialogflowRestClient
    {
        private readonly DialogflowApiOptions _dialogflowApiOptions;

        public DialogflowRestClient(IOptions<DialogflowApiOptions> dialogflowApiOptions)
        {
            _dialogflowApiOptions = dialogflowApiOptions.Value;
        }

        public async Task<string> SendQueryRequestAsync(string message, string language = "uk",
            CancellationToken cancellationToken = default)
        {
            var model = new SendQueryRequest
            {
                Language = language,
                Query = message
            };

            using (var client = new HttpClient())
            {
                client.DefaultRequestHeaders.Add("Authorization", $"Bearer {_dialogflowApiOptions.Token}");
                
                var content = new StringContent(JsonConvert.SerializeObject(model), Encoding.UTF8);
                var response = await client.PostAsync(_dialogflowApiOptions.Url, content, cancellationToken);

                var parsedResponse =
                    JsonConvert.DeserializeObject<SendQueryResponse>(await response.Content.ReadAsStringAsync());

                return parsedResponse.Result.Fulfillment.Speech;
            }
        }
    }
}
