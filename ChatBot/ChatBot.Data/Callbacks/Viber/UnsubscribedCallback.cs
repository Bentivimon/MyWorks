﻿using Newtonsoft.Json;

namespace ChatBot.Data.Callbacks.Viber
{
    public class UnsubscribedCallback : BaseCallback
    {
        [JsonProperty("user_id")]
        public string UserId { get; set; }
    }
}
