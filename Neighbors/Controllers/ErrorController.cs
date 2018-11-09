using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Neighbors.Controllers
{
    [Route("Error")]
	[AllowAnonymous]
    public class ErrorController : Controller
    {

        public ErrorController()
        {
        }
        [HttpGet("/Error/{errorId}")]
        public IActionResult InvalidAction(int errorId)
        {
            if (errorId == 401 || errorId == 403)
                return View("Views/Error/InvalidAction.cshtml");
            return OurError();
        }

        [Route("")]
        [Route("/Error")]
        public IActionResult OurError()
        {
            return View("Views/Error/OurError.cshtml");
        }

        [Route("/Error/InvalidAction")]
        public IActionResult InvalidAction()
        {
            return View("Views/Error/InvalidAction.cshtml");
        }
    }
}
