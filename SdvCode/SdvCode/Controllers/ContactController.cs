using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace SdvCode.Controllers
{
    public class ContactController : Controller
    {
        public IActionResult Index()
        {
            return this.View();
        }
    }
}