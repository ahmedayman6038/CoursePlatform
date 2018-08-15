using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;
using CoursePlatform.Attributes;
using CoursePlatform.Helper;
using CoursePlatform.ViewModels;
using CoursePlatform.Models;

namespace CKESWORK.Controllers
{
    [UserAuthorize(Role.Student)]
    public class AccountController : Controller
    {
        private CoursePlatformEntities db = new CoursePlatformEntities();

        private readonly List<Level> levels = new List<Level>()
           {
                new Level(){LevelCode=1,LevelName="المستوى الاول"},
                new Level(){LevelCode=2,LevelName="المستوى الثانى"},
                new Level(){LevelCode=3,LevelName="المستوى الثالث"},
                new Level(){LevelCode=4,LevelName="المستوى الرابع"},
                new Level(){LevelCode=5,LevelName="المستوى الخامس"}
            };

        public ActionResult Logout()
        {
            Session["UserId"] = null;
            Session["UserName"] = null;
            Session["UserType"] = null;
            return RedirectToAction("Index","Home");
        }

        [AllowAnonymous]
        public ActionResult Register()
        {
            ViewBag.LevelName = new SelectList(levels, "LevelName", "LevelName");
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Register([Bind(Include = "StudentCode,StudentEmail,StudentName,StudentFaculty,NationalID,Password,ConfirmPassword,LevelName,PhoneNumber")] Register register)
        {
            if (ModelState.IsValid)
            {
                if(register.StudentName == null || register.StudentFaculty == null)
                {
                    ViewBag.ModelErrors = "كود الطالب غير صحيح";
                    ViewBag.LevelName = new SelectList(levels, "LevelName", "LevelName", register.LevelName);
                    return View(register);
                }
                CP_Registration registration = new CP_Registration();
                var pass = HashHelper.Encrypt(register.Password);
                var registered = db.CP_Registration.Where(s => s.StudentCode == register.StudentCode || s.StudentEmail == register.StudentEmail || s.Password == pass || s.NationalID == register.NationalID).FirstOrDefault();
                if (registered != null)
                {
                    if (registered.StudentCode == register.StudentCode)
                    {
                        ViewBag.ModelErrors = "كود الطالب مسجل مسبقا";
                    }
                    else if (registered.Password == pass)
                    {
                        ViewBag.ModelErrors = "كلمة المرور مسجل مسبقا";
                    }
                    else if (registered.StudentEmail == register.StudentEmail)
                    {
                        ViewBag.ModelErrors = "الايميل مسجل مسبقا";
                    }
                    else if (registered.NationalID == register.NationalID)
                    {
                        ViewBag.ModelErrors = "الرقم القومى مسجل مسبقا";
                    }
                    else
                    {
                        ViewBag.ModelErrors = "حدث خطا ما اثناء معالجة طلبك";
                    }
                    ViewBag.LevelName = new SelectList(levels, "LevelName", "LevelName", register.LevelName);
                    return View(register);
                }
                var hashedpassword = HashHelper.Encrypt(register.Password);
                registration.StudentCode = register.StudentCode;
                registration.StudentEmail = register.StudentEmail;
                registration.StudentName = register.StudentName;
                registration.StudentFaculty = register.StudentFaculty;
                registration.NationalID = register.NationalID;
                registration.Password = hashedpassword;
                registration.LevelName = register.LevelName;
                registration.PhoneNumber = register.PhoneNumber;
                db.CP_Registration.Add(registration);
                db.SaveChanges();
                var activationcode = HashHelper.Encrypt(register.StudentCode);
                await MailHelper.SendActivationMail(register.StudentEmail, Request.Url.Scheme, Request.Url.Host, Request.Url.Port.ToString(), activationcode);
                ViewBag.Message = "تم ارسال رابط التفعيل على البريد الالكترونى";
                return View("Message");
            }
            ViewBag.LevelName = new SelectList(levels, "LevelName", "LevelName", register.LevelName);
            return View(register);
        }

        private ActionResult RedirectToLocal(string returnUrl)
        {
            if (Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }
            return RedirectToAction("Index", "Home");
        }

        [AllowAnonymous]
        public ActionResult Login(string returnUrl)
        {
            ViewBag.ReturnUrl = returnUrl;
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Login([Bind(Include = "StudentCode,Password")] Login login, string returnUrl)
        {
            if (ModelState.IsValid)
            {
                string hahsedpassword = HashHelper.Encrypt(login.Password);
                var registered = db.CP_Registration.Where(s => s.StudentCode == login.StudentCode && s.Password == hahsedpassword).FirstOrDefault();
                if (registered == null)
                {
                    ViewBag.ModelErrors = "الطالب غير مسجل او التحقق من البيانات";
                    return View(login);
                }
                if (registered.ActivationStatus == false)
                {
                    var activationcode = HashHelper.Encrypt(registered.StudentCode);
                    await MailHelper.SendActivationMail(registered.StudentEmail, Request.Url.Scheme, Request.Url.Host, Request.Url.Port.ToString(), activationcode);
                    ViewBag.Message = "يجب تفعيل حسابك اولا قبل الدخول افحص بريدك الالكترونى";
                    return View("Message");
                }
                Session["UserId"] = registered.ID;
                Session["UserName"] = registered.StudentName;
                Session["UserType"] = Enum.GetName(typeof(Role), 1);
                return RedirectToLocal(returnUrl);
            }
            return View(login);
        }

        [AllowAnonymous]
        public ActionResult Activate(string code)
        {
            if (code != null)
            {
                try
                {
                    var StudentCode = HashHelper.Decrypt(code);
                    var registred = db.CP_Registration.Where(s => s.StudentCode == StudentCode).FirstOrDefault();
                    if (registred != null)
                    {
                        registred.ActivationStatus = true;
                        db.Entry(registred).State = EntityState.Modified;
                        db.SaveChanges();
                        ViewBag.Message = " تم تفعيل حسابك بنجاح لتسجيل الدخول <a href='/Account/Login'> اضغط هنا </a>";
                        return View("Message");
                    }
                }
                catch (Exception)
                {
                    ViewBag.Error = "حدث خطأء ما اثناء معالجة طلبك";
                    return View("Error");
                }
            }
            ViewBag.Error = "حدث خطأء ما اثناء معالجة طلبك";
            return View("Error");
        }

        [AllowAnonymous]
        public ActionResult ForgetPassword()
        {
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ForgetPassword([Bind(Include = "StudentEmail")] ForgetPassword forget)
        {
            if (ModelState.IsValid)
            {
                var registered = db.CP_Registration.Where(s => s.StudentEmail == forget.StudentEmail).FirstOrDefault();
                if(registered == null)
                {
                    ViewBag.ModelErrors = "الطالب غير مسجل او التحقق من البيانات";
                    return View(forget);
                }
                if (registered.ActivationStatus == false)
                {
                    var activationcode = HashHelper.Encrypt(registered.StudentCode);
                    await MailHelper.SendActivationMail(registered.StudentEmail, Request.Url.Scheme, Request.Url.Host, Request.Url.Port.ToString(), activationcode);
                    ViewBag.Message = "يجب تفعيل حسابك اولا قبل حتى يمكنك تغيير كلمة المرور افحص بريدك الالكترونى";
                    return View("Message");
                }
                var resetcode = HashHelper.Encrypt(registered.StudentCode);
                await MailHelper.SendResetPasswordMail(forget.StudentEmail, Request.Url.Scheme, Request.Url.Host, Request.Url.Port.ToString(), resetcode);
                ViewBag.Message = "تم ارسال رابط تغيير كلمة المرور على البريد الالكترونى";
                return View("Message");
            }
            return View(forget);
        }

        [AllowAnonymous]
        public ActionResult ResetPassword(string code)
        {
            if(code == null)
            {
                ViewBag.Error = "حدث خطأء ما اثناء معالجة طلبك";
                return View("Error");
            }
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult ResetPassword([Bind(Include = "Code,Password,ConfirmPassword")] ResetPassword reset)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var StudentCode = HashHelper.Decrypt(reset.Code);
                    var registred = db.CP_Registration.Where(s => s.StudentCode == StudentCode).FirstOrDefault();
                    if (registred == null)
                    {
                        ViewBag.Error = "حدث خطأء ما اثناء معالجة طلبك";
                        return View("Error");
                    }
                    var hashedpassword = HashHelper.Encrypt(reset.Password);
                    var registertions = db.CP_Registration.Where(s => s.Password == hashedpassword).FirstOrDefault();
                    if (registertions != null)
                    {
                        ViewBag.ModelErrors = "كلمة المرور موجودة";
                        return View(reset);
                    }
                    registred.Password = hashedpassword;
                    db.Entry(registred).State = EntityState.Modified;
                    db.SaveChanges();
                    ViewBag.Message = " تم تغيير كلمة المرور بنجاح لتسجيل الدخول  <a href='/Account/Login'> اضغط هنا </a>";
                    return View("Message");
                }
                catch
                {
                    ViewBag.Error = "حدث خطأء ما اثناء معالجة طلبك";
                    return View("Error");
                }
            }
            return View(reset);
        }
    }
}