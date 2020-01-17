using ChatBot.Models.Responses.Viber;
using System.Threading.Tasks;

namespace ChatBot.Logic.Services
{
    public interface IViberCallbackService
    {
        Task<ConversationStartedResponse> ProccessCallbackMessageAsync(string callbackMessage);
    }
}
