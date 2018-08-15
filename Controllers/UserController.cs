using System;
using System.Linq;
using System.Web.Mvc;
using System.Data.Entity;
using CoursePlatform.Helper;
using System.Net;
using CoursePlatform.Attributes;
using CoursePlatform.Models;
using CoursePlatform.ViewModels;

namespace CKESWORK.Controllers
{
    [UserAuthorize(Role.Admin)]
    public class UserController : Controller
    {
        private CoursePlatformEntities db = new CoursePlatformEntities();

        public ActionResult Index()
        {
            var users = db.CP_User.Include(c => c.CoursePrivilege).Select(u => new User
            {
                UserId = u.ID,
                UserName = u.Name,
                UserEmail = u.Email,
                CourseName = u.CP_Course2.Name
            }).ToList();
            return View(users);
        }

        public ActionResult Create()
        {
            ViewBag.CoursePrivilege = new SelectList(db.CP_Course, "ID", "Name");
            return View();
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "UserName,UserEmail,IsAdmin,CoursePrivilege,UserPassword")] User user)
        {
            if (ModelState.IsValid)
            {
                var password = HashHelper.Encrypt(user.UserPassword);
                var ouser = db.CP_User.Where(u => u.Name == user.UserName || u.Password == password).FirstOrDefault();
                if(ouser != null)
                {
                    if (ouser.Password == user.UserPassword)
                    {
                        ViewBag.ModelErrors = "كلمة المرور موجود مسبقا";
                    }
                    else
                    {
                        ViewBag.ModelErrors = "اسم المستخدم موجود مسبقا";
                    }
                    ViewBag.CoursePrivilege = new SelectList(db.CP_Course, "ID", "Name", user.CoursePrivilege);
                    return View(user);
                }
                CP_User us = new CP_User
                {
                    Name = user.UserName,
                    Email = user.UserEmail,
                    CreationDate = DateTime.Now,
                    CreatorID = Int32.Parse(Session["UserId"].ToString())
                };
                if (!user.IsAdmin)
                {
                    us.CoursePrivilege = user.CoursePrivilege;
                }
                us.Password = password;
                db.CP_User.Add(us);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(user);
        }

        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var user = db.CP_User.Where(u => u.ID == id).AsEnumerable().Select(u => new User
            {
                UserId = u.ID,
                UserName = u.Name,
                UserEmail = u.Email,
                CoursePrivilege = u.CoursePrivilege,
                UserPassword = HashHelper.Decrypt(u.Password),
                CreationDate = u.CreationDate,
                CreatorId = u.CreatorID
            }).FirstOrDefault();
            if(user.CoursePrivilege == null)
            {
                user.IsAdmin = true;
            }
            if (user == null)
            {
                return HttpNotFound();
            }
            ViewBag.CoursePrivilege = new SelectList(db.CP_Course, "ID", "Name",user.CoursePrivilege);
            return View(user);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "UserId,UserName,UserEmail,IsAdmin,CoursePrivilege,UserPassword,CreatorId,CreationDate")] User user)
        {
            if (ModelState.IsValid)
            {
                var ouser = db.CP_User.Find(user.UserId);
                if (ouser == null)
                {
                    ViewBag.ModelErrors = "هذا المستخدم غير موجود";
                    ViewBag.CoursePrivilege = new SelectList(db.CP_Course, "ID", "Name", user.CoursePrivilege);
                    return View(user);
                }
                var password = HashHelper.Encrypt(user.UserPassword);
                if (ouser.Password != user.UserPassword || ouser.Name != user.UserName)
                {
                    var o2user = db.CP_User.Where(u => u.Name == user.UserName || u.Password == password).FirstOrDefault();
                    if (o2user != null && o2user.ID != ouser.ID)
                    {
                        if (o2user.Password == user.UserPassword)
                        {
                            ViewBag.ModelErrors = "كلمة المرور موجود مسبقا";
                        }
                        else
                        {
                            ViewBag.ModelErrors = "اسم المستخدم موجود مسبقا";
                        }
                        ViewBag.CoursePrivilege = new SelectList(db.CP_Course, "ID", "Name", user.CoursePrivilege);
                        return View(user);
                    }
                }
                ouser.Name = user.UserName;
                ouser.Email = user.UserEmail;
                if (!user.IsAdmin)
                {
                    ouser.CoursePrivilege = user.CoursePrivilege;
                }
                else
                {
                    ouser.CoursePrivilege = null;
                }
                ouser.Password = password;
                ouser.CreatorID = user.CreatorId;
                ouser.ModificationDate = DateTime.Now;
                ouser.ModifiedID = Int32.Parse(Session["UserId"].ToString());
                ouser.CreationDate = user.CreationDate;
                db.Entry(ouser).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(user);
        }

        [UserAuthorize(Role.Admin)]
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var user = db.CP_User.Where(u => u.ID == id).Select(u => new User
            {
                UserId = u.ID,
                UserName = u.Name,
                UserEmail = u.Email,
                CourseName = u.CP_Course2.Name,
                CreationDate = u.CreationDate,
                CreatorName = u.CP_User2.Name,
                ModifiedName = u.CP_User3.Name,
                ModficationDate = u.ModificationDate
            }).FirstOrDefault();
            if (user == null)
            {
                return HttpNotFound();
            }
            return View(user);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id)
        {
            CP_User user = db.CP_User.Find(id);
            db.CP_User.Remove(user);
            db.SaveChanges();
            return RedirectToAction("Index");
        }
    }
}