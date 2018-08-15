using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace CoursePlatform.ViewModels
{
    public class ForgetPassword
    {
        [EmailAddress(ErrorMessage = "الايميل غير صحيح")]
        [Required(ErrorMessage = "البريد الالكترونى مطلوب")]
        public string StudentEmail { get; set; }
    }
}