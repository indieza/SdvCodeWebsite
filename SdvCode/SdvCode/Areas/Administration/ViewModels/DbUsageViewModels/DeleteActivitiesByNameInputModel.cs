using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace SdvCode.Areas.Administration.ViewModels.DbUsageViewModels
{
    public class DeleteActivitiesByNameInputModel
    {
        [Required]
        public string ActivityName { get; set; }
    }
}