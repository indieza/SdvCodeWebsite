using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using SdvCode.Services;
using SdvCode.ViewModels.Administration;

namespace SdvCode.Controllers
{
    public class AdministrationController : Controller
    {
        private readonly IAdministrationService administrationService;

        public AdministrationController(IAdministrationService administrationService)
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