namespace SdvCode.Controllers
{
    using Microsoft.AspNetCore.Mvc;

    public class BlogController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}