using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace CoursePlatform.ViewModels
{
    public class User
    {

        public int UserId { get; set; }

        [Display(Name = "الاسم")]
        [Required(ErrorMessage = "الاسم مطلوب")]
        public string UserName { get; set; }

        [Display(Name = "الايميل")]
        [EmailAddress(ErrorMessage = "الايميل غير صحيح")]
        public string UserEmail { get; set; }

        [Display(Name = "الصلاحية")]
        [Required(ErrorMessage = "صلاحية الكورس مطلوب")]
        public int? CoursePrivilege { get; set; }

        [Display(Name = "صلاحية الكورس")]
        public string CourseName { get; set; }

        [Display(Name = "ادمن")]
        public bool IsAdmin { get; set; }

        [Display(Name = "كلمة المرور")]
        [DataType(DataType.Password)]
        [Required(ErrorMessage = "كلمة المرور مطلوبة")]
        [RegularExpression(@"^(?=.*[a-z])(?=.*\d).{6,}$", ErrorMessage = "كلمة المرور يجب ان لا تقل عن 6 حروف وارقام")]
        public string UserPassword { get; set; }

        public int? CreatorId { get; set; }

        [Display(Name = "تاريخ الاضافة")]
        public DateTime CreationDate { get; set; }

        [Display(Name = "تاريخ التعديل")]
        public DateTime? ModficationDate { get; set; }

        [Display(Name = "المضيف")]
        public string CreatorName { get; set; }

        [Display(Name = "اخر معدل")]
        public string ModifiedName { get; set; }
    }
}