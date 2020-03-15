// Copyright (c) SDV Code Project. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace SdvCode.Areas.Identity.Pages.Account
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.RazorPages;
    using Microsoft.Extensions.Options;
    using SdvCode.Areas.Administration.Services;
    using SdvCode.Constraints;
    using SdvCode.Models.User;
    using SdvCode.SecurityModels;
    using Twilio.Rest.Verify.V2.Service;

    [Authorize]
    public class ConfirmPhoneModel : PageModel
    {
        private readonly TwilioVerifySettings settings;
        private readonly UserManager<ApplicationUser> userManager;
        private readonly IDashboardService dashboardService;

        public ConfirmPhoneModel(
            UserManager<ApplicationUser> userManager,
            IOptions<TwilioVerifySettings> settings,
            IDashboardService dashboardService)
        {
            this.userManager = userManager;
            this.dashboardService = dashboardService;
            this.settings = settings.Value;
        }

        public string PhoneNumber { get; set; }

        public string CountryCode { get; set; }

        [BindProperty]
        [Required]
        [Display(Name = "Code")]
        public string VerificationCode { get; set; }

        public async Task<IActionResult> OnGetAsync()
        {
            await this.LoadPhoneNumber();
            return this.Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            await this.LoadPhoneNumber();
            if (!this.ModelState.IsValid)
            {
                return this.Page();
            }

            try
            {
                var verification = await VerificationCheckResource.CreateAsync(
                    to: $"+{this.CountryCode}{this.PhoneNumber}",
                    code: this.VerificationCode,
                    pathServiceSid: this.settings.VerificationServiceSID);
                if (verification.Status == "approved")
                {
                    var identityUser = await this.userManager.GetUserAsync(this.User);
                    identityUser.PhoneNumberConfirmed = true;
                    var updateResult = await this.userManager.UpdateAsync(identityUser);

                    if (updateResult.Succeeded)
                    {
                        var user = this.userManager.GetUserAsync(this.HttpContext.User);
                        var isAdded = await this.dashboardService.IsAddedUserInRole(GlobalConstants.ContributorRole, user.Result.UserName);
                        if (isAdded)
                        {
                            this.TempData["Success"] = string.Format(
                                SuccessMessages.SuccessfullyConfirmedPhoneNumberAndRegisteredContributorRole,
                                GlobalConstants.ContributorRole);
                        }
                        else
                        {
                            this.TempData["Success"] = string.Format(
                                SuccessMessages.SuccessfullyConfirmedPhoneNumberInContributorRole,
                                GlobalConstants.ContributorRole);
                        }

                        return this.Redirect($"/Profile/{user.Result.UserName}");
                    }
                    else
                    {
                        this.ModelState.AddModelError(string.Empty, "There was an error confirming the verification code, please try again");
                    }
                }
                else
                {
                    this.ModelState.AddModelError(string.Empty, $"There was an error confirming the verification code: {verification.Status}");
                }
            }
            catch (Exception)
            {
                this.ModelState.AddModelError(
                    string.Empty,
                    "There was an error confirming the code, please check the verification code is correct and try again");
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