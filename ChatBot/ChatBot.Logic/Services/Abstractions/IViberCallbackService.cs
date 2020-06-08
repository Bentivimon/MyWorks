using ChatBot.Models.Responses.Viber;
using System.Threading.Tasks;
using ChatBot.Models.Callbacks.Viber;

namespace ChatBot.Logic.Services
{
    public interface IViberCallbackService
    {
        Task<ConversationStartedResponse> ProccessCallbackMessageAsync(string callbackMessage);

        Task HandleUserMessageAsync(ReceiveMessageFromUserCallback callback);
    }
}
