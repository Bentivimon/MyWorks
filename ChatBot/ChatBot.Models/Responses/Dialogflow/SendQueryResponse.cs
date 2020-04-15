using Newtonsoft.Json;

namespace ChatBot.Models.Responses.Dialogflow
{
    public class SendQueryResponse
    {
        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("lang")]
        public string Language { get; set; }

        [JsonProperty("query")]
        public string Query { get; set; }

        [JsonProperty("sessionId")]
        public string SessionId { get; set; }
        
        [JsonProperty("timestamp")]
        public string Timestamp { get; set; }

        [JsonProperty("result")]
        public SendQueryResultResponse Result { get; set; }

        [JsonProperty("status")]
        public SendQueryStatusResponse Status { get; set; }
    }

    public class SendQueryStatusResponse
    {
        [JsonProperty("code")]
        public int Code { get; set; }

        [JsonProperty("errorType")]
        public string ErrorType { get; set; }
    }

    public class SendQueryResultResponse
    {
        [JsonProperty("source")]
        public string Source { get; set; }

        [JsonProperty("resolvedQuery")]
        public string ResolvedQuery { get; set; }

        [JsonProperty("action")]
        public string Action { get; set; }

        [JsonProperty("actionIncomplete")]
        public bool ActionIncomplete { get; set; }

        [JsonProperty("score")]
        public double Score { get; set; }

        [JsonProperty("parameters")]
        public object Parameters { get; set; }

        [JsonProperty("contexts")]
        public object[] Contexts { get; set; }

        [JsonProperty("metadata")]
        public object Metadata { get; set; }

        [JsonProperty("fulfillment")]
        public FulfillmentResponse Fulfillment { get; set; }
    }

    public class FulfillmentResponse
    {
        [JsonProperty("speech")]
        public string Speech { get; set; }

        [JsonProperty("messages")]
        public object[] Messages { get; set; }
    }
}
