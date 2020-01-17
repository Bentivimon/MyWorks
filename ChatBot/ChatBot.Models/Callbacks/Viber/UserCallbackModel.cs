using Newtonsoft.Json;

namespace ChatBot.Models.Callbacks.Viber
{
    public class UserCallbackModel
    {
        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("avatar")]
        public string Avatart { get; set; }

        [JsonProperty("country")]
        public string Country { get; set; }

        [JsonProperty("language")]
        public string Language { get; set; }

        [JsonProperty("api_version")]
        public int ApiVersion { get; set; }
    }   
}
