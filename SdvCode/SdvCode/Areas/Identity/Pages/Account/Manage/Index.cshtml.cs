using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using CloudinaryDotNet;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SdvCode.Models;
using SdvCode.Models.Enums;
using SdvCode.Services;
using SdvCode.ViewModels.Users;

namespace SdvCode.Areas.Identity.Pages.Account.Manage
{
    public partial class IndexModel : PageModel
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly Cloudinary cloudinary;

        public IndexModel(
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            Cloudinary cloudinary)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            this.cloudinary = cloudinary;
        }

        public string Username { get; set; }

        [TempData]
        public string StatusMessage { get; set; }

        [BindProperty]
        public ManageAccountInputModel Input { get; set; }

        private async Task LoadAsync(ApplicationUser user)
        {
            var userName = await _userManager.GetUserNameAsync(user);
            var phoneNumber = await _userManager.GetPhoneNumberAsync(user);

            Username = userName;

            Input = new ManageAccountInputModel
            {
                PhoneNumber = phoneNumber,
                City = user.City,
                Country = user.Country,
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
                CountryCode = user.CountryCode,
                Email = user.Email
                // TODO Image URL
            };
        }

        public async Task<IActionResult> OnGetAsync()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }

            await LoadAsync(user);
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            bool isUpdatePersonalData = false;
            bool isUpdateProfileImage = false;
            bool isUpdateCoverImage = false;

            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }

            if (!ModelState.IsValid)
            {
                await LoadAsync(user);
                return Page();
            }

            var phoneNumber = await _userManager.GetPhoneNumberAsync(user);
            if (Input.PhoneNumber != phoneNumber && Input.PhoneNumber != null)
            {
                var setPhoneResult = await _userManager.SetPhoneNumberAsync(user, Input.PhoneNumber);
                if (!setPhoneResult.Succeeded)
                {
                    var userId = await _userManager.GetUserIdAsync(user);
                    throw new InvalidOperationException($"Unexpected error occurred setting phone number for user with ID '{userId}'.");
                }

                isUpdatePersonalData = true;
            }

            if (Input.City != user.City)
            {
                user.City = Input.City;
                isUpdatePersonalData = true;
            }

            if (Input.Country != user.Country)
            {
                user.Country = Input.Country;
                isUpdatePersonalData = true;
            }

            if (Input.BirthDate != user.BirthDate)
            {
                user.BirthDate = Input.BirthDate;
                isUpdatePersonalData = true;
            }

            if (Input.Gender != user.Gender)
            {
                user.Gender = Input.Gender;
                isUpdatePersonalData = true;
            }

            if (Input.AboutMe != user.AboutMe)
            {
                user.AboutMe = Input.AboutMe;
                isUpdatePersonalData = true;
            }

            if (Input.FirstName != user.FirstName)
            {
                user.FirstName = Input.FirstName;
                isUpdatePersonalData = true;
            }

            if (Input.LastName != user.LastName)
            {
                user.LastName = Input.LastName;
                isUpdatePersonalData = true;
            }

            if (Input.GitHubUrl != user.GitHubUrl)
            {
                user.GitHubUrl = Input.GitHubUrl;
                isUpdatePersonalData = true;
            }

            if (Input.StackoverflowUrl != user.StackoverflowUrl)
            {
                user.StackoverflowUrl = Input.StackoverflowUrl;
                isUpdatePersonalData = true;
            }

            if (Input.FacebookUrl != user.FacebookUrl)
            {
                user.FacebookUrl = Input.FacebookUrl;
                isUpdatePersonalData = true;
            }

            if (Input.LinkedinUrl != user.LinkedinUrl)
            {
                user.LinkedinUrl = Input.LinkedinUrl;
                isUpdatePersonalData = true;
            }

            if (Input.TwitterUrl != user.TwitterUrl)
            {
                user.TwitterUrl = Input.TwitterUrl;
                isUpdatePersonalData = true;
            }

            if (Input.InstagramUrl != user.InstagramUrl)
            {
                user.InstagramUrl = Input.InstagramUrl;
                isUpdatePersonalData = true;
            }

            if (Input.CountryCode != user.CountryCode)
            {
                user.CountryCode = Input.CountryCode;
                isUpdatePersonalData = true;
            }

            var profileImageUrl = await ApplicationCloudinary.UploadImage(this.cloudinary,
                Input.ProfilePicture,
                $"{user.UserName}ProfilePicture");

            if (profileImageUrl != null)
            {
                isUpdateProfileImage = true;
                if (profileImageUrl != user.ImageUrl)
                {
                    user.ImageUrl = profileImageUrl;
                }
            }

            var coverImageUrl = await ApplicationCloudinary.UploadImage(this.cloudinary,
                Input.CoverImage,
                $"{user.UserName}CoverPicture");

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
                    PersonProfileImageUrl = user.ImageUrl,
                    ApplicationUserId = user.Id
                });
            }

            if (isUpdateCoverImage == true)
            {
                user.UserActions.Add(new UserAction
                {
                    Action = UserActionsType.ChangeCoverImage,
                    ActionDate = DateTime.UtcNow,
                    PersonUsername = user.UserName,
                    PersonProfileImageUrl = user.ImageUrl,
                    CoverImageUrl = coverImageUrl,
                    ApplicationUserId = user.Id
                });
            }

            if (isUpdateProfileImage == true)
            {
                user.UserActions.Add(new UserAction
                {
                    Action = UserActionsType.ChangeProfilePicture,
                    ActionDate = DateTime.UtcNow,
                    PersonUsername = user.UserName,
                    PersonProfileImageUrl = user.ImageUrl,
                    ProfileImageUrl = profileImageUrl,
                    ApplicationUserId = user.Id
                });
            }

            await _userManager.UpdateAsync(user);
            await _signInManager.RefreshSignInAsync(user);
            StatusMessage = "Your profile has been updated";
            return RedirectToPage();
        }
    }
}