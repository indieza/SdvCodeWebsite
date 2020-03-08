using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace SdvCode.Areas.Administration.ViewModels
{
    public class AddUserInRoleInputModel
    {
        [Required]
        public string Username { get; set; }

        [Required]
        public string Role { get; set; }
    }
}