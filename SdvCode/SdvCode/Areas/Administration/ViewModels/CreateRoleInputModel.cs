using SdvCode.Areas.Administration.Models.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace SdvCode.Areas.Administration.ViewModels
{
    public class CreateRoleInputModel
    {
        [Required]
        public string Role { get; set; }
    }
}