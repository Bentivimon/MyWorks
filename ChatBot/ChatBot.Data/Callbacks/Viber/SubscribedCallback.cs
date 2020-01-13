using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace ChatBot.Data.Callbacks.Viber
{
    public class SubscribedCallback : BaseCallback
    {
        [JsonProperty("user")]
        public UserCallbackModel User { get; set; }
    }
}
    