using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;
using CoursePlatform.Helper;
using CoursePlatform.Models;
using CoursePlatform.ViewModels;


namespace CoursePlatform.Controllers
{
    public class HomeController : Controller
    {
        private CoursePlatformEntities db = new CoursePlatformEntities();

        public ActionResult Index()
        {
            ViewBag.Courses = db.CP_Course.OrderByDescending(c => c.ID).ToList();
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Contact(Email email)
        {
            var success = "تم ارسال الرسالة بنجاح";
            var faild = "حدث خطأ ما اثناء ارسال الرسالة";
            if (ModelState.IsValid)
            {
                await MailHelper.SendContactMail(email.FromName, email.FromEmail, email.Title, email.Message);
                return Content(success);
            }
            return Content(faild);
        }

    }
}