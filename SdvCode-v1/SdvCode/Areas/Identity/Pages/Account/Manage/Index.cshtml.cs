// Copyright (c) SDV Code Project. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace SdvCode.Areas.Identity.Pages.Account.Manage
{
    using System;
    using System.Collections.Generic;
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
    using SdvCode.Models.User.UserActions.ProfileActions;
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

            CountryCode targetCountryCode = await this.ValidateCountryCode(
                    this.ManageAccountBaseModel.ManageAccountInputModel.CountryCode);

            Country targetCountry = await this.ValidateCountry(
                this.ManageAccountBaseModel.ManageAccountInputModel.Country,
                targetCountryCode);

            State targetState = await this.ValidateState(
                this.ManageAccountBaseModel.ManageAccountInputModel.State,
                targetCountry);

            City targetCity = await this.ValidateCity(
                this.ManageAccountBaseModel.ManageAccountInputModel.City,
                targetState,
                targetCountry);

            ZipCode targetZipCode = await this.ValidateZipCode(
                this.ManageAccountBaseModel.ManageAccountInputModel.ZipCode,
                targetCity);

            var personalDataContent = new List<string>();

            if (user.CountryCodeId != targetCountryCode?.Id)
            {
                user.CountryCodeId = targetCountryCode?.Id;
                isUpdatePersonalData = true;
                personalDataContent.Add("Country Code");
            }

            if (user.CountryId != targetCountry?.Id)
            {
                user.CountryId = targetCountry?.Id;
                isUpdatePersonalData = true;
                personalDataContent.Add("Country");
            }

            if (user.StateId != targetState?.Id)
            {
                user.StateId = targetState?.Id;
                isUpdatePersonalData = true;
                personalDataContent.Add("State");
            }

            if (user.CityId != targetCity?.Id)
            {
                user.CityId = targetCity?.Id;
                isUpdatePersonalData = true;
                personalDataContent.Add("City");
            }

            if (user.ZipCodeId != targetZipCode?.Id)
            {
                user.ZipCodeId = targetZipCode?.Id;
                isUpdatePersonalData = true;
                personalDataContent.Add("Zip Code");
            }

            if (this.ManageAccountBaseModel.ManageAccountInputModel.BirthDate != user.BirthDate)
            {
                user.BirthDate = this.ManageAccountBaseModel.ManageAccountInputModel.BirthDate;
                isUpdatePersonalData = true;
                personalDataContent.Add("Birth Date");
            }

            if (this.ManageAccountBaseModel.ManageAccountInputModel.Gender != user.Gender)
            {
                user.Gender = this.ManageAccountBaseModel.ManageAccountInputModel.Gender;
                isUpdatePersonalData = true;
                personalDataContent.Add("Gender");
            }

            if (this.ManageAccountBaseModel.ManageAccountInputModel.AboutMe != user.AboutMe)
            {
                user.AboutMe = this.ManageAccountBaseModel.ManageAccountInputModel.AboutMe;
                isUpdatePersonalData = true;
                personalDataContent.Add("Bio");
            }

            if (this.ManageAccountBaseModel.ManageAccountInputModel.FirstName != user.FirstName)
            {
                user.FirstName = this.ManageAccountBaseModel.ManageAccountInputModel.FirstName;
                isUpdatePersonalData = true;
                personalDataContent.Add("First Name");
            }

            if (this.ManageAccountBaseModel.ManageAccountInputModel.LastName != user.LastName)
            {
                user.LastName = this.ManageAccountBaseModel.ManageAccountInputModel.LastName;
                isUpdatePersonalData = true;
                personalDataContent.Add("Last Name");
            }

            if (this.ManageAccountBaseModel.ManageAccountInputModel.GitHubUrl != user.GitHubUrl)
            {
                user.GitHubUrl = this.ManageAccountBaseModel.ManageAccountInputModel.GitHubUrl;
                isUpdatePersonalData = true;
                personalDataContent.Add("GitHub URL");
            }

            if (this.ManageAccountBaseModel.ManageAccountInputModel.StackoverflowUrl != user.StackoverflowUrl)
            {
                user.StackoverflowUrl = this.ManageAccountBaseModel.ManageAccountInputModel.StackoverflowUrl;
                isUpdatePersonalData = true;
                personalDataContent.Add("Stackoverflow URL");
            }

            if (this.ManageAccountBaseModel.ManageAccountInputModel.FacebookUrl != user.FacebookUrl)
            {
                user.FacebookUrl = this.ManageAccountBaseModel.ManageAccountInputModel.FacebookUrl;
                isUpdatePersonalData = true;
                personalDataContent.Add("Facebook URL");
            }

            if (this.ManageAccountBaseModel.ManageAccountInputModel.LinkedinUrl != user.LinkedinUrl)
            {
                user.LinkedinUrl = this.ManageAccountBaseModel.ManageAccountInputModel.LinkedinUrl;
                isUpdatePersonalData = true;
                personalDataContent.Add("LinkedIn URL");
            }

            if (this.ManageAccountBaseModel.ManageAccountInputModel.TwitterUrl != user.TwitterUrl)
            {
                user.TwitterUrl = this.ManageAccountBaseModel.ManageAccountInputModel.TwitterUrl;
                isUpdatePersonalData = true;
                personalDataContent.Add("Twitter URL");
            }

            if (this.ManageAccountBaseModel.ManageAccountInputModel.InstagramUrl != user.InstagramUrl)
            {
                user.InstagramUrl = this.ManageAccountBaseModel.ManageAccountInputModel.InstagramUrl;
                isUpdatePersonalData = true;
                personalDataContent.Add("Instagram URL");
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
                    BaseUserAction = new EditPersonalDataUserAction
                    {
                        ApplicationUser = user,
                        ApplicationUserId = user.Id,
                        Content = $"User has changed his {string.Join(", ", personalDataContent)}.",
                    },
                });
            }

            if (isUpdateCoverImage == true)
            {
                user.UserActions.Add(new UserAction
                {
                    BaseUserAction = new ChangeCoverImageUserAction
                    {
                        ApplicationUser = user,
                        ApplicationUserId = user.Id,
                    },
                });
            }

            if (isUpdateProfileImage == true)
            {
                user.UserActions.Add(new UserAction
                {
                    BaseUserAction = new ChangeProfilePictureUserAction
                    {
                        ApplicationUser = user,
                        ApplicationUserId = user.Id,
                    },
                });
            }

            await this.userManager.UpdateAsync(user);
            await this.signInManager.RefreshSignInAsync(user);
            this.StatusMessage = "Your profile has been updated";
            return this.RedirectToPage();
        }

        private async Task<ZipCode> ValidateZipCode(int zipCode, City targetCity)
        {
            if (zipCode != 0)
            {
                var targetZipCode = await this.db.ZipCodes
                    .FirstOrDefaultAsync(x => x.Code == zipCode);

                if (targetZipCode == null)
                {
                    targetZipCode = new ZipCode
                    {
                        Code = zipCode,
                        CityId = targetCity?.Id,
                    };

                    this.db.ZipCodes.Add(targetZipCode);
                    await this.db.SaveChangesAsync();
                }
                else
                {
                    if (targetCity != null && targetCity.Id != targetZipCode.CityId)
                    {
                        targetZipCode.CityId = targetCity.Id;

                        this.db.ZipCodes.Update(targetZipCode);
                        await this.db.SaveChangesAsync();
                    }
                }

                return targetZipCode;
            }

            return null;
        }

        private async Task<City> ValidateCity(string city, State targetState, Country targetCountry)
        {
            if (city != null)
            {
                var targetCity = await this.db.Cities
                    .FirstOrDefaultAsync(x => x.Name.ToUpper() == city.ToUpper());

                if (targetCity == null)
                {
                    targetCity = new City
                    {
                        Name = city,
                        CountryId = targetCountry?.Id,
                        StateId = targetState?.Id,
                    };

                    this.db.Cities.Add(targetCity);
                    await this.db.SaveChangesAsync();
                }
                else
                {
                    if (targetCountry != null && targetCountry.Id != targetCity.CountryId)
                    {
                        targetCity.CountryId = targetCountry.Id;

                        this.db.Cities.Update(targetCity);
                        await this.db.SaveChangesAsync();
                    }

                    if (targetState != null && targetState.Id != targetCity.StateId)
                    {
                        targetCity.StateId = targetState.Id;

                        this.db.Cities.Update(targetCity);
                        await this.db.SaveChangesAsync();
                    }
                }

                return targetCity;
            }

            return null;
        }

        private async Task<State> ValidateState(string state, Country targetCountry)
        {
            if (state != null)
            {
                var targetSate = await this.db.States
                    .FirstOrDefaultAsync(x => x.Name.ToUpper() == state.ToUpper());

                if (targetSate == null)
                {
                    targetSate = new State
                    {
                        Name = state,
                        CountryId = targetCountry?.Id,
                    };

                    this.db.States.Add(targetSate);
                    await this.db.SaveChangesAsync();
                }
                else
                {
                    if (targetCountry != null && targetCountry.Id != targetSate.CountryId)
                    {
                        targetSate.CountryId = targetCountry.Id;

                        this.db.States.Update(targetSate);
                        await this.db.SaveChangesAsync();
                    }
                }

                return targetSate;
            }

            return null;
        }

        private async Task<Country> ValidateCountry(string country, CountryCode targetCountryCode)
        {
            if (country != null)
            {
                var targetCountry = await this.db.Countries
                        .FirstOrDefaultAsync(x => x.Name.ToUpper() == country.ToUpper());

                if (targetCountry == null)
                {
                    targetCountry = new Country
                    {
                        Name = country,
                        CountryCodeId = targetCountryCode?.Id,
                    };

                    this.db.Countries.Add(targetCountry);
                    await this.db.SaveChangesAsync();
                }
                else
                {
                    if (targetCountryCode != null && targetCountryCode.Id != targetCountry.CountryCodeId)
                    {
                        targetCountry.CountryCodeId = targetCountryCode.Id;

                        this.db.Countries.Update(targetCountry);
                        await this.db.SaveChangesAsync();
                    }
                }

                return targetCountry;
            }

            return null;
        }

        private async Task<CountryCode> ValidateCountryCode(string countryCode)
        {
            if (countryCode != null)
            {
                var targetCountryCode = await this.db.CountryCodes
                        .FirstOrDefaultAsync(x => x.Code.ToUpper() == countryCode.ToUpper());

                if (targetCountryCode == null)
                {
                    targetCountryCode = new CountryCode
                    {
                        Code = countryCode,
                    };

                    this.db.CountryCodes.Add(targetCountryCode);
                    await this.db.SaveChangesAsync();
                }

                return targetCountryCode;
            }

            return null;
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
                    State = state?.Name,
                    Country = country?.Name,
                    City = city?.Name,
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
                    CountryCode = countryCode?.Code,
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