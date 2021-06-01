using Alice.ResponseLogic;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Yandex.Alice.Sdk.Models;

namespace Alice.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AliceController : ControllerBase
    {
        private readonly AliceHandler _aliceHandler;
        public AliceController(AliceHandler aliceHandler)
        {
            _aliceHandler = aliceHandler;
        }

        [HttpPost]
        [Route("post")]
        public IActionResult Get(AliceRequest aliceRequest)
        {
            return Ok(new AliceResponse(aliceRequest, _aliceHandler.Process(aliceRequest)));
        }
    }
}
