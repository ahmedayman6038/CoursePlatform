using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CoursePlatform.ViewModels
{
    public class Course
    {
        public int CourseId { get; set; }

        [Display(Name ="اسم الكورس")]
        [Required(ErrorMessage = "برجاء كتابة الاسم")]
        public string CourseName { get; set; }

        [Display(Name = "نوع المتقدمين")]
        public string GenderName { get; set; }

        [Display(Name = "نوع المتقدمين")]
        [Required(ErrorMessage = "برجاء اختيار نوع المتقدمين")]
        public int GenderAllowed { get; set; }

        [AllowHtml]
        [Display(Name = "تفاصيل الكورس")]
        public string CourseDetails { get; set; }

        [Display(Name = "صورة الكورس")]
        public string CourseImage { get; set; }

        public int CreatorId { get; set; }

        [Display(Name = "تاريخ الاضافة")]
        public DateTime CreationDate { get; set; }

        [Display(Name = "تاريخ التعديل")]
        public DateTime? ModficationDate { get; set; }

        [Display(Name = "المضيف")]
        public string CreatorName { get; set; }

        [Display(Name = "اخر معدل")]
        public string ModifiedName { get; set; }

        public int DurationsCount { get; set; }
    }
}