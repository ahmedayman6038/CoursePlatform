using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace CoursePlatform.ViewModels
{
    public class Register
    {
        [Required(ErrorMessage = "الكود مطلوب")]
        [Range(10000000000000, 99999999999999, ErrorMessage = "الكود يتكون من 14 رقم فقط")]
        public string StudentCode { get; set; }

        public string StudentName { get; set; }

        public string StudentFaculty { get; set; }

        [Required(ErrorMessage = "الايميل مطلوب")]
        [EmailAddress(ErrorMessage = "الايميل غير صحيح")]
        public string StudentEmail { get; set; }

        [Required(ErrorMessage = "الرقم القومى مطلوب")]
        [Range(10000000000000, 99999999999999, ErrorMessage = "الرقم القومى يتكون من 14 رقم فقط")]
        public string NationalID { get; set; }

        [DataType(DataType.Password)]
        [Required(ErrorMessage = "كلمة المرور مطلوبة")]
        [RegularExpression(@"^(?=.*[a-z])(?=.*\d).{6,}$", ErrorMessage = "كلمة المرور يجب ان لا تقل عن 6 حروف وارقام")]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "كلمة المرور غير متوافقة")]
        public string ConfirmPassword { get; set; }

        [Required(ErrorMessage = "مستوى الطالب مطلوب")]
        public string LevelName { get; set; }

        [Required(ErrorMessage = "رقم الهاتف مطلوب")]
        [RegularExpression(@"^\(?([0-9]{3})\)?[-. ]?([0-9]{3})[-. ]?([0-9]{5})$", ErrorMessage = "رقم الهاتف غير صحيح")]
        public string PhoneNumber { get; set; }

    }
}