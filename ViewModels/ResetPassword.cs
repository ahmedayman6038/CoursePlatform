using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace CoursePlatform.ViewModels
{
    public class ResetPassword
    {
        public string Code { get; set; }

        [DataType(DataType.Password)]
        [Required(ErrorMessage = "كلمة المرور مطلوبة")]
        [RegularExpression(@"^(?=.*[a-z])(?=.*\d).{6,}$", ErrorMessage = "كلمة المرور يجب ان لا تقل عن 6 حروف وارقام")]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "كلمة المرور غير متوافقة")]
        public string ConfirmPassword { get; set; }
    }
}