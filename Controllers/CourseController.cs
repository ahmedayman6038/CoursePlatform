using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data.Entity;
using System.Net;
using System.IO;
using CoursePlatform.Helper;
using NReco.PdfGenerator;
using CoursePlatform.Attributes;
using CoursePlatform.Models;
using CoursePlatform.ViewModels;

namespace CKESWORK.Controllers
{
    public class CourseController : Controller
    {
        private CoursePlatformEntities db = new CoursePlatformEntities();

        [UserAuthorize(Role.Admin)]
        public ActionResult Index()
        {
            var courses = db.CP_Course.Include(g => g.Gender).Select(s => new Course
            {
                CourseId = s.ID,
                CourseName = s.Name,
                GenderName = s.CP_Gender.Name,
                DurationsCount = s.CP_Duration.Count
            }).ToList();
            return View(courses);
        }

        [UserAuthorize(Role.Admin)]
        public ActionResult Create()
        {
            ViewBag.GenderAllowed = new SelectList(db.CP_Gender, "ID", "Name");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [UserAuthorize(Role.Admin)]
        public ActionResult Create([Bind(Include = "CourseId,CourseImage,CourseName,GenderAllowed,CourseDetails")] Course course, HttpPostedFileBase Image)
        {
            if (ModelState.IsValid)
            {
                if (Image != null && Image.ContentLength > 0)
                {
                    var allowedExtensions = new[] { ".jpg", ".jpeg" };
                    var extension = Path.GetExtension(Image.FileName);
                    var fileExtension = extension.ToLower();
                    if (allowedExtensions.Contains(fileExtension))
                    {
                        var uniqe = Guid.NewGuid();
                        string path = Path.Combine(Server.MapPath("~/Uploads"), uniqe + extension);
                        Image.SaveAs(path);
                        course.CourseImage = uniqe + extension;
                    }
                    else
                    {
                        ViewBag.ModelErrors = "يجب ان يكون الفايل بامتداد .jpg , .jpeg";
                        ViewBag.GenderAllowed = new SelectList(db.CP_Gender, "ID", "Name", course.GenderAllowed);
                        return View(course);
                    }
                }
                CP_Course cours = new CP_Course
                {
                    Name = course.CourseName,
                    Gender = course.GenderAllowed,
                    Details = course.CourseDetails,
                    Image = course.CourseImage,
                    CreationDate = DateTime.Now,
                    CreatorID = Int32.Parse(Session["UserId"].ToString())
                };
                db.CP_Course.Add(cours);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.GenderAllowed = new SelectList(db.CP_Gender, "ID", "Name", course.GenderAllowed);
            return View(course);
        }

        [UserAuthorize(Role.Admin)]
        public ActionResult Edit(int? id)
        {
            ViewBag.GenderAllowed = new SelectList(db.CP_Gender, "ID", "Name");
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var course = db.CP_Course.Where(c => c.ID == id).Select(s => new Course
            {
                CourseId = s.ID,
                CourseName = s.Name,
                GenderAllowed = s.Gender,
                CourseDetails = s.Details,
                CourseImage = s.Image,
                CreationDate = s.CreationDate,
                CreatorId = s.CreatorID
            }).FirstOrDefault();
            if (course == null)
            {
                return HttpNotFound();
            }
            ViewBag.GenderAllowed = new SelectList(db.CP_Gender, "ID", "Name", course.GenderAllowed);
            return View(course);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [UserAuthorize(Role.Admin)]
        public ActionResult Edit([Bind(Include = "CourseId,CourseImage,CourseName,GenderAllowed,CourseDetails,CreatorId,CreationDate")] Course course, HttpPostedFileBase Image)
        {
            if (ModelState.IsValid)
            {
                if (Image != null && Image.ContentLength > 0)
                {
                    var allowedExtensions = new[] { ".jpg", ".jpeg" };
                    var extension = Path.GetExtension(Image.FileName);
                    var fileExtension = extension.ToLower();
                    if (allowedExtensions.Contains(fileExtension))
                    {
                        var uniqe = Guid.NewGuid();
                        string path = Path.Combine(Server.MapPath("~/Uploads"), uniqe + extension);
                        Image.SaveAs(path);
                        string fullPath = Request.MapPath("~/Uploads/" + course.CourseImage);
                        if (System.IO.File.Exists(fullPath))
                        {
                            System.IO.File.Delete(fullPath);
                        }
                        course.CourseImage = uniqe + extension;
                    }
                    else
                    {
                        ViewBag.ModelErrors = "يجب ان يكون الفايل بامتداد .jpg , .jpeg";
                        ViewBag.GenderAllowed = new SelectList(db.CP_Gender, "ID", "Name", course.GenderAllowed);
                        return View(course);
                    }
                }
                CP_Course cours = new CP_Course
                {
                    ID = course.CourseId,
                    Name = course.CourseName,
                    Gender = course.GenderAllowed,
                    Details = course.CourseDetails,
                    Image = course.CourseImage,
                    CreatorID = course.CreatorId,
                    ModficationDate = DateTime.Now,
                    ModifiedID = Int32.Parse(Session["UserId"].ToString()),
                    CreationDate = course.CreationDate
                };
                db.Entry(cours).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.GenderAllowed = new SelectList(db.CP_Gender, "GenderId", "GenderName", course.GenderAllowed);
            return View(course);
        }

        [UserAuthorize(Role.Admin)]
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var course = db.CP_Course.Where(c => c.ID == id).Select(s => new Course
            {
                CourseId = s.ID,
                CourseName = s.Name,
                GenderName = s.CP_Gender.Name,
                CreationDate = s.CreationDate,
                CreatorName = s.CP_User.Name,
                ModficationDate = s.ModficationDate,
                ModifiedName = s.CP_User1.Name
            }).FirstOrDefault();
            if (course == null)
            {
                return HttpNotFound();
            }
            return View(course);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [UserAuthorize(Role.Admin)]
        public ActionResult Delete(int id)
        {
            var cours = db.CP_Course.Find(id);
            try
            {
                db.CP_Course.Remove(cours);
                db.SaveChanges();
                if (cours.Image != null)
                {
                    string fullPath = Request.MapPath("~/Uploads/" + cours.Image);
                    if (System.IO.File.Exists(fullPath))
                    {
                        System.IO.File.Delete(fullPath);
                    }
                }
            }
            catch (Exception)
            {

            }
            return RedirectToAction("Index");
        }

        [UserAuthorize(Role.Admin, CoursePrivilege = true)]
        public ActionResult Register(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            dynamic cour = SystemHelper.GetCourseObject((int)id);
            if (cour == null)
            {
                return HttpNotFound();
            }
            string viewName = cour.GetType().Name.ToString() + "Registration";
            CourseRegistration registration = cour;
            var durations = SystemHelper.GetActiveCourseDuration((int)id);
            ViewBag.DurationID = new SelectList(durations, "DurationID", "DurationName");
            return View(viewName, registration);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult TarbeaRegistration([Bind(Include = "DurationID,NationalID,StudentName,StudentFacuty,StudentUniversity,LevelName,Section,Address,TelephoneNumber,PhoneNumber")] Tarbea tarbeaRegistration)
        {
            int courseId = SystemHelper.GetCourseObjectId(tarbeaRegistration);
            var course = db.CP_Course.Find(courseId);
            if (course == null)
            {
                return HttpNotFound();
            }
            if (ModelState.IsValid)
            {
                var registered = db.CP_Registration.Where(s => s.NationalID == tarbeaRegistration.NationalID).FirstOrDefault();
                if (registered != null)
                {
                    ViewBag.ModelErrors = "الرقم القومى موجود مسبقا";
                    var dur = SystemHelper.GetActiveCourseDuration(course.ID);
                    ViewBag.DurationID = new SelectList(dur, "DurationID", "DurationName", tarbeaRegistration.DurationID);
                    return View(tarbeaRegistration);
                }
                var duration = db.CP_Duration.Find(tarbeaRegistration.DurationID);
                if (duration.NumberRolled == duration.MaxCapacity - 1)
                {
                    duration.Active = false;
                }
                duration.NumberRolled++;
                duration.SerialFrom++;
                var serial = duration.SerialFrom;
                db.Entry(duration).State = EntityState.Modified;
                CP_Registration registration = new CP_Registration
                {
                    NationalID = tarbeaRegistration.NationalID,
                    StudentName = tarbeaRegistration.StudentName,
                    StudentFaculty = tarbeaRegistration.StudentFacuty,
                    UniversityName = tarbeaRegistration.StudentUniversity,
                    LevelName = tarbeaRegistration.LevelName,
                    StudentSection = tarbeaRegistration.Section,
                    StudentAddress = tarbeaRegistration.Address,
                    PhoneNumber = tarbeaRegistration.PhoneNumber,
                    TelephoneNumber = tarbeaRegistration.TelephoneNumber
                };
                CP_RegisteredCourse registeredCourse = new CP_RegisteredCourse
                {
                    CourseID = course.ID,
                    RegisteredID = registration.ID,
                    DurationID = tarbeaRegistration.DurationID,
                    SerialNumber = serial
                };
                registration.CP_RegisteredCourse.Add(registeredCourse);
                db.CP_Registration.Add(registration);
                db.SaveChanges();
                return RedirectToAction("RegisteredStudents",new { id = courseId, durid = tarbeaRegistration.DurationID });
            }
            var durations = SystemHelper.GetActiveCourseDuration(course.ID);
            ViewBag.DurationID = new SelectList(durations, "DurationID", "DurationName", tarbeaRegistration.DurationID);
            return View(tarbeaRegistration);
        }

        [UserAuthorize(Role.Admin, CoursePrivilege = true)]
        public ActionResult RegisteredStudents(int? id, int? durid)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            IList<StudentRegistration> result = new List<StudentRegistration>();
            if (durid != null)
            {
                result = SystemHelper.GetStudentsRegistrationCourse((int)id, (int)durid);
            }
            var durations = SystemHelper.GetAllCourseDuration((int)id);
            ViewBag.CourseId = id;
            ViewBag.Result = db.CP_Result.ToList();
            var faild = db.CP_Result.Where(r => r.Name == "راسب").FirstOrDefault();
            if(faild != null)
            {
                ViewBag.FaildId = faild.ID;
            }
            ViewBag.durid = new SelectList(durations, "DurationID", "DurationName");
            return View(result);
        }

        [UserAuthorize(Role.Admin, CoursePrivilege = true)]
        public ActionResult RegistrationDetails(int? id,string nid)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var registration = db.CP_Registration.Where(r => r.NationalID == nid).FirstOrDefault();
            if(registration == null)
            {
                return View();
            }
            var student = SystemHelper.GetTarbeaRegistrationData(registration.ID, (int)id);
            ViewBag.CourseId = id;
            ViewBag.RegistrationId = registration.ID;
            var faild = db.CP_Result.Where(r => r.Name == "راسب").FirstOrDefault();
            if (faild != null)
            {
                ViewBag.FaildId = faild.ID;
            }
            return View(student);
        }

        [UserAuthorize(Role.Admin, CoursePrivilege = true)]
        public ActionResult ResultRegistration(int? id, int? durid)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            IList<StudentRegistration> result = new List<StudentRegistration>();
            if (durid != null)
            {
                result = SystemHelper.GetCourseResultRegistration((int)id, (int)durid);
            }
            var durations = SystemHelper.GetAllCourseDuration((int)id);
            ViewBag.CourseId = id;
            ViewBag.Result = db.CP_Result.ToList();
            ViewBag.durid = new SelectList(durations, "DurationID", "DurationName");
            return View(result);
        }

        [HttpPost]
        [UserAuthorize(Role.Admin, CoursePrivilege = true)]
        public ActionResult ResultRegistration(int? id, int? rid, int? result)
        {
            if (id == null || rid == null || result == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var registeredCourse = db.CP_RegisteredCourse.Where(r => r.RegisteredID == rid && r.CourseID == id).FirstOrDefault();
            if (registeredCourse == null)
            {
                return HttpNotFound();
            }
            registeredCourse.ResultID = result;
            CP_Registration registration = db.CP_Registration.Find(rid);
            registration.CP_RegisteredCourse.Add(registeredCourse);
            db.Entry(registration).State = EntityState.Modified;
            db.SaveChanges();
            return new HttpStatusCodeResult(HttpStatusCode.OK);
        }

        [UserAuthorize(Role.Admin, CoursePrivilege = true)]
        public ActionResult GetFacultyNames(int? id, int? durid)
        {
            if (id == null || durid == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var facultyNames = SystemHelper.GetCourseRegistrationFacultyNames((int)id, (int)durid);
            return Json(facultyNames, JsonRequestBehavior.AllowGet);
        }

        [UserAuthorize(Role.Admin, CoursePrivilege = true)]
        public ActionResult PrintResult(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var durations = SystemHelper.GetAllCourseDuration((int)id);
            ViewBag.CourseId = id;
            ViewBag.durid = new SelectList(durations, "DurationID", "DurationName");
            return View();
        }

        [HttpPost]
        [UserAuthorize(Role.Admin, CoursePrivilege = true)]
        public ActionResult PrintResult(int? id, int? durid,string fname)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            if(durid == null || fname == "" || fname == null)
            {
                var durations = SystemHelper.GetAllCourseDuration((int)id);
                ViewBag.CourseId = id;
                ViewBag.durid = new SelectList(durations, "DurationID", "DurationName");
                return View();
            }
            dynamic course = SystemHelper.GetCourseObject((int)id);
            if (course == null)
            {
                return HttpNotFound();
            }
            var objectName = course.GetType().Name.ToString();
            List<StudentRegistration> result = SystemHelper.GetCourseResultRegistrationByFaculty((int)id, (int)durid, fname);
            string viewName = @"..\Print\" + objectName + "ResultRegistration";
            string htmlToConvert = SystemHelper.RenderViewAsString(viewName, result, this.ControllerContext);
            var htmlToPdf = new HtmlToPdfConverter();
            var pdfBytes = htmlToPdf.GeneratePdf(htmlToConvert);
            FileResult fileResult = new FileContentResult(pdfBytes, "application/pdf")
            {
                FileDownloadName = fname + "-" + durid + ".pdf"
            };
            return fileResult;
        }
    }
}