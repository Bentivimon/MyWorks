using System;
using System.Threading.Tasks;
using ChatBot.Data;
using ChatBot.Data.DataStores;
using ChatBot.Data.Entities;
using ChatBot.Logic.Factories;
using ChatBot.Logic.RestClients;
using ChatBot.Models.Callbacks.Viber;
using ChatBot.Models.Responses.Viber;
using Newtonsoft.Json;

namespace ChatBot.Logic.Services
{
    public class ViberCallbackService : IViberCallbackService
    {
        private readonly ApplicationDbContext _dbContext;

        private readonly ViberUserDataStore _userDataStore;
        private readonly ViberUserMessageDataStore _messageDataStore;
        private readonly ViberRestClient _viberRestClient;

        private readonly DialogflowRestClient _dialogflowRestClient;
        private readonly DialogflowResultDataStore _dialogflowResultDataStore;

        public ViberCallbackService(ApplicationDbContext dbContext, ViberRestClient viberRestClient,
            ViberUserDataStore userDataStore, ViberUserMessageDataStore messageDataStore,
            DialogflowRestClient dialogflowRestClient, DialogflowResultDataStore dialogflowResultDataStore)
        {
            _dbContext = dbContext;
            _viberRestClient = viberRestClient;
            _userDataStore = userDataStore;
            _messageDataStore = messageDataStore;
            _dialogflowRestClient = dialogflowRestClient;
            _dialogflowResultDataStore = dialogflowResultDataStore;
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

                        return new ConversationStartedResponse
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
            if (await _userDataStore.ExistAsync(x=> x.ViberId == callback.User.Id).ConfigureAwait(false))
                return;

            var entity = ViberUserFactory.ToEntity(callback);

            await _userDataStore.CreateAsync(entity).ConfigureAwait(false);
            await _userDataStore.SaveAsync().ConfigureAwait(false);
        }

        private async Task UnsubscribeUserAsync(UnsubscribedCallback callback)
        {
            var user = await _userDataStore.FindAsync(x => x.ViberId == callback.UserId).ConfigureAwait(false);

            if (user == null)
                return;

            user.IsSubscribed = false;
            user.UpdatedTimestamp = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();

            _userDataStore.Update(user);
            await _userDataStore.SaveAsync().ConfigureAwait(false);
        }

        private async Task HandleUserMessageAsync(ReceiveMessageFromUserCallback callback)
        {
            await SaveUserMessageAsync(callback).ConfigureAwait(false);

            var response = await _dialogflowRestClient.SendQueryRequestAsync(callback.Message.Text);

            await _dialogflowResultDataStore.CreateAsync(new DialogflowResultEntity()
                {Request = callback.Message.Text, Response = response}).ConfigureAwait(false);
            await _dialogflowResultDataStore.SaveAsync().ConfigureAwait(false);

            await _viberRestClient.SendMessage(
                $"{response}", callback.Sender.Id).ConfigureAwait(false);
        }

        private async Task SaveUserMessageAsync(ReceiveMessageFromUserCallback callback)
        {
            var senderEntity = await _userDataStore.FindAsync(x => x.ViberId == callback.Sender.Id).ConfigureAwait(false);

            if (senderEntity == null)
            {
                var userEntity = ViberUserFactory.ToEntity(callback);

                await _userDataStore.CreateAsync(userEntity).ConfigureAwait(false);
                await _dbContext.SaveChangesAsync().ConfigureAwait(false);

                senderEntity = userEntity;
            }

            var messageEntity = ViberUserMessageFactory.ToEntity(callback, senderEntity.Id);

            await _messageDataStore.CreateAsync(messageEntity).ConfigureAwait(false);
            await _messageDataStore.SaveAsync().ConfigureAwait(false);
        }
    }
}