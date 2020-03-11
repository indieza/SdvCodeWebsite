namespace SdvCode.Controllers
{
    using Microsoft.AspNetCore.Mvc;

    public class ErrorController : Controller
    {
        [Route("/Error/{statusCode}")]
        public IActionResult HttpStatusCodeHandler(int statusCode)
        {
            return this.View("NotFound");
        }
    }
}