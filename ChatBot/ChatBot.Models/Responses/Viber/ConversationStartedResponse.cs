using ChatBot.Models.Requests.Viber;
using Newtonsoft.Json;

namespace ChatBot.Models.Responses.Viber
{
    public class ConversationStartedResponse
    {
        [JsonProperty("type")]
        public string Type { get; set; }

        [JsonProperty("tracking_data")]
        public string TrackingData { get; set; }

        [JsonProperty("text")]
        public string Text { get; set; }

        [JsonProperty("sender")]
        public ViberSenderModel Sender { get; set; }
    }
}
