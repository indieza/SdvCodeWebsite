using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using SdvCode.Models.Enums;
using SdvCode.Services.UserPosts;
using SdvCode.ViewModels.UserPosts;

namespace SdvCode.Controllers
{
    public class UserPostsController : Controller
    {
        private readonly IUserPostsService userPostsService;

        public UserPostsController(IUserPostsService userPostsService)
        {
            this.userPostsService = userPostsService;
        }

        [Authorize]
        [Route("Blog/UserPosts/{username}/{filter}")]
        public async Task<IActionResult> Index(string username, string filter)
        {
            UserPostsViewModel model = new UserPostsViewModel
            {
                Username = username,
            };

            if (filter == UserPostsFilter.Liked.ToString())
            {
                model.Action = UserPostsFilter.Liked.ToString();
                model.Posts = await this.userPostsService.ExtractLikedPostsByUsername(username);
            }
            else if (filter == UserPostsFilter.Created.ToString())
            {
                model.Action = UserPostsFilter.Created.ToString();
                model.Posts = await this.userPostsService.ExtractCreatedPostsByUsername(username);
            }

            return this.View(model);
        }
    }
}