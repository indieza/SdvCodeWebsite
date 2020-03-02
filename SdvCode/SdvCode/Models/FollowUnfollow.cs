using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace SdvCode.Models
{
    public class FollowUnfollow
    {
        [Key]
        public string PersonId { get; set; }

        public string FollowerId { get; set; }
    }
}