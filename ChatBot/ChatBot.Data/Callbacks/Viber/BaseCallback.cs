using Newtonsoft.Json;

namespace ChatBot.Data.Callbacks.Viber
{
    public class BaseCallback
    {
        [JsonProperty("event")]
        public string Event { get; set; }

        [JsonProperty("timestamp")]
        public long Timestamp { get; set; }

        [JsonProperty("message_token")]
        public long MeesageToken { get; set; }
    }
}
