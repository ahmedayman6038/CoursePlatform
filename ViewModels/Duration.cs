using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace CoursePlatform.ViewModels
{
    public class Duration
    {
        public int DurationId { get; set; }

        [Display(Name = "التاريخ من")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        [Required(ErrorMessage = "التاريخ من مطلوب")]
        public DateTime FromDate { get; set; }

        [Display(Name = "التاريخ الى")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        [Required(ErrorMessage = "التاريخ الى مطلوب")]
        public DateTime ToDate { get; set; }

        public int CourseId { get; set; }

        [Display(Name = "الحد الادنى")]
        public int? MinCapacity { get; set; }

        [Display(Name = "الحد الاقصى")]
        public int? MaxCapacity { get; set; }

        [Display(Name = "تفعيل")]
        public bool Activate { get; set; }

        [Display(Name = "عدد المسجلين")]
        public int? NumberRolled { get; set; }

        [Required(ErrorMessage = "الرقم التسلسلى مطلوب")]
        [Display(Name = "الرقم التسلسلى")]
        public int SerialFrom { get; set; }

        public int CreatorId { get; set; }

        [Display(Name = "تاريخ الاضافة")]
        public DateTime CreationDate { get; set; }

        [Display(Name = "تاريخ التعديل")]
        public DateTime? ModficationDate { get; set; }

        [Display(Name = "اسم الكورس")]
        public string CourseName { get; set; }

        [Display(Name = "المضيف")]
        public string CreatorName { get; set; }

        [Display(Name = "اخر معدل")]
        public string ModifiedName { get; set; }

        [Display(Name = "مغلق الدورة")]
        public string TerminatorName { get; set; }
    }
}