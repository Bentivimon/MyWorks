using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using ChatBot.Data.Callbacks.Viber;
using ChatBot.Data.Responses.Viber;
using Newtonsoft.Json;

namespace ChatBot.Logic.Services
{
    public class ViberCallbackService : IViberCallbackService
    {
        public async Task<ConversationStartedResponse> ProccessCallbackMessageAsync(string callbackMessage)
        {
            var baseCallback = JsonConvert.DeserializeObject<BaseCallback>(callbackMessage);

            switch (baseCallback.Event)
            {
                case "subscribed":
                    {
                        var model = JsonConvert.DeserializeObject<SubscribedCallback>(callbackMessage);
                        //TODO Do something.
                        break;
                    }
                case "unsubscribed":
                    {
                        var model = JsonConvert.DeserializeObject<UnsubscribedCallback>(callbackMessage);
                        //TODO Do something.
                        break;
                    }
                case "conversation_started":
                    {
                        var model = JsonConvert.DeserializeObject<ConversationStartedCallback>(callbackMessage);

                        return new ConversationStartedResponse()
                        {
                            Sender = new Data.Requests.Viber.ViberSenderModel() { Name = "FCIT Computer Science Bot" },
                            Type = "text",
                            Text = "This is Welcomed Message."
                        };
                    }
                case "message":
                    {
                        var model = JsonConvert.DeserializeObject<ReceiveMessageFromUserCallback>(callbackMessage);
                        //TODO Do something
                        break;
                    }
                default:
                    return null;
            }

            return null;
        }
    }
}