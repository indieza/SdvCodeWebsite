using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace SdvCode.Data.Models
{
    public class FollowUnfollow
    {
        public string PersonId { get; set; }

        public string FollowerId { get; set; }

        public bool IsFollowed { get; set; }
    }
}