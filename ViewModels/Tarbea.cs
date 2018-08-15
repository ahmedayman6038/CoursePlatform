using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace CoursePlatform.ViewModels
{
    public class Tarbea : CourseRegistration
    {
        [Display(Name = "القسم")]
        [Required(ErrorMessage = "القسم مطلوب")]
        public string Section { get; set; }

        [Display(Name = "العنوان")]
        [Required(ErrorMessage = "العنوان مطلوب")]
        public string Address { get; set; }

        [Display(Name = "رقم الهاتف")]
        [RegularExpression(@"^\(?([0-9]{3})\)?[-. ]?([0-9]{3})[-. ]?([0-9]{4})$", ErrorMessage = "تليفون المنزل غير صحيح يجب ادخاء كود المدينة اولا")]
        public string TelephoneNumber { get; set; }
    }
}