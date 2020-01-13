using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace ChatBot.Api.Controllers
{
    [Route("/")]
    [ApiController]
    public class ViberCallbackController : ControllerBase
    {

        [HttpPost]
        public async Task<IActionResult> CatchCallbackAsync([FromBody] object data)
        {
            var k = data;

            return Ok();
        }
    }
}