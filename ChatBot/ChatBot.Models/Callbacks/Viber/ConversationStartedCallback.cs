﻿using Newtonsoft.Json;

namespace ChatBot.Models.Callbacks.Viber
{
    public class ConversationStartedCallback : BaseCallback
    {

        [JsonProperty("type")]
        public string Type { get; set; }

        [JsonProperty("context")]
        public string Context { get; set; }

        [JsonProperty("user")]
        public UserCallbackModel User { get; set; }

        [JsonProperty("subscribed")]
        public bool Subscribed { get; set; }
    }
}
