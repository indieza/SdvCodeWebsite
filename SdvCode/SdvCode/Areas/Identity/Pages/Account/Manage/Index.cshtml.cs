// Copyright (c) SDV Code Project. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace SdvCode.Areas.Identity.Pages.Account.Manage
{
    using System;
    using System.Linq;
    using System.Threading.Tasks;
    using CloudinaryDotNet;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.RazorPages;
    using Microsoft.EntityFrameworkCore;
    using SdvCode.Constraints;
    using SdvCode.Data;
    using SdvCode.Models.Enums;
    using SdvCode.Models.User;
    using SdvCode.Services;
    using SdvCode.Services.Cloud;
    using SdvCode.ViewModels.Users.InputModels;
    using SdvCode.ViewModels.Users.ViewModels;

    public partial class IndexModel : PageModel
    {
        private readonly UserManager<ApplicationUser> userManager;
        private readonly SignInManager<ApplicationUser> signInManager;
        private readonly Cloudinary cloudinary;
        private readonly ApplicationDbContext db;

        public IndexModel(
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            Cloudinary cloudinary,
            ApplicationDbContext db)
        {
            this.userManager = userManager;
            this.signInManager = signInManager;
            this.cloudinary = cloudinary;
            this.db = db;
        }

        public string Username { get; set; }

        [TempData]
        public string StatusMessage { get; set; }

        [BindProperty]
        public ManageAccountBaseModel ManageAccountBaseModel { get; set; }

        public async Task<IActionResult> OnGetAsync()
        {
            var user = await this.userManager.GetUserAsync(this.User);
            if (user == null)
            {
                return this.NotFound($"Unable to load user with ID '{this.userManager.GetUserId(this.User)}'.");
            }

            await this.LoadAsync(user);
            return this.Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            bool isUpdatePersonalData = false;
            bool isUpdateProfileImage = false;
            bool isUpdateCoverImage = false;

            var user = await this.userManager.GetUserAsync(this.User);
            if (user == null)
            {
                return this.NotFound($"Unable to load user with ID '{this.userManager.GetUserId(this.User)}'.");
            }

            if (!this.ModelState.IsValid)
            {
                await this.LoadAsync(user);
                return this.Page();
            }

            var phoneNumber = await this.userManager.GetPhoneNumberAsync(user);
            if (this.ManageAccountBaseModel.ManageAccountInputModel.PhoneNumber != phoneNumber && this.ManageAccountBaseModel.ManageAccountInputModel.PhoneNumber != null)
            {
                var setPhoneResult = await this.userManager.SetPhoneNumberAsync(user, this.ManageAccountBaseModel.ManageAccountInputModel.PhoneNumber);
                if (!setPhoneResult.Succeeded)
                {
                    var userId = await this.userManager.GetUserIdAsync(user);
                    throw new InvalidOperationException($"Unexpected error occurred setting phone number for user with ID '{userId}'.");
                }

                isUpdatePersonalData = true;
            }

            var targetCountry = this.db.Countries.FirstOrDefault(x => x.Name.ToUpper() == this.ManageAccountBaseModel.ManageAccountInputModel.Country.ToUpper());
            var targetState = this.db.States.FirstOrDefault(x => x.Name.ToUpper() == this.ManageAccountBaseModel.ManageAccountInputModel.State.ToUpper());
            var targetCity = this.db.Cities.FirstOrDefault(x => x.Name.ToUpper() == this.ManageAccountBaseModel.ManageAccountInputModel.City.ToUpper());
            var targetZipCode = this.db.ZipCodes.FirstOrDefault(x => x.Code == this.ManageAccountBaseModel.ManageAccountInputModel.ZipCode);
            var targetCountryCode = this.db.CountryCodes.FirstOrDefault(x => x.Code == this.ManageAccountBaseModel.ManageAccountInputModel.CountryCode);

            if (this.ManageAccountBaseModel.ManageAccountInputModel.CountryCode != null)
            {
                if (targetCountryCode == null)
                {
                    targetCountryCode = new CountryCode
                    {
                        Code = this.ManageAccountBaseModel.ManageAccountInputModel.CountryCode,
                    };

                    this.db.CountryCodes.Add(targetCountryCode);
                }

                if (user.CountryCodeId != targetCountryCode.Id)
                {
                    user.CountryCodeId = targetCountryCode.Id;
                    isUpdatePersonalData = true;
                }
            }

            if (this.ManageAccountBaseModel.ManageAccountInputModel.Country != null)
            {
                if (targetCountry == null)
                {
                    targetCountry = new Country
                    {
                        Name = this.ManageAccountBaseModel.ManageAccountInputModel.Country,
                        CountryCode = targetCountryCode,
                    };
                    this.db.Countries.Add(targetCountry);
                }

                if (user.CountryId != targetCountry.Id)
                {
                    user.CountryId = targetCountry.Id;
                    isUpdatePersonalData = true;
                }
            }

            if (this.ManageAccountBaseModel.ManageAccountInputModel.State != null)
            {
                if (targetState == null)
                {
                    targetState = new State
                    {
                        Name = this.ManageAccountBaseModel.ManageAccountInputModel.State,
                        Country = targetCountry,
                    };
                    this.db.States.Add(targetState);
                }

                if (user.StateId != targetState.Id)
                {
                    user.StateId = targetState.Id;
                    isUpdatePersonalData = true;
                }
            }

            if (this.ManageAccountBaseModel.ManageAccountInputModel.City != null)
            {
                if (targetCity == null)
                {
                    targetCity = new City
                    {
                        Name = this.ManageAccountBaseModel.ManageAccountInputModel.City,
                        Country = targetCountry,
                        State = targetState,
                    };
                    this.db.Cities.Add(targetCity);
                }

                if (user.CityId != targetCity.Id)
                {
                    user.CityId = targetCity.Id;
                    isUpdatePersonalData = true;
                }
            }

            if (this.ManageAccountBaseModel.ManageAccountInputModel.ZipCode != 0)
            {
                if (targetZipCode == null)
                {
                    targetZipCode = new ZipCode
                    {
                        Code = this.ManageAccountBaseModel.ManageAccountInputModel.ZipCode,
                        City = targetCity,
                    };

                    this.db.ZipCodes.Add(targetZipCode);
                }

                if (user.ZipCodeId != targetZipCode.Id)
                {
                    user.ZipCodeId = targetZipCode.Id;
                    isUpdatePersonalData = true;
                }
            }

            if (this.ManageAccountBaseModel.ManageAccountInputModel.BirthDate != user.BirthDate)
            {
                user.BirthDate = this.ManageAccountBaseModel.ManageAccountInputModel.BirthDate;
                isUpdatePersonalData = true;
            }

            if (this.ManageAccountBaseModel.ManageAccountInputModel.Gender != user.Gender)
            {
                user.Gender = this.ManageAccountBaseModel.ManageAccountInputModel.Gender;
                isUpdatePersonalData = true;
            }

            if (this.ManageAccountBaseModel.ManageAccountInputModel.AboutMe != user.AboutMe)
            {
                user.AboutMe = this.ManageAccountBaseModel.ManageAccountInputModel.AboutMe;
                isUpdatePersonalData = true;
            }

            if (this.ManageAccountBaseModel.ManageAccountInputModel.FirstName != user.FirstName)
            {
                user.FirstName = this.ManageAccountBaseModel.ManageAccountInputModel.FirstName;
                isUpdatePersonalData = true;
            }

            if (this.ManageAccountBaseModel.ManageAccountInputModel.LastName != user.LastName)
            {
                user.LastName = this.ManageAccountBaseModel.ManageAccountInputModel.LastName;
                isUpdatePersonalData = true;
            }

            if (this.ManageAccountBaseModel.ManageAccountInputModel.GitHubUrl != user.GitHubUrl)
            {
                user.GitHubUrl = this.ManageAccountBaseModel.ManageAccountInputModel.GitHubUrl;
                isUpdatePersonalData = true;
            }

            if (this.ManageAccountBaseModel.ManageAccountInputModel.StackoverflowUrl != user.StackoverflowUrl)
            {
                user.StackoverflowUrl = this.ManageAccountBaseModel.ManageAccountInputModel.StackoverflowUrl;
                isUpdatePersonalData = true;
            }

            if (this.ManageAccountBaseModel.ManageAccountInputModel.FacebookUrl != user.FacebookUrl)
            {
                user.FacebookUrl = this.ManageAccountBaseModel.ManageAccountInputModel.FacebookUrl;
                isUpdatePersonalData = true;
            }

            if (this.ManageAccountBaseModel.ManageAccountInputModel.LinkedinUrl != user.LinkedinUrl)
            {
                user.LinkedinUrl = this.ManageAccountBaseModel.ManageAccountInputModel.LinkedinUrl;
                isUpdatePersonalData = true;
            }

            if (this.ManageAccountBaseModel.ManageAccountInputModel.TwitterUrl != user.TwitterUrl)
            {
                user.TwitterUrl = this.ManageAccountBaseModel.ManageAccountInputModel.TwitterUrl;
                isUpdatePersonalData = true;
            }

            if (this.ManageAccountBaseModel.ManageAccountInputModel.InstagramUrl != user.InstagramUrl)
            {
                user.InstagramUrl = this.ManageAccountBaseModel.ManageAccountInputModel.InstagramUrl;
                isUpdatePersonalData = true;
            }

            var profileImageUrl = await ApplicationCloudinary.UploadImage(
                this.cloudinary,
                this.ManageAccountBaseModel.ManageAccountInputModel.ProfilePicture,
                string.Format(GlobalConstants.CloudinaryUserProfilePictureName, user.UserName),
                string.Format(GlobalConstants.UserProfilePicturesFolder, user.UserName));

            if (profileImageUrl != null)
            {
                isUpdateProfileImage = true;
                if (profileImageUrl != user.ImageUrl)
                {
                    user.ImageUrl = profileImageUrl;
                }
            }

            var coverImageUrl = await ApplicationCloudinary.UploadImage(
                this.cloudinary,
                this.ManageAccountBaseModel.ManageAccountInputModel.CoverImage,
                string.Format(GlobalConstants.CloudinaryUserCoverImageName, user.UserName),
                string.Format(GlobalConstants.UserProfilePicturesFolder, user.UserName));

            if (coverImageUrl != null)
            {
                isUpdateCoverImage = true;
                if (coverImageUrl != user.ImageUrl)
                {
                    user.CoverImageUrl = coverImageUrl;
                }
            }

            if (isUpdatePersonalData == true)
            {
                user.UserActions.Add(new UserAction
                {
                    Action = UserActionsType.EditPersonalData,
                    ActionDate = DateTime.UtcNow,
                    PersonUsername = user.UserName,
                    ProfileImageUrl = user.ImageUrl,
                    ApplicationUserId = user.Id,
                });
            }

            if (isUpdateCoverImage == true)
            {
                user.UserActions.Add(new UserAction
                {
                    Action = UserActionsType.ChangeCoverImage,
                    ActionDate = DateTime.UtcNow,
                    PersonUsername = user.UserName,
                    ProfileImageUrl = user.ImageUrl,
                    CoverImageUrl = coverImageUrl,
                    ApplicationUserId = user.Id,
                });
            }

            if (isUpdateProfileImage == true)
            {
                user.UserActions.Add(new UserAction
                {
                    Action = UserActionsType.ChangeProfilePicture,
                    ActionDate = DateTime.UtcNow,
                    PersonUsername = user.UserName,
                    ProfileImageUrl = profileImageUrl,
                    ApplicationUserId = user.Id,
                });
            }

            await this.userManager.UpdateAsync(user);
            await this.signInManager.RefreshSignInAsync(user);
            this.StatusMessage = "Your profile has been updated";
            return this.RedirectToPage();
        }

        private async Task LoadAsync(ApplicationUser user)
        {
            var userName = await this.userManager.GetUserNameAsync(user);
            var phoneNumber = await this.userManager.GetPhoneNumberAsync(user);
            var zipCode = this.db.ZipCodes.FirstOrDefault(x => x.Id == user.ZipCodeId);
            var country = this.db.Countries.FirstOrDefault(x => x.Id == user.CountryId);
            var state = this.db.States.FirstOrDefault(x => x.Id == user.StateId);
            var city = this.db.Cities.FirstOrDefault(x => x.Id == user.CityId);
            var countryCode = this.db.CountryCodes.FirstOrDefault(x => x.Id == user.CountryCodeId);

            var allCountryCodesNames = this.db.CountryCodes.Select(x => x.Code).OrderBy(x => x).ToList();
            var allCities = this.db.Cities.Select(x => x.Name).OrderBy(x => x).ToList();
            var allStates = this.db.States.Select(x => x.Name).OrderBy(x => x).ToList();
            var allCountries = this.db.Countries.Select(x => x.Name).OrderBy(x => x).ToList();

            this.Username = userName;

            this.ManageAccountBaseModel = new ManageAccountBaseModel
            {
                ManageAccountInputModel = new ManageAccountInputModel
                {
                    PhoneNumber = phoneNumber,
                    State = state == null ? string.Empty : state.Name,
                    Country = country == null ? string.Empty : country.Name,
                    City = city == null ? string.Empty : city.Name,
                    BirthDate = user.BirthDate,
                    Gender = user.Gender,
                    AboutMe = user.AboutMe,
                    FirstName = user.FirstName,
                    LastName = user.LastName,
                    GitHubUrl = user.GitHubUrl,
                    StackoverflowUrl = user.StackoverflowUrl,
                    FacebookUrl = user.FacebookUrl,
                    InstagramUrl = user.InstagramUrl,
                    TwitterUrl = user.TwitterUrl,
                    LinkedinUrl = user.LinkedinUrl,
                    RegisteredOn = user.RegisteredOn,
                    CountryCode = countryCode == null ? string.Empty : countryCode.Code,
                    Email = user.Email,
                    ZipCode = zipCode == null ? 0 : zipCode.Code,

                    // TODO Image URL
                },
                ManageAccountViewModel = new ManageAccountViewModel
                {
                    CountryCodes = allCountryCodesNames,
                    Cities = allCities,
                    States = allStates,
                    Countries = allCountries,
                },
            };
        }
    }
}