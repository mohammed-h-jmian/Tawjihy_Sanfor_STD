using Microsoft.AspNetCore.Mvc;

namespace tawjihy.Controllers
{
    public class BaseController : Controller
    {
        public IActionResult NotFound()
        {
            return View();
        }   
        public IActionResult ErrorDatabase()
        {
            return View();
        }

    }
}
