// Copyright (c) SDV Code Project. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace SdvCode.Controllers
{
    using Microsoft.AspNetCore.Mvc;
    using SdvCode.Constraints;
    using SdvCode.Services.Contact;
    using SdvCode.ViewModels.Contacts;

    public class ContactController : Controller
    {
        private readonly IContactService contactsService;

        public ContactController(IContactService contactsService)
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
                return this.View(model);
            }

            this.contactsService.SendEmail(model);
            this.TempData["Success"] =
                string.Format(SuccessMessages.SuccessfullySubmitedContactForm, model.Name);
            return this.RedirectToPage("/");
        }
    }
}