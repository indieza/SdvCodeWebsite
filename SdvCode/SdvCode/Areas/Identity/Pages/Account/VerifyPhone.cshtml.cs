using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Options;
using SdvCode.Models;
using SdvCode.SecurityModels;
using System;
using System.Threading.Tasks;
using Twilio.Rest.Verify.V2.Service;

namespace SdvCode.Areas.Identity.Pages.Account
{
    [Authorize]
    public class VerifyPhoneModel : PageModel
    {
        private readonly TwilioVerifySettings _settings;
        private readonly UserManager<ApplicationUser> _userManager;

        public VerifyPhoneModel(IOptions<TwilioVerifySettings> settings, UserManager<ApplicationUser> userManager)
        {
            _settings = settings.Value;
            _userManager = userManager;
        }

        public string PhoneNumber { get; set; }

        public string CountryCode { get; set; }

        public async Task<IActionResult> OnGetAsync()
        {
            await LoadPhoneNumber();
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            await LoadPhoneNumber();

            try
            {
                var verification = await VerificationResource.CreateAsync(
                    to: $"+{this.CountryCode}{PhoneNumber}",
                    channel: "sms",
                    pathServiceSid: _settings.VerificationServiceSID
                );

                if (verification.Status == "pending")
                {
                    return RedirectToPage("ConfirmPhone");
                }

                ModelState.AddModelError("", $"There was an error sending the verification code: {verification.Status}");
            }
            catch (Exception)
            {
                ModelState.AddModelError("",
                    "There was an error sending the verification code, please check the phone number is correct and try again");
            }

            return Page();
        }

        private async Task LoadPhoneNumber()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                throw new Exception($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }

            PhoneNumber = user.PhoneNumber;
            this.CountryCode = user.CountryCode.ToString().Split("_")[1];
        }
    }
}