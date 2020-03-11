using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SdvCode.SecurityModels
{
    public class ReCaptchSettings
    {
        public string SiteKey { get; set; }

        public string SecretKey { get; set; }
    }
}