using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using CoursePlatform.Attributes;
using CoursePlatform.Helper;
using NReco.PdfGenerator;
using CoursePlatform.Models;
using System;
using CoursePlatform.ViewModels;

namespace CKESWORK.Controllers
{
    public class StudentController : Controller
    {
        private CoursePlatformEntities db = new CoursePlatformEntities();

        [UserAuthorize(Role.Student)]
        public ActionResult CourseDetails(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var course = db.CP_Course.Find(id);
            if (course == null)
            {
                return HttpNotFound();
            }
            var studentId = Int32.Parse(Session["UserId"].ToString());
            var registered = db.CP_RegisteredCourse.Where(s => s.RegisteredID == studentId && s.CourseID == course.ID).FirstOrDefault();
            if(registered != null)
            {
                ViewBag.Rid = registered.RegisteredID;
                if (registered.ResultID != null)
                {
                    ViewBag.ResultName = registered.CP_Result.Name;
                }
            }
            var durations = SystemHelper.GetActiveCourseDuration((int)id);
            ViewBag.Course = course;
            return View(durations);
        }

        [UserAuthorize(Role.Student)]
        public ActionResult CourseRegistration(int? id,int? durid)
        {
            if(id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            int? studentRegistration = null;
            if(Session["UserId"] != null)
            {
                studentRegistration = (int)Session["UserId"];
            }
            var student = db.CP_Registration.Find(studentRegistration);
            if(student == null)
            {
                return HttpNotFound();
            }
            var course = db.CP_Course.Where(c => c.ID == id).FirstOrDefault();

            dynamic cour = SystemHelper.GetCourseObject(course.ID);
            if (cour == null)
            {
                return HttpNotFound();
            }
            string viewName = cour.GetType().Name.ToString() + "Registration";
            CourseRegistration registration = cour;
            registration.StudentName = student.StudentName;
            registration.StudentFacuty = student.StudentFaculty;
            registration.NationalID = student.NationalID;
            registration.PhoneNumber = student.PhoneNumber;
            registration.LevelName = student.LevelName;
            registration.StudentUniversity = "جامعة القاهرة";
            var registered = db.CP_RegisteredCourse.Where(s => s.RegisteredID == student.ID && s.CourseID == course.ID).FirstOrDefault();
            if(registered != null)
            {
                ViewBag.Message = " لقد قمت بالتسجيل مسبقا برجاء الغاء التسجيل اولا للرجوع  <a href='/Student/CourseDetails/" + course.ID + "'> اضغط هنا </a>";
                return View("Message");
            }
            var durations = SystemHelper.GetActiveCourseDuration(course.ID);
            if(durations.Count == 0)
            {
                ViewBag.Message = "عفوا لايوجد دورات حاليا متاحة للتسجيل";
                return View("Message");
            }
            if (durid == null)
            {
                ViewBag.DurationID = new SelectList(durations, "DurationID", "DurationName");
                return View(viewName, registration);
            }
            ViewBag.DurationID = new SelectList(durations, "DurationID", "DurationName", durid);
            return View(viewName, registration);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [UserAuthorize(Role.Student)]
        public ActionResult TarbeaRegistration([Bind(Include = "DurationID,NationalID,StudentName,StudentFacuty,StudentUniversity,LevelName,Section,Address,TelephoneNumber,PhoneNumber")] Tarbea tarbeaRegistration)
        {
            int courseId = SystemHelper.GetCourseObjectId(tarbeaRegistration);
            var course = db.CP_Course.Find(courseId);
            if(course == null)
            {
                return HttpNotFound();
            }
            if (ModelState.IsValid)
            {
                var studentRegistration = (int)Session["UserId"];
                var student = db.CP_Registration.Find(studentRegistration);
                if (student != null)
                {
                    var duration = db.CP_Duration.Find(tarbeaRegistration.DurationID);
                    if(duration.NumberRolled == duration.MaxCapacity - 1)
                    {
                        duration.Active = false;
                    }
                    duration.NumberRolled++;
                    duration.SerialFrom++;
                    var serial = duration.SerialFrom;
                    db.Entry(duration).State = EntityState.Modified;
                    student.StudentSection = tarbeaRegistration.Section;
                    student.StudentAddress = tarbeaRegistration.Address;
                    student.TelephoneNumber = tarbeaRegistration.TelephoneNumber;
                    student.UniversityName = tarbeaRegistration.StudentUniversity;
                    CP_RegisteredCourse registeredCourse = new CP_RegisteredCourse
                    {
                        CourseID = course.ID,
                        RegisteredID = student.ID,
                        DurationID = tarbeaRegistration.DurationID,
                        SerialNumber = serial
                    };
                    student.CP_RegisteredCourse.Add(registeredCourse);
                    db.Entry(student).State = EntityState.Modified;
                    db.SaveChanges();
                    return RedirectToAction("CourseDetails", new { id = course.ID });
                }
                return HttpNotFound();
            }
            var durations = SystemHelper.GetActiveCourseDuration(course.ID);
            ViewBag.DurationID = new SelectList(durations, "ID", "Name", tarbeaRegistration.DurationID);
            return View(tarbeaRegistration);
        }

        [UserAuthorize(Role.Student)]
        public ActionResult RemoveRegistration(int? id,int? rid)
        {
            if(id == null || rid == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var courseRegistration = db.CP_RegisteredCourse.Where(r => r.RegisteredID == rid && r.CourseID == id).FirstOrDefault();
            var registration = db.CP_Registration.Find(rid);
            if(registration == null)
            {
                return HttpNotFound();
            }
            if(courseRegistration.CP_Result.Name != "راسب")
            {
                ViewBag.Message = "عفوا لقد قمت باجتياز الدورة بنجاح لايمكن الغاء تسجيلك";
                return View("Message");
            }
            var duration = db.CP_Duration.Find(courseRegistration.DurationID);
            duration.NumberRolled--;
            db.Entry(duration).State = EntityState.Modified;
            registration.CP_RegisteredCourse.Remove(courseRegistration);
            db.SaveChanges();
            return RedirectToAction("CourseDetails", new { id = id});
        }

        [UserAuthorize(Role.Admin,Role.Student,Role.Tarbea)]
        public ActionResult DownloadRegistrationForm(int? id,int? rid)
        {
            if(id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            dynamic course = SystemHelper.GetCourseObject((int)id);
            if (course == null)
            {
                return HttpNotFound();
            }
            var objectName = course.GetType().Name.ToString();
            string viewName = @"..\Print\" + objectName + "Registration";
            string fileName = objectName + "Registration.pdf";
            int? registrationId = rid;
            if (rid == null && Session["UserId"] != null)
            {
                registrationId = (int)Session["UserId"];
            }
            dynamic student = null;
            if(objectName == typeof(Tarbea).Name.ToString())
            {
                student = SystemHelper.GetTarbeaRegistrationData((int)registrationId, (int)id);
            }

            /**
             * Add Else If Here For More Course Registration Print
             * 
             * */
          
            if (student == null)
            {
                ViewBag.Message = "برجاء التسجيل اولا للتسجيل <a href='/Student/CourseRegistration/" + id + "'> اضغط هنا </a>";
                return View("Message");
            }
            string htmlToConvert = SystemHelper.RenderViewAsString(viewName, student,this.ControllerContext);
            var htmlToPdf = new HtmlToPdfConverter();
            var pdfBytes = htmlToPdf.GeneratePdf(htmlToConvert);
            FileResult fileResult = new FileContentResult(pdfBytes, "application/pdf")
            {
                FileDownloadName = registrationId + "-" + fileName
            };
            return fileResult;
        }

        [UserAuthorize(Role.Admin, Role.Student, Role.Tarbea)]
        public ActionResult DownloadResultForm(int? id,int? rid)
        {
            if (id == null || rid == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            dynamic course = SystemHelper.GetCourseObject((int)id);
            if (course == null)
            {
                return HttpNotFound();
            }
            var objectName = course.GetType().Name.ToString();
            string viewName = @"..\Print\" + objectName + "StudentResult";
            string fileName = objectName + "Result.pdf";
            int registrationId = (int)rid;
            dynamic result = null;
            if (objectName == typeof(Tarbea).Name.ToString())
            {
                result = SystemHelper.GetCourseStudentResult(registrationId, (int)id);
            }

            /**
             * Add Else If Here For More Course Registration Print
             * 
             * */

            if (result == null)
            {
                ViewBag.Message = "لم يتم ظهور النتيجة بعد";
                return View("Message");
            }
            if(result.ResultName == "راسب")
            {
                ViewBag.Message = "لم يتم اجتياز الدورة بنجاح";
                return View("Message");
            }
            string htmlToConvert = SystemHelper.RenderViewAsString(viewName, result, this.ControllerContext);
            var htmlToPdf = new HtmlToPdfConverter();
            var pdfBytes = htmlToPdf.GeneratePdf(htmlToConvert);
            FileResult fileResult = new FileContentResult(pdfBytes, "application/pdf")
            {
                FileDownloadName = registrationId + "-" + fileName
            };
            return fileResult;
        }

        [AllowAnonymous]
        public ActionResult GetStudent(string code)
        {
            var student = db.CP_Student.Where(s => s.StudentCode == code).Select(p => new
            {
                StudentName = p.StudentName,
                FacultyName = p.StudentFaculty
            }).FirstOrDefault();
            return Json(student, JsonRequestBehavior.AllowGet);
        }
    }
}