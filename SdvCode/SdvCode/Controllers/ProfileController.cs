using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using SdvCode.Data;
using SdvCode.Models;

namespace SdvCode.Controllers
{
    public class ProfileController : Controller
    {
        private readonly UserManager<ApplicationUser> userManager;
        private readonly ApplicationDbContext db;

        public ProfileController(UserManager<ApplicationUser> userManager, ApplicationDbContext db)
        {
            this.userManager = userManager;
            this.db = db;
        }

        [Authorize]
        [Route("/Profile/{username}")]
        public IActionResult Index(string username)
        {
            var currentUserId = this.userManager.GetUserId(HttpContext.User);
            var user = this.db.Users.FirstOrDefault(u => u.UserName == username);
            user.IsFollowed = this.db.FollowUnfollows.Any(x => x.FollowerId == currentUserId && x.PersonId == user.Id) &&
                user.IsFollowed == true;
            return View(user);
        }

        [Authorize]
        [Route("/Follow/{username}")]
        public IActionResult Follow(string username)
        {
            var currentUserId = this.userManager.GetUserId(HttpContext.User);

            var user = this.db.Users.FirstOrDefault(u => u.UserName == username);
            var currentUser = this.db.Users.FirstOrDefault(u => u.Id == currentUserId);

            db.FollowUnfollows.Add(new FollowUnfollow
            {
                PersonId = user.Id,
                FollowerId = currentUser.Id
            });
            user.IsFollowed = true;

            this.db.SaveChanges();
            return this.Redirect($"/Profile/{currentUser.UserName}");
        }

        [Authorize]
        [Route("/Unfollow/{username}")]
        public IActionResult Unfollow(string username)
        {
            var currentUserId = this.userManager.GetUserId(HttpContext.User);

            var user = this.db.Users.FirstOrDefault(u => u.UserName == username);
            var currentUser = this.db.Users.FirstOrDefault(u => u.Id == currentUserId);
            user.IsFollowed = false;
            db.SaveChanges();
            return this.Redirect($"/Profile/{currentUser.UserName}");
        }
    }
}