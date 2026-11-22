using Microsoft.AspNetCore.Mvc;

namespace Stock.IdentityServer.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
