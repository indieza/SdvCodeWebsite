// Copyright (c) SDV Code Project. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace SdvCode.Controllers
{
    using Microsoft.AspNetCore.Mvc;
    using SdvCode.Services;
    using SdvCode.ViewModels.Contacts;

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
            if (!this.ModelState.IsValid)
            {
                return this.View();
            }

            this.contactsService.SendEmail(model);
            this.TempData["Success"] = $"Your message has been sent. Be patient you will receive a reply within 1 day.";
            return this.RedirectToPage("/");
        }
    }
}