using SdvCode.Data.Models.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace SdvCode.Data.Models
{
    public class UserAction
    {
        [Key]
        public int Id { get; set; }

        [ForeignKey(nameof(ApplicationUser)), Required]
        public string ApplicationUserId { get; set; }

        public ApplicationUser ApplicationUser { get; set; }

        [Required]
        public UserActionsType Action { get; set; }

        [Required]
        public DateTime ActionDate { get; set; }

        public string PersonUsername { get; set; }

        public string PersonProfileImageUrl { get; set; }

        public string FollowerUsername { get; set; }

        public string FollowerProfileImageUrl { get; set; }

        public string ProfileImageUrl { get; set; }

        public string CoverImageUrl { get; set; }
    }
}