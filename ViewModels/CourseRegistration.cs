using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace CoursePlatform.ViewModels
{
    public class CourseRegistration
    {
        [Display(Name = "الفترة")]
        [Required(ErrorMessage = "الفترة مطلوبة")]
        public int DurationID { get; set; }

        [Display(Name = "الفترة من")]
        public DateTime DurationFrom { get; set; }

        [Display(Name = "الفترة الى")]
        public DateTime DurationTo { get; set; }

        [Display(Name = "الرقم القومى")]
        [Required(ErrorMessage = "الرقم القومى مطلوب")]
        [Range(10000000000000, 99999999999999, ErrorMessage = "الرقم القومى يتكون من 14 رقم فقط")]
        public string NationalID { get; set; }

        [Display(Name = "اسم الطالب")]
        [Required(ErrorMessage = "اسم الطالب مطلوب")]
        public string StudentName { get; set; }

        [Display(Name = "اسم الكلية")]
        [Required(ErrorMessage = "اسم الكلية مطلوب")]
        public string StudentFacuty { get; set; }

        [Display(Name = "اسم الجامعة")]
        [Required(ErrorMessage = "اسم الجامعة مطلوب")]
        public string StudentUniversity { get; set; }

        [Display(Name = "المستوى")]
        [Required(ErrorMessage = "المستوى مطلوب")]
        public string LevelName { get; set; }


        [Display(Name = "رقم المحمول")]
        [Required(ErrorMessage = "التلفون المحمول مطلوب")]
        [RegularExpression(@"^\(?([0-9]{3})\)?[-. ]?([0-9]{3})[-. ]?([0-9]{5})$", ErrorMessage = "رقم المحمول غير صحيح")]
        public string PhoneNumber { get; set; }

        [Display(Name = "الرقم المسلسل")]
        public int? SerialNumber { get; set; }

        public int? ResultId { get; set; }
    }
}