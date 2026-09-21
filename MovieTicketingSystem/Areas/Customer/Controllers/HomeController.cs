using Microsoft.AspNetCore.Mvc;

namespace MovieTicketingSystem.Areas.Customer.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
