using System.Threading.Tasks;
using ChatBot.Logic.Services;
using Microsoft.AspNetCore.Mvc;

namespace ChatBot.Api.Controllers
{
    [Route("/")]
    [Produces("application/json")]
    [ApiController]
    public class ViberCallbackController : ControllerBase
    {
        private readonly IViberCallbackService _viberCallbackService;

        public ViberCallbackController(IViberCallbackService viberCallbackService)
        {
            _viberCallbackService = viberCallbackService;
        }

        [HttpPost]
        public async Task<IActionResult> CatchCallbackAsync([FromBody] object data)
        {
            var response = await _viberCallbackService.ProccessCallbackMessageAsync(data.ToString()).ConfigureAwait(false);

            if (response != null)
            {
                return Ok(response);
            }
            return Ok();
        }
    }
}