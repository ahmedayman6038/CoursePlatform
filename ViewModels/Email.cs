using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace CoursePlatform.ViewModels
{
    public class Email
    {
        [Required(ErrorMessage ="برجاء كتابة الاسم")]
        public string FromName { get; set; }

        [Required(ErrorMessage = "برجاء كتابة البريد الالكترونى")]
        [EmailAddress(ErrorMessage ="البريد الالكتورنى غير صحيح")]
        public string FromEmail { get; set; }

        [Required(ErrorMessage = "برجاء كتابة عنوان الرسالة")]
        public string Title { get; set; }

        [Required(ErrorMessage = "برجاء كتابة موضوع الرسالة")]
        [StringLength(300,ErrorMessage = "الحد الاقصى للرسالة 300 حرف")]
        public string Message { get; set; }
    }
}