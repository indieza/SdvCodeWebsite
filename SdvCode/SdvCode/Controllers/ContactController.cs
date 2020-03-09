using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using SdvCode.Services;
using SdvCode.ViewModels.Contacts;

namespace SdvCode.Controllers
{
    public class ContactController : Controller
    {
        private readonly IContactsService contactsService;

        public ContactController(IContactsService contactsService)
        {
            this.contactsService = contactsService;
        }

        [BindProperty]
        public ContactInputModel Contact { get; set; }

        public IActionResult Index()
        {
            return this.View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Index(ContactInputModel model)
        {
            if (!ModelState.IsValid)
            {
                return this.View();
            }

            this.contactsService.SendEmail(model);
            TempData["Success"] = $"Your message has been sent. Be patient you will receive a reply within 1 day.";
            return RedirectToPage("/");
        }
    }
}