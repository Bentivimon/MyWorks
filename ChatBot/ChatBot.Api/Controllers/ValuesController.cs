using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ChatBot.Logic.RestClients;
using Microsoft.AspNetCore.Mvc;

namespace ChatBot.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ValuesController : ControllerBase
    {
        private readonly ViberRestClient _viberRestClient;

        public ValuesController(ViberRestClient viberRestClient)
        {
            _viberRestClient = viberRestClient;
        }

        // GET api/values
        [HttpPost("send")]
        public async Task<IActionResult> SendMessageAsync()
        {
            await _viberRestClient.SendMessage("NEw test message))))))", "EDsjO05Cz5eazCv14FxuFw==");

            return Ok();
        }
    }
}
