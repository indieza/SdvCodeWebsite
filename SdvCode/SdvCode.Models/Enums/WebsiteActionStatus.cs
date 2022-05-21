namespace SdvCode.Models.Enums
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    public enum WebsiteActionStatus
    {
        [Display(Name = "Unread")]
        Unread = 1,

        [Display(Name = "Read")]
        Read = 2,

        [Display(Name = "Pinned")]
        Pinned = 3,
    }
}