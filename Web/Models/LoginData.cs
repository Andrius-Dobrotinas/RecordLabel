using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using RecordLabel.Web.Localization;

namespace RecordLabel.Web
{
    public class LoginData
    {
        [Required]
        [Display(ResourceType = typeof(AdminApplicationLocalization), Name = "Login_UserName")]
        public string UserName { get; set; }

        [Required]
        [Display(ResourceType = typeof(AdminApplicationLocalization), Name = "Login_Password")]
        public string Password { get; set; }

        [Display(ResourceType = typeof(AdminApplicationLocalization), Name = "Login_RememberMe")]
        public bool RememberMe { get; set; }
    }
}