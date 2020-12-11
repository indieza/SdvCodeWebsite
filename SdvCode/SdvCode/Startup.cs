// Copyright (c) SDV Code Project. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace SdvCode
{
    using System;
    using System.Linq;
    using System.Net.Http;
    using AutoMapper;
    using Blazored.LocalStorage;
    using Blazored.SessionStorage;
    using BlazorStrap;
    using CloudinaryDotNet;
    using Hangfire;
    using Hangfire.Dashboard;
    using Hangfire.SqlServer;
    using Microsoft.AspNetCore.Builder;
    using Microsoft.AspNetCore.Components;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Identity.UI.Services;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Hosting;
    using Microsoft.Extensions.ML;
    using OfficeOpenXml;
    using SdvCode.Areas.Administration.Services.AddChatSticker;
    using SdvCode.Areas.Administration.Services.AddChatStickerType;
    using SdvCode.Areas.Administration.Services.AddChatTheme;
    using SdvCode.Areas.Administration.Services.AddEmoji;
    using SdvCode.Areas.Administration.Services.AddEmojis;
    using SdvCode.Areas.Administration.Services.AddEmojiWithSkin;
    using SdvCode.Areas.Administration.Services.AllEmojis;
    using SdvCode.Areas.Administration.Services.BlogAddons;
    using SdvCode.Areas.Administration.Services.Dashboard;
    using SdvCode.Areas.Administration.Services.DbUsage;
    using SdvCode.Areas.Administration.Services.DeleteChatTheme;
    using SdvCode.Areas.Administration.Services.DeleteEmoji;
    using SdvCode.Areas.Administration.Services.DeleteEmojisByType;
    using SdvCode.Areas.Administration.Services.EditChatStickerType;
    using SdvCode.Areas.Administration.Services.EditChatTheme;
    using SdvCode.Areas.Administration.Services.EditEmoji;
    using SdvCode.Areas.Administration.Services.EditEmojiPosition;
    using SdvCode.Areas.Administration.Services.PendingComments;
    using SdvCode.Areas.Administration.Services.PendingPosts;
    using SdvCode.Areas.Administration.Services.Shop.Orders;
    using SdvCode.Areas.Administration.Services.Shop.ShopDbUsage;
    using SdvCode.Areas.Administration.Services.SiteReports.BlogReports;
    using SdvCode.Areas.Administration.Services.SiteReports.ShopReports;
    using SdvCode.Areas.Administration.Services.UserPenalties;
    using SdvCode.Areas.Administration.Services.UsersInformation;
    using SdvCode.Areas.Editor.Services.Category;
    using SdvCode.Areas.Editor.Services.Comment;
    using SdvCode.Areas.Editor.Services.Post;
    using SdvCode.Areas.PrivateChat.Services.ChatMessagesDbUsage;
    using SdvCode.Areas.PrivateChat.Services.PrivateChat;
    using SdvCode.Areas.SdvShop.Services.Category;
    using SdvCode.Areas.SdvShop.Services.Order;
    using SdvCode.Areas.SdvShop.Services.Product;
    using SdvCode.Areas.SdvShop.Services.ProductComment;
    using SdvCode.Areas.SdvShop.Services.ProductReview;
    using SdvCode.Areas.SdvShop.Services.States.FavoriteProducts;
    using SdvCode.Areas.SdvShop.Services.States.ShoppingCart;
    using SdvCode.Areas.SdvShop.Services.TrackOrder;
    using SdvCode.Areas.UserNotifications.Services;
    using SdvCode.Areas.UserNotifications.Services.NotificationDbUsage;
    using SdvCode.Constraints;
    using SdvCode.Data;
    using SdvCode.Hubs;
    using SdvCode.MlModels.CommentModels;
    using SdvCode.MlModels.PostModels;
    using SdvCode.Models.User;
    using SdvCode.SecurityModels;
    using SdvCode.Services.AllCategories;
    using SdvCode.Services.Blog;
    using SdvCode.Services.Category;
    using SdvCode.Services.Cloud;
    using SdvCode.Services.Comment;
    using SdvCode.Services.Contact;
    using SdvCode.Services.Home;
    using SdvCode.Services.Post;
    using SdvCode.Services.Profile;
    using SdvCode.Services.Profile.Pagination.AllUsers.AllAdministrators;
    using SdvCode.Services.Profile.Pagination.AllUsers.AllUsersTab;
    using SdvCode.Services.Profile.Pagination.AllUsers.BannedUsers;
    using SdvCode.Services.Profile.Pagination.AllUsers.RecommendedUsers;
    using SdvCode.Services.Profile.Pagination.Profile;
    using SdvCode.Services.RecommendedFriends;
    using SdvCode.Services.Tag;
    using SdvCode.Services.UserActivitesDbUsage.AllActivities;
    using SdvCode.Services.UserActivitesDbUsage.FollowActivities;
    using SdvCode.Services.UserPosts;
    using Twilio;

    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            this.Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            // Initialize ApplicationUser and DdContext
            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(
                    this.Configuration.GetConnectionString("DefaultConnection")));
            services.AddIdentity<ApplicationUser, ApplicationRole>(options =>
            {
                options.SignIn.RequireConfirmedAccount = true;
                options.Password.RequireUppercase = false;
                options.Password.RequiredLength = GlobalConstants.PasswordRequiredLength;
                options.Password.RequiredUniqueChars = 0;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequireLowercase = false;
            })
            .AddEntityFrameworkStores<ApplicationDbContext>()
            .AddDefaultTokenProviders();

            services.ConfigureApplicationCookie(options =>
            {
                options.AccessDeniedPath = "/Identity/Account/AccessDenied";
                options.Cookie.HttpOnly = true;
                options.LoginPath = "/Identity/Account/Login";
                options.SlidingExpiration = true;
            });

            services.AddAntiforgery(options =>
            {
                options.HeaderName = "X-CSRF-TOKEN";
            });

            // Configuration for update cookies when user is added in Role!!!
            services.Configure<SecurityStampValidatorOptions>(options =>
            {
                options.ValidationInterval = TimeSpan.FromMinutes(0);
            });

            // Social Network Authentication
            services.AddAuthentication()
                .AddFacebook(facebookOptions =>
                {
                    facebookOptions.AppId = this.Configuration["Authentication:Facebook:AppId"];
                    facebookOptions.AppSecret = this.Configuration["Authentication:Facebook:AppSecret"];
                })
                .AddGoogle(googleOptions =>
                {
                    googleOptions.ClientId = this.Configuration["Authentication:Google:ClientId"];
                    googleOptions.ClientSecret = this.Configuration["Authentication:Google:ClientSecret"];
                })
                .AddTwitter(twitterOptions =>
                {
                    twitterOptions.ConsumerKey = this.Configuration["Authentication:Twitter:ApiKey"];
                    twitterOptions.ConsumerSecret = this.Configuration["Authentication:Twitter:ApiSecretKey"];
                    twitterOptions.RetrieveUserDetails = true;
                });

            // Cloudinary Authentication
            var cloudinaryAccount = new CloudinaryDotNet.Account(
                this.Configuration["Cloudinary:CloudName"],
                this.Configuration["Cloudinary:ApiKey"],
                this.Configuration["Cloudinary:ApiSecret"]);
            var cloudinary = new Cloudinary(cloudinaryAccount);
            services.AddSingleton(cloudinary);

            // Twilio Authentication
            var accountSid = this.Configuration["Twilio:AccountSID"];
            var authToken = this.Configuration["Twilio:AuthToken"];
            TwilioClient.Init(accountSid, authToken);
            services.Configure<TwilioVerifySettings>(this.Configuration.GetSection("Twilio"));

            services.AddTransient<ApplicationDbContext>();

            // Register Administration Services
            services.AddTransient<IDashboardService, DashboardService>();
            services.AddTransient<IDbUsageService, DbUsageService>();
            services.AddTransient<IBlogAddonsService, BlogAddonsService>();
            services.AddTransient<IUsersPenaltiesService, UsersPenaltiesService>();
            services.AddTransient<IEditCategoryService, EditCategoryService>();
            services.AddTransient<IAddCategoryService, AddCategoryService>();
            services.AddTransient<IEditorPostService, EditorPostService>();
            services.AddTransient<IEditorCommentService, EditorCommentService>();
            services.AddTransient<IPendingPostsService, PendingPostsService>();
            services.AddTransient<IPendingCommentsService, PendingCommentsService>();
            services.AddTransient<IBlogPostReport, BlogPostReport>();
            services.AddTransient<IShopReport, ShopReport>();
            services.AddTransient<IUsersInformationService, UsersInformationService>();
            services.AddTransient<IAddEmojiService, AddEmojiService>();
            services.AddTransient<IEditEmojiService, EditEmojiService>();
            services.AddTransient<IDeleteEmojiService, DeleteEmojiService>();
            services.AddTransient<IEditEmojiPositionService, EditEmojiPositionService>();
            services.AddTransient<IAllEmojisService, AllEmojisService>();
            services.AddTransient<IAddChatThemeService, AddChatThemeService>();
            services.AddTransient<IDeleteChatThemeService, DeleteChatThemeService>();
            services.AddTransient<IEditChatThemeService, EditChatThemeService>();
            services.AddTransient<IAddEmojisService, AddEmojisService>();
            services.AddTransient<IAddEmojiWithSkinService, AddEmojiWithSkinService>();
            services.AddTransient<IDeleteEmojisByTypeService, DeleteEmojisByTypeService>();
            services.AddTransient<IAddChatStickerTypeService, AddChatStickerTypeService>();
            services.AddTransient<IAddChatStickerService, AddChatStickerService>();
            services.AddTransient<IEditChatStickerTypeService, EditChatStickerTypeService>();

            // Register Logic Services
            services.AddScoped<IContactService, ContactService>();
            services.AddTransient<IEmailSender, EmailSender>();
            services.AddTransient<IProfileService, ProfileService>();
            services.AddTransient<IHomeService, HomeService>();
            services.AddTransient<IBlogService, BlogService>();
            services.AddTransient<IBlogComponentService, BlogComponentService>();
            services.AddTransient<IPostService, PostService>();
            services.AddTransient<ICategoryService, CategoryService>();
            services.AddTransient<ITagService, TagService>();
            services.AddTransient<IUserPostsService, UserPostsService>();
            services.AddTransient<IPrivateChatService, PrivateChatService>();
            services.AddTransient<ICommentService, CommentService>();
            services.AddTransient<IAllCategoriesService, AllCategoriesService>();

            // Register User Notification Services
            services.AddTransient<INotificationService, NotificationService>();

            // Register Pagination Services
            services.AddTransient<IProfileActivitiesService, ProfileActivitiesService>();
            services.AddTransient<IProfileFollowersService, ProfileFollowersService>();
            services.AddTransient<IProfileFollowingService, ProfileFollowingService>();
            services.AddTransient<IProfileFavoritesService, ProfileFavoritesService>();
            services.AddTransient<IProfilePendingPostsService, ProfilePendingPostsService>();
            services.AddTransient<IProfileBannedPostsService, ProfileBannedPostsService>();
            services.AddTransient<IAllUsersService, AllUsersService>();
            services.AddTransient<IBannedUsersService, BannedUsersService>();
            services.AddTransient<IRecommendedUsersService, RecommendedUsersService>();
            services.AddTransient<IAllAdministratorsService, AllAdministratorsService>();

            // Register Shop Services
            services.AddTransient<IShopDbUsageService, ShopDbUsageService>();
            services.AddTransient<IOrdersService, OrdersService>();
            services.AddTransient<IProductCategoryService, ProductCategoryService>();
            services.AddTransient<IProductService, ProductService>();
            services.AddTransient<IProductCommentService, ProductCommentService>();
            services.AddTransient<IProductReviewService, ProductReviewService>();
            services.AddTransient<IOrderService, OrderService>();
            services.AddTransient<ITrackOrder, TrackOrder>();
            services.AddScoped<ShoppingCartState>();
            services.AddScoped<FavoriteProductsState>();

            // Register ML Models
            services.AddPredictionEnginePool<BlogPostModelInput, BlogPostModelOutput>()
                .FromFile("MlModels/PostModels/BlogPostMLModel.zip");
            services.AddPredictionEnginePool<BlogCommentModelInput, BlogCommentModelOutput>()
                .FromFile("MlModels/CommentModels/BlogCommentMLModel.zip");

            // Configure ReCaptch Settings
            services.Configure<ReCaptchSettings>(this.Configuration.GetSection("GoogleReCAPTCHA"));

            // Add Hangfire services.
            services.AddHangfire(configuration => configuration
                .SetDataCompatibilityLevel(CompatibilityLevel.Version_170)
                .UseSimpleAssemblyNameTypeSerializer()
                .UseRecommendedSerializerSettings()
                .UseSqlServerStorage(
                this.Configuration.GetConnectionString("DefaultConnection"),
                new SqlServerStorageOptions
                {
                    CommandBatchMaxTimeout = TimeSpan.FromMinutes(5),
                    SlidingInvisibilityTimeout = TimeSpan.FromMinutes(5),
                    QueuePollInterval = TimeSpan.Zero,
                    UseRecommendedIsolationLevel = true,
                    UsePageLocksOnDequeue = true,
                    DisableGlobalLocks = true,
                }));

            // Add the processing server as IHostedService
            services.AddHangfireServer();

            // Add Server Side Blazor
            services.AddServerSideBlazor().AddCircuitOptions(options => { options.DetailedErrors = true; });

            // Server Side Blazor doesn't register HttpClient by default
            if (!services.Any(x => x.ServiceType == typeof(HttpClient)))
            {
                // Setup HttpClient for server side in a client side compatible fashion
                services.AddScoped<HttpClient>(s =>
                {
                    // Creating the URI helper needs to wait until the JS Runtime is initialized, so defer it.
                    var uriHelper = s.GetRequiredService<NavigationManager>();
                    return new HttpClient
                    {
                        BaseAddress = new Uri(uriHelper.BaseUri),
                    };
                });
            }

            // Register OfficeOpenXml License
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

            // Add Blazor Session and Local Storages
            services.AddBlazoredSessionStorage();
            services.AddBlazoredLocalStorage();

            services.AddBootstrapCss();

            services.AddHttpClient();
            services.AddAutoMapper(typeof(Startup));
            services.AddControllersWithViews();
            services.AddRazorPages();
            services.AddSignalR();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, IRecurringJobManager recurringJobManager)
        {
            this.SeedHangfireJobs(recurringJobManager);

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseDatabaseErrorPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");

                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseStatusCodePagesWithRedirects("/Error/{0}");
            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();
            app.UseCookiePolicy();

            app.UseAuthentication();
            app.UseAuthorization();

            if (!env.IsProduction())
            {
                app.UseHangfireServer(new BackgroundJobServerOptions { WorkerCount = 2 });
                app.UseHangfireDashboard(
                    "/Administration/UsersPenalties/HangFire",
                    new DashboardOptions { Authorization = new[] { new HangfireAuthorizationFilter() } });
            }

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                      name: "areas",
                      pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}");

                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");

                endpoints.MapHub<PrivateChatHub>("/privateChatHub");
                endpoints.MapHub<NotificationHub>("/notificationHub");
                endpoints.MapHub<UserStatusHub>("/userStatusHub");

                endpoints.MapRazorPages();
                endpoints.MapBlazorHub();
                endpoints.MapFallbackToAreaController("Index", "Shop", "SdvShop");
            });
        }

        private void SeedHangfireJobs(IRecurringJobManager recurringJobManager)
        {
            // Edit recommended friend list for each user
            recurringJobManager
                .AddOrUpdate<RecommendedFriends>(
                "RecommendedFriends",
                x => x.AddRecomendedFrinds(),
                Cron.Weekly);

            // Delete all follow-unfollow activities
            recurringJobManager
                .AddOrUpdate<UserFollowActivitiesDbUsage>(
                "UserActivitiesDbSavage",
                x => x.DeleteFollowActivites(),
                Cron.Monthly);

            // Delete all user activities
            recurringJobManager
                .AddOrUpdate<AllActivities>("AllActivities", x => x.DeleteAllActivites(), Cron.Yearly);

            // Delete all chat messages
            recurringJobManager
                .AddOrUpdate<DeleteMessages>("DeleteMessages", x => x.DeleteAllChatMessages(), Cron.Yearly);

            // Delete all user notification
            recurringJobManager
                .AddOrUpdate<NotificationDbUsage>("DeleteNotifications", x => x.DeleteNotifications(), Cron.Yearly);
        }

        public class HangfireAuthorizationFilter : IDashboardAuthorizationFilter
        {
            public bool Authorize(DashboardContext context)
            {
                var httpContext = context.GetHttpContext();
                return httpContext.User.IsInRole(GlobalConstants.AdministratorRole);
            }
        }
    }
}