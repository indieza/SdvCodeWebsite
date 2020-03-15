// Copyright (c) SDV Code Project. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace SdvCode.Areas.Identity.Pages.Account
{
    using System;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.RazorPages;
    using Microsoft.Extensions.Options;
    using SdvCode.Models.User;
    using SdvCode.SecurityModels;
    using Twilio.Rest.Verify.V2.Service;

    [Authorize]
    public class VerifyPhoneModel : PageModel
    {
        private readonly TwilioVerifySettings settings;
        private readonly UserManager<ApplicationUser> userManager;

        public VerifyPhoneModel(IOptions<TwilioVerifySettings> settings, UserManager<ApplicationUser> userManager)
        {
            this.settings = settings.Value;
            this.userManager = userManager;
        }

        public string PhoneNumber { get; set; }

        public string CountryCode { get; set; }

        public async Task<IActionResult> OnGetAsync()
        {
            await this.LoadPhoneNumber();
            return this.Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            await this.LoadPhoneNumber();

            try
            {
                var verification = await VerificationResource.CreateAsync(
                    to: $"+{this.CountryCode}{this.PhoneNumber}",
                    channel: "sms",
                    pathServiceSid: this.settings.VerificationServiceSID);

                if (verification.Status == "pending")
                {
                    return this.RedirectToPage("ConfirmPhone");
                }

                this.ModelState.AddModelError(string.Empty, $"There was an error sending the verification code: {verification.Status}");
            }
            catch (Exception)
            {
                this.ModelState.AddModelError(
                    string.Empty,
                    "There was an error sending the verification code, please check the phone number is correct and try again");
            }

            return this.Page();
        }

        private async Task LoadPhoneNumber()
        {
            var user = await this.userManager.GetUserAsync(this.User);
            if (user == null)
            {
                throw new Exception($"Unable to load user with ID '{this.userManager.GetUserId(this.User)}'.");
            }

            this.PhoneNumber = user.PhoneNumber;
            this.CountryCode = user.CountryCode.ToString().Split("_")[1];
        }
    }
}