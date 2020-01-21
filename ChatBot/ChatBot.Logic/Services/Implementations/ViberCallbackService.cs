using System;
using System.Threading.Tasks;
using ChatBot.Data;
using ChatBot.Data.Entities;
using ChatBot.Logic.RestClients;
using ChatBot.Models.Callbacks.Viber;
using ChatBot.Models.Responses.Viber;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

namespace ChatBot.Logic.Services
{
    public class ViberCallbackService : IViberCallbackService
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly ViberRestClient _viberRestClient;

        public ViberCallbackService(ApplicationDbContext dbContext, ViberRestClient viberRestClient)
        {
            _dbContext = dbContext;
            _viberRestClient = viberRestClient;
        }

        public async Task<ConversationStartedResponse> ProccessCallbackMessageAsync(string callbackMessage)
        {
            var baseCallback = JsonConvert.DeserializeObject<BaseCallback>(callbackMessage);

            switch (baseCallback.Event)
            {
                case "subscribed":
                    {
                        var model = JsonConvert.DeserializeObject<SubscribedCallback>(callbackMessage);
                        await SubscribeUserAsync(model).ConfigureAwait(false);
                        break;
                    }
                case "unsubscribed":
                    {
                        var model = JsonConvert.DeserializeObject<UnsubscribedCallback>(callbackMessage);
                        await UnsubscribeUserAsync(model).ConfigureAwait(false);
                        break;
                    }
                case "conversation_started":
                    {
                        var model = JsonConvert.DeserializeObject<ConversationStartedCallback>(callbackMessage);

                        return new ConversationStartedResponse()
                        {
                            Sender = new Models.Requests.Viber.ViberSenderModel() { Name = "FCIT Computer Science Bot" },
                            Type = "text",
                            Text = "This is Welcomed Message."
                        };
                    }
                case "message":
                    {
                        var model = JsonConvert.DeserializeObject<ReceiveMessageFromUserCallback>(callbackMessage);
                        await HandleUserMessageAsync(model).ConfigureAwait(false);
                        break;
                    }
                default:
                    return null;
            }

            return null;
        }

        private async Task SubscribeUserAsync(SubscribedCallback callback)
        {
            var now = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();

            var entity = new ViberUserEntity()
            {
                Avatart = callback.User.Avatart,
                Country = callback.User.Country,
                CreatedTimestamp = now,
                UpdatedTimestamp = now,
                IsSubscribed = true,
                Language = callback.User.Language,
                Name = callback.User.Name,
                ViberId = callback.User.Id
            };

            await _dbContext.ViberUsers.AddAsync(entity).ConfigureAwait(false);
            await _dbContext.SaveChangesAsync().ConfigureAwait(false);
        }

        private async Task UnsubscribeUserAsync(UnsubscribedCallback callback)
        {
            var entity = await _dbContext.ViberUsers.FirstOrDefaultAsync(x => x.ViberId == callback.UserId)
                .ConfigureAwait(false);

            if (entity == null)
                return;
            
            entity.IsSubscribed = false;
            entity.UpdatedTimestamp = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();

            await _dbContext.SaveChangesAsync().ConfigureAwait(false);
        }

        private async Task HandleUserMessageAsync(ReceiveMessageFromUserCallback callback)
        {
            await SaveUserMessageAsync(callback).ConfigureAwait(false);

            await _viberRestClient.SendMessage(
                $"You send me this message '{callback.Message.Text}'. Thank you))) I'm learning (smiley)", callback.Sender.Id);
        }

        private async Task SaveUserMessageAsync(ReceiveMessageFromUserCallback callback)
        {
            var senderEntity = await _dbContext.ViberUsers.FirstOrDefaultAsync(x => x.ViberId == callback.Sender.Id);

            if (senderEntity == null)
            {
                var now = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();

                var userEntity = new ViberUserEntity()
                {
                    Avatart = callback.Sender.Avatart,
                    Country = callback.Sender.Country,
                    CreatedTimestamp = now,
                    UpdatedTimestamp = now,
                    IsSubscribed = true,
                    Language = callback.Sender.Language,
                    Name = callback.Sender.Name,
                    ViberId = callback.Sender.Id
                };
                await _dbContext.ViberUsers.AddAsync(userEntity).ConfigureAwait(false);
                await _dbContext.SaveChangesAsync().ConfigureAwait(false);

                senderEntity = userEntity;
            }

            var messageEntity = new ViberUserMessageEntity()
            {
                Message = callback.Message.Text,
                MessageType = callback.Message.Type,
                UserId = senderEntity.Id
            };

            await _dbContext.ViberUserMessages.AddAsync(messageEntity).ConfigureAwait(false);
            await _dbContext.SaveChangesAsync().ConfigureAwait(false);
        }
    }
}