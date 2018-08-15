using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace CoursePlatform.ViewModels
{
    public class Login
    {
        [Required(ErrorMessage = "الكود مطلوب")]
        public string StudentCode { get; set; }

        [Required(ErrorMessage = "كلمة المرور مطلوبة")]
        public string Password { get; set; }
    }
}