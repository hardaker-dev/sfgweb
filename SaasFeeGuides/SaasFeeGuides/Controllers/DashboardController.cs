using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SaasFeeGuides.ViewModels;

namespace SaasFeeGuides.Controllers
{
    [Authorize("User")]
    [Route("api/[controller]")]
    [ApiController]
    public class DashboardController : ControllerBase
    {
        public DashboardController()
        {

        }
        [HttpGet("index")]
        public IActionResult GetIndex()
        {
            return new OkObjectResult(new DashboardIndex { Message = "This is secure data!" });
        }
    }
}