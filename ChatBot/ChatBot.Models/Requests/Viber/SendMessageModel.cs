using Newtonsoft.Json;

namespace ChatBot.Models.Requests.Viber
{
    public class SendMessageModel
    {
        [JsonProperty("receiver")]
        public string Receiver { get; set; }

        [JsonProperty("type")]
        public string Type { get; set; }

        [JsonProperty("text")]
        public string Text { get; set; }

        [JsonProperty("sender")]
        public ViberSenderModel Sender { get; set; }
    }

    public class ViberSenderModel
    {
        [JsonProperty("name")]
        public string Name { get; set; }

        // OPTIONAL!!!
        //public string Avatar { get; set; }
    }
}
