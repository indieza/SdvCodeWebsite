using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace SdvCode.Areas.Administration.ViewModels
{
    public class RemoveUserFromRoleInputModel
    {
        [Required]
        public string Role { get; set; }

        [Required]
        public string Username { get; set; }
    }
}