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
        [HttpGet("/Error/{errorId}")]
        public IActionResult InvalidAction(int errorId)
        {
            if (errorId == 401 || errorId == 403)
                return View();
            return OurError();
        }

        [Route("")]
        [Route("/Error")]
        public IActionResult OurError()
        {
            return View("Views/Error/OurError.cshtml");
        }

    }
}
