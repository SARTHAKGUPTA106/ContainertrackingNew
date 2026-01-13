using Microsoft.AspNetCore.Mvc;

namespace Containertracking.Controllers
{
    public class ForgetPasswordController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
