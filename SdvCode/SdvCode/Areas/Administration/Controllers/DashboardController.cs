using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using SdvCode.Areas.Administration.Services;
using SdvCode.Areas.Administration.ViewModels;
using SdvCode.Services;

namespace SdvCode.Areas.Administration.Controllers
{
    [Area("Administration")]
    public class DashboardController : Controller
    {
        private readonly IAdministrationService administrationService;

        public DashboardController(IAdministrationService administrationService)
        {
            this.administrationService = administrationService;
        }

        public IActionResult Index()
        {
            DashboardViewModel dashboard = this.administrationService.GetDashboardInformation();
            return View(dashboard);
        }
    }
}