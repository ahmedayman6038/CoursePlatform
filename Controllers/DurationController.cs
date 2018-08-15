using CoursePlatform.Attributes;
using System;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using CoursePlatform.Models;
using CoursePlatform.ViewModels;

namespace CKESWORK.Controllers
{
    [UserAuthorize(Role.Admin,Role.Tarbea)]
    public class DurationController : Controller
    {
        private CoursePlatformEntities db = new CoursePlatformEntities();

        [UserAuthorize(Role.Admin, CoursePrivilege = true)]
        public ActionResult Index(int? id)
        {
            if(id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var durations = db.CP_Duration.Where(d => d.CourseID == id).Select(du => new Duration
            {
                DurationId = du.ID,
                FromDate = du.FromDate,
                ToDate = du.ToDate,
                MinCapacity = du.MinCapacity,
                MaxCapacity = du.MaxCapacity,
                NumberRolled = du.NumberRolled,
                Activate = du.Active
            }).ToList();
            ViewBag.CourseId = id;
            return View(durations);
        }

        [UserAuthorize(Role.Admin, CoursePrivilege = true)]
        public ActionResult Create(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Duration duration = new Duration
            {
                CourseId = (int)id,
                FromDate = DateTime.Now,
                ToDate = DateTime.Now,
                Activate = true
            };
            return View(duration);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "FromDate,ToDate,CourseId,Activate,MinCapacity,MaxCapacity,SerialFrom")] Duration duration)
        {
            if (ModelState.IsValid)
            {
                if(DateTime.Compare(duration.FromDate,duration.ToDate) > 0)
                {
                    ViewBag.ModelErrors = "يجب ان يكون الفترة من اصغر من الفترة الى";
                    return View(duration);
                }
                if (duration.MinCapacity > duration.MaxCapacity)
                {
                    ViewBag.ModelErrors = "يجب ان يكون الحد الادنى اصغر من الحد الاقصى";
                    return View(duration);
                }
                CP_Duration dur = new CP_Duration
                {
                    CourseID = duration.CourseId,
                    FromDate = duration.FromDate,
                    ToDate = duration.ToDate,
                    Active = duration.Activate,
                    MinCapacity = duration.MinCapacity,
                    MaxCapacity = duration.MaxCapacity,
                    SerialFrom = duration.SerialFrom,
                    CreationDate = DateTime.Now,
                    CreatorID = Int32.Parse(Session["UserId"].ToString()),
                    NumberRolled = 0
                };
                if (dur.Active == false)
                {
                    dur.TerminatorID = Int32.Parse(Session["UserId"].ToString());
                }
                db.CP_Duration.Add(dur);
                db.SaveChanges();
                return RedirectToAction("Index", new { id = duration.CourseId });
            }
            return View(duration);
        }

        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var duration = db.CP_Duration.Where(d => d.ID == id).Select(s => new Duration
            {
                DurationId = s.ID,
                CourseId = s.CourseID,
                FromDate = s.FromDate,
                ToDate = s.ToDate,
                MinCapacity = s.MinCapacity,
                MaxCapacity = s.MaxCapacity,
                NumberRolled = s.NumberRolled,
                Activate = s.Active,
                CreatorId = s.CreatorID,
                CreationDate = s.CreationDate
            }).FirstOrDefault();
            if (duration == null)
            {
                return HttpNotFound();
            }
            return View(duration);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "DurationId,FromDate,ToDate,CourseId,Activate,MinCapacity,MaxCapacity,NumberRolled,SerialFrom,CreatorID,CreationDate")] Duration duration)
        {
            if (ModelState.IsValid)
            {
                CP_Duration dur = new CP_Duration
                {
                    ID = duration.DurationId,
                    FromDate = duration.FromDate,
                    ToDate = duration.ToDate,
                    Active = duration.Activate,
                    MinCapacity = duration.MinCapacity,
                    MaxCapacity = duration.MaxCapacity,
                    SerialFrom = duration.SerialFrom,
                    NumberRolled = duration.NumberRolled,
                    CourseID = duration.CourseId,
                    CreatorID = duration.CreatorId,
                    ModificationDate = DateTime.Now,
                    ModifiedID = Int32.Parse(Session["UserId"].ToString()),
                    CreationDate = duration.CreationDate
                };
                if(dur.Active == false)
                {
                    dur.TerminatorID = Int32.Parse(Session["UserId"].ToString());
                }
                db.Entry(dur).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index", new { id = duration.CourseId });
            }
            return View(duration);
        }

        [UserAuthorize(Role.Admin)]
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var user = db.CP_Duration.Where(d => d.ID == id).Select(s => new Duration
            {
                DurationId = s.ID,
                CourseName = s.CP_Course.Name,
                FromDate = s.FromDate,
                ToDate = s.ToDate,
                MinCapacity = s.MinCapacity,
                MaxCapacity = s.MaxCapacity,
                NumberRolled = s.NumberRolled,
                CreationDate = s.CreationDate,
                CreatorName = s.CP_User.Name,
                TerminatorName = s.CP_User1.Name,
                ModifiedName = s.CP_User2.Name,
                ModficationDate = s.ModificationDate
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
            var duration = db.CP_Duration.Find(id);
            var CourseId = duration.CourseID;
            try
            {
                db.CP_Duration.Remove(duration);
                db.SaveChanges();
            }
            catch (Exception)
            {

            }
            return RedirectToAction("Index", new { id = CourseId });
        }
    }
}