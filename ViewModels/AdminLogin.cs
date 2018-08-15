using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace CoursePlatform.ViewModels
{
    public class AdminLogin
    {
        [Required(ErrorMessage = "اسم المستخدم مطلوب")]
        public string Name { get; set; }

        [Required(ErrorMessage = "كلمة المرور مطلوبة")]
        public string Password { get; set; }

        [Display(Name = "تذكرنى")]
        public bool RememberMe { get; set; }
    }
}