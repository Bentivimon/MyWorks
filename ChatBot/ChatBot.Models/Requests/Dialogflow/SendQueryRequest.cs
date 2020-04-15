using Newtonsoft.Json;

namespace ChatBot.Models.Requests.Dialogflow
{
    public class SendQueryRequest
    {
        [JsonProperty("lang")]
        public string Language { get; set; }

        [JsonProperty("query")]
        public string Query { get; set; }

        [JsonProperty("sessionId")]
        public string SessionId { get; set; } = "123456";
    }
}
