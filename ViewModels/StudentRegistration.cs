using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace CoursePlatform.ViewModels
{
    public class StudentRegistration
    {
        [Display(Name = "رقم التسجيل")]
        public int RegisterId { get; set; }

        public DateTime DurationFrom { get; set; }

        public DateTime DurationTo { get; set; }

        [Display(Name = "الكلية")]
        public string FacultyName { get; set; }

        public int MyProperty { get; set; }
        [Display(Name = "الاسم")]
        public string StudentName { get; set; }

        [Display(Name = "السنة الدراسية")]
        public string AcademicYear { get; set; }

        [Display(Name = "الرقم القومى")]
        public string NationalId { get; set; }

        [Display(Name = "مسلسل الدورة")]
        public int? DurationSerial { get; set; }

        [Display(Name = "النتيجة")]
        public int? Result { get; set; }

        public string ResultName { get; set; }

        [Display(Name = "الجامعة")]
        public string UniversityName { get; set; }
    }
}