using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Web.Models;
using Application.Home.Queries.GetAboutInfo;

namespace Web.Controllers
{
    public class HomeController : BaseController
    {
        public HomeController()
        {
        }

        public IActionResult Index()
        {
            return View();
        }

        public async Task<ActionResult> About()
        {
            var result = await Mediator.Send(new GetAboutInfoQuery());
            return View(result);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
