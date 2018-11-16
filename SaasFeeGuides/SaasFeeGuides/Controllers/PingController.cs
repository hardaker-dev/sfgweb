using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace SaasFeeGuides.Controllers
{
    [Route("api/[controller]")]
    public class PingController : Controller
    {
       
        [HttpGet]
        public async Task<IActionResult> Ping()
        {
            return await Task.FromResult(new ObjectResult("pong"));
        }

        
    }
}
