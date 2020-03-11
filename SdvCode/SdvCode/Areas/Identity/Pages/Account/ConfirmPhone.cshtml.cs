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
using SdvCode.Areas.Administration.Services;
using SdvCode.Constraints;
using SdvCode.Models;
using SdvCode.SecurityModels;
using Twilio.Rest.Verify.V2.Service;

namespace SdvCode.Areas.Identity.Pages.Account
{
    [Authorize]
    public class ConfirmPhoneModel : PageModel
    {
        private readonly TwilioVerifySettings _settings;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IDashboardService dashboardService;

        public ConfirmPhoneModel(UserManager<ApplicationUser> userManager,
            IOptions<TwilioVerifySettings> settings,
            IDashboardService dashboardService)
        {
            _userManager = userManager;
            this.dashboardService = dashboardService;
            _settings = settings.Value;
        }

        public string PhoneNumber { get; set; }

        public string CountryCode { get; set; }

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
                    to: $"+{this.CountryCode}{PhoneNumber}",
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
                        var isAdded = await this.dashboardService.IsAddedUserInRole(GlobalConstants.ContributorRole, user.Result.UserName);
                        if (isAdded)
                        {
                            TempData["Success"] = string.Format(SuccessMessages.SuccessfullyConfirmedPhoneNumber,
                                GlobalConstants.ContributorRole);
                        }

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

            this.PhoneNumber = user.PhoneNumber;
            this.CountryCode = user.CountryCode.ToString().Split("_")[1];
        }
    }
}