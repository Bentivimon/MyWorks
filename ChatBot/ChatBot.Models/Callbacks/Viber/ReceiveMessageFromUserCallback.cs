using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace ChatBot.Models.Callbacks.Viber
{
    public class ReceiveMessageFromUserCallback : BaseCallback
    {
        [JsonProperty("sender")]
        public UserCallbackModel Sender { get; set; }

        [JsonProperty("message")]
        public ReceiveMessageModel Message { get; set; }
    }

    public class ReceiveMessageModel
    {
        [JsonProperty("type")]
        public string Type { get; set; }

        [JsonProperty("text")]
        public string Text { get; set; }

        [JsonProperty("media")]
        public string Media { get; set; }

        [JsonProperty("location")]
        public Location Location { get; set; }

        [JsonProperty("tracking_data")]
        public string TrackingData { get; set; }
    }

    public class Location
    {
        [JsonProperty("lat")]
        public double Lat { get; set; }

        [JsonProperty("lot")]
        public double Lot { get; set; }
    }
}
