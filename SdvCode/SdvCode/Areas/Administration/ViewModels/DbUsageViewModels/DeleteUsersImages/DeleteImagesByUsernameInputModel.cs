using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace SdvCode.Areas.Administration.ViewModels.DbUsageViewModels.DeleteUsersImages
{
    public class DeleteImagesByUsernameInputModel
    {
        [Required]
        public string Username { get; set; }
    }
}