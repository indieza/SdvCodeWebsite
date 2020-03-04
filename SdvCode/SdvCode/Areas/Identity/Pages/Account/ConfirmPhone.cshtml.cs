using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Options;
using SdvCode.Models;
using SdvCode.ViewModels.Security;
using Twilio.Rest.Verify.V2.Service;

namespace SdvCode.Areas.Identity.Pages.Account
{
    [Authorize]
    public class ConfirmPhoneModel : PageModel
    {
        private readonly TwilioVerifySettings _settings;
        private readonly UserManager<ApplicationUser> _userManager;

        public ConfirmPhoneModel(UserManager<ApplicationUser> userManager, IOptions<TwilioVerifySettings> settings)
        {
            _userManager = userManager;
            _settings = settings.Value;
        }

        public string PhoneNumber { get; set; }

        [BindProperty, Required, Display(Name = "Code")]
        public string VerificationCode { get; set; }

        public async Task<IActionResult> OnGetAsync()
        {
            await LoadPhoneNumber();
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            await LoadPhoneNumber();
            if (!ModelState.IsValid)
            {
                return Page();
            }

            try
            {
                var verification = await VerificationCheckResource.CreateAsync(
                    to: "+359" + PhoneNumber,
                    code: VerificationCode,
                    pathServiceSid: _settings.VerificationServiceSID
                );
                if (verification.Status == "approved")
                {
                    var identityUser = await _userManager.GetUserAsync(User);
                    identityUser.PhoneNumberConfirmed = true;
                    var updateResult = await _userManager.UpdateAsync(identityUser);

                    if (updateResult.Succeeded)
                    {
                        var user = this._userManager.GetUserAsync(HttpContext.User);
                        return Redirect($"/Profile/{user.Result.UserName}");
                    }
                    else
                    {
                        ModelState.AddModelError("", "There was an error confirming the verification code, please try again");
                    }
                }
                else
                {
                    ModelState.AddModelError("", $"There was an error confirming the verification code: {verification.Status}");
                }
            }
            catch (Exception)
            {
                ModelState.AddModelError("",
                    "There was an error confirming the code, please check the verification code is correct and try again");
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
        }
    }
}