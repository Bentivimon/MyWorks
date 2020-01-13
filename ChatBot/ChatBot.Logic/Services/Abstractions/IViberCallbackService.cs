using ChatBot.Data.Responses.Viber;
using System.Threading.Tasks;

namespace ChatBot.Logic.Services
{
    public interface IViberCallbackService
    {
        Task<ConversationStartedResponse> ProccessCallbackMessageAsync(string callbackMessage);
    }
}
