using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Neighbors.Controllers
{
    [Route("Error")]
    public class ErrorController : Controller
    {

        public ErrorController()
        {
        }

        [Route("")]
        [Route("/Error")]
        public IActionResult OurError()
        {
            return View();
        }

    }
}
