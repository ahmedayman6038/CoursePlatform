using CoursePlatform.Attributes;
using CoursePlatform.Helper;
using System;
using System.Linq;
using System.Web.Mvc;
using CoursePlatform.Models;
using System.Web;
using CoursePlatform.ViewModels;

namespace CKESWORK.Controllers
{
    [UserAuthorize(Role.Admin, Role.Tarbea)]
    public class DashboardController : Controller
    {
        private CoursePlatformEntities db = new CoursePlatformEntities();

        public ActionResult Index()
        {
            return View();
        }

        [AllowAnonymous]
        public ActionResult Login(string returnUrl)
        {
            HttpCookie UserCookie = Request.Cookies["c_user"];
            if(UserCookie != null)
            {
                var uid = Int32.Parse(HashHelper.Decrypt(UserCookie.Values["u_id"]));
                var upass = HashHelper.Decrypt(UserCookie.Values["u_pa"]);
                var user = db.CP_User.Find(uid);
                if (user != null && upass == user.Password)
                {
                    Session["UserName"] = user.Name;
                    Session["UserId"] = user.ID;
                    Session["CourseId"] = user.CoursePrivilege;
                    if (user.CoursePrivilege == null)
                    {
                        Session["UserType"] = Enum.GetName(typeof(Role), 2);
                    }
                    else
                    {
                        dynamic course = SystemHelper.GetCourseObject((int)user.CoursePrivilege);
                        Session["UserType"] = course.GetType().Name.ToString();
                    }
                    return RedirectToLocal(returnUrl);
                }
            }
            ViewBag.ReturnUrl = returnUrl;
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult Login([Bind(Include = "Name,Password,RememberMe")] AdminLogin login, string returnUrl)
        {
            if (ModelState.IsValid)
            {
                string hahsedpassword = HashHelper.Encrypt(login.Password);
                var registered = db.CP_User.Where(s => s.Name == login.Name && s.Password == hahsedpassword).FirstOrDefault();
                if (registered == null)
                {
                    ViewBag.ModelErrors = "غير مسجل او التحقق من البيانات";
                    return View(login);
                }
                Session["UserName"] = registered.Name;
                Session["UserId"] = registered.ID;
                Session["CourseId"] = registered.CoursePrivilege;
                if (registered.CoursePrivilege == null)
                {
                    Session["UserType"] = Enum.GetName(typeof(Role), 2);
                }
                else
                {
                    dynamic course = SystemHelper.GetCourseObject((int)registered.CoursePrivilege);
                    Session["UserType"] = course.GetType().Name.ToString();
                }
                if (login.RememberMe)
                {
                    HttpCookie UserCookie = new HttpCookie("c_user");
                    UserCookie.Values["u_id"] = HashHelper.Encrypt(registered.ID.ToString());
                    UserCookie.Values["u_pa"] = HashHelper.Encrypt(registered.Password);
                    UserCookie.Expires = DateTime.Now.AddYears(1);
                    Response.Cookies.Add(UserCookie);
                }
                return RedirectToLocal(returnUrl);
            }
            return View(login);
        }

        public ActionResult Logout()
        {
            HttpCookie UserCookie = Request.Cookies["c_user"];
            if(UserCookie != null)
            {
                UserCookie.Expires = DateTime.Now.AddDays(-1);
                Response.Cookies.Add(UserCookie);
            }
            Session["UserId"] = null;
            Session["UserName"] = null;
            Session["UserType"] = null;
            Session["CourseId"] = null;
            return RedirectToAction("Index", "Home");
        }

        private ActionResult RedirectToLocal(string returnUrl)
        {
            if (Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }
            return RedirectToAction("Index", "Dashboard");
        }
    }
}