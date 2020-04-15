using Newtonsoft.Json;

namespace ChatBot.Models.Callbacks.Viber
{
    public class SubscribedCallback : BaseCallback
    {
        [JsonProperty("user")]
        public UserCallbackModel User { get; set; }
    }
}
    