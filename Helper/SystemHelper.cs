using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web.Configuration;
using System.Web.Mvc;
using CoursePlatform.Models;
using CoursePlatform.ViewModels;

namespace CoursePlatform.Helper
{
    public static class SystemHelper
    {
        private static CoursePlatformEntities db = new CoursePlatformEntities();

        public static string ToArabicNumber(string input)
        {
            return input = input.Replace('0', '٠')
                     .Replace('1', '١')
                     .Replace('2', '۲')
                     .Replace('3', '۳')
                     .Replace('4', '٤')
                     .Replace('5', '٥')
                     .Replace('6', '٦')
                     .Replace('7', '٧')
                     .Replace('8', '٨')
                     .Replace('9', '٩');
        }

        public static string RenderViewAsString(string viewName, object model, ControllerContext context)
        {
            StringWriter stringWriter = new StringWriter();
            ViewEngineResult viewResult = ViewEngines.Engines.FindView(context, viewName, null);
            ViewContext viewContext = new ViewContext(
                    context,
                    viewResult.View,
                    new ViewDataDictionary(model),
                    new TempDataDictionary(),
                    stringWriter
                    );
            viewResult.View.Render(viewContext, stringWriter);
            return stringWriter.ToString();
        }


        public static void CreateAdminUser()
        {
            CP_User user = new CP_User
            {
                Name = WebConfigurationManager.AppSettings["UserName"],
                Email = WebConfigurationManager.AppSettings["UserEmail"],
                Password = HashHelper.Encrypt(WebConfigurationManager.AppSettings["UserPassword"]),
                CreationDate = DateTime.Now
            };
            var us = db.CP_User.Where(u => u.Name == user.Name || u.Password == user.Password).FirstOrDefault();
            if(us == null)
            {
                db.CP_User.Add(user);
                db.SaveChanges();
            }
        }

        public static void CreateDefaultResult()
        {
            var resultsNumber = Int32.Parse(WebConfigurationManager.AppSettings["Results"]);
            for (int i = 1; i <= resultsNumber; i++)
            {
                var resultName = WebConfigurationManager.AppSettings["Result"+i].ToString();
                var re = db.CP_Result.Where(r => r.Name == resultName).FirstOrDefault();
                if(re == null)
                {
                    CP_Result result = new CP_Result
                    {
                        Name = resultName
                    };
                    db.CP_Result.Add(result);
                    db.SaveChanges();
                }
            }
        }

        public static void CreateDefaultGender()
        {
            var typesNumber = Int32.Parse(WebConfigurationManager.AppSettings["Types"]);
            for (int i = 1; i <= typesNumber; i++)
            {
                var typeName = WebConfigurationManager.AppSettings["Type" + i].ToString();
                var ty = db.CP_Gender.Where(t => t.Name == typeName).FirstOrDefault();
                if(ty == null)
                {
                    CP_Gender gender = new CP_Gender
                    {
                        Name = typeName
                    };
                    db.CP_Gender.Add(gender);
                    db.SaveChanges();
                }
            }
        }

        public static dynamic GetCourseObject(int CourseId)
        {
            dynamic course;
            if (MvcApplication.courses.TryGetValue(CourseId, out course))
            {
                return course;
            }
            return null;
        }

        public static int GetCourseObjectId(object obj)
        {
            var objectName = obj.GetType().FullName;
            int objectId = MvcApplication.courses.FirstOrDefault(x => x.Value.ToString() == objectName).Key;
            return objectId;
        }

        public static List<DurationView> GetActiveCourseDuration(int CourseId)
        {
            return db.CP_Duration.Where(d => d.CourseID == CourseId && d.Active == true).OrderBy(s => s.FromDate).AsEnumerable().Select(p => new DurationView
            {
                DurationID = p.ID,
                DurationName = "من " + ToArabicNumber(p.FromDate.ToString("yyyy/MM/dd")) + " الى " + ToArabicNumber(p.ToDate.ToString("yyyy/MM/dd"))
            }).ToList();
        }

        public static List<DurationView> GetAllCourseDuration(int CourseId)
        {
            return db.CP_Duration.Where(d => d.CourseID == CourseId).OrderBy(s => s.FromDate).AsEnumerable().Select(p => new DurationView
            {
                DurationID = p.ID,
                DurationName = "من " + ToArabicNumber(p.FromDate.ToString("yyyy/MM/dd")) + " الى " + ToArabicNumber(p.ToDate.ToString("yyyy/MM/dd"))
            }).ToList();
        }

        public static Tarbea GetTarbeaRegistrationData(int RegistrationId, int CourseId)
        {
            return (from rc in db.CP_RegisteredCourse
                    join sr in db.CP_Registration on rc.RegisteredID equals sr.ID
                    join co in db.CP_Course on rc.CourseID equals co.ID
                    join du in db.CP_Duration on rc.DurationID equals du.ID
                    where rc.RegisteredID == RegistrationId && co.ID == CourseId
                    select new Tarbea
                    {
                        StudentName = sr.StudentName,
                        StudentFacuty = sr.StudentFaculty,
                        StudentUniversity = sr.UniversityName,
                        NationalID = sr.NationalID,
                        Section = sr.StudentSection,
                        TelephoneNumber = sr.TelephoneNumber,
                        PhoneNumber = sr.PhoneNumber,
                        Address = sr.StudentAddress,
                        LevelName = sr.LevelName,
                        DurationFrom = du.FromDate,
                        DurationTo = du.ToDate,
                        SerialNumber = rc.SerialNumber,
                        ResultId = rc.ResultID
                    }).FirstOrDefault();
        }

        public static List<StudentRegistration> GetCourseResultRegistration(int CourseId, int DurationId)
        {
            return (from rc in db.CP_RegisteredCourse
                    join sr in db.CP_Registration on rc.RegisteredID equals sr.ID
                    join co in db.CP_Course on rc.CourseID equals co.ID
                    join du in db.CP_Duration on rc.DurationID equals du.ID
                    where co.ID == CourseId && du.ID == DurationId
                    orderby rc.ResultID
                    select new StudentRegistration
                    {
                        RegisterId = rc.RegisteredID,
                        StudentName = sr.StudentName,
                        NationalId = sr.NationalID,
                        DurationSerial = rc.SerialNumber,
                        Result = rc.ResultID
                    }).ToList();
        }

        public static List<StudentRegistration> GetStudentsRegistrationCourse(int CourseId, int DurationId)
        {
            return (from rc in db.CP_RegisteredCourse
                    join sr in db.CP_Registration on rc.RegisteredID equals sr.ID
                    join co in db.CP_Course on rc.CourseID equals co.ID
                    join du in db.CP_Duration on rc.DurationID equals du.ID
                    where co.ID == CourseId && du.ID == DurationId
                    select new StudentRegistration
                    {
                        RegisterId = rc.RegisteredID,
                        StudentName = sr.StudentName,
                        NationalId = sr.NationalID,
                        UniversityName = sr.UniversityName,
                        FacultyName = sr.StudentFaculty,
                        Result = rc.ResultID
                    }).ToList();
        }

        public static List<StudentRegistration> GetCourseResultRegistrationByFaculty(int CourseId, int DurationId, string FacultyName)
        {
            return (from rc in db.CP_RegisteredCourse
                    join sr in db.CP_Registration on rc.RegisteredID equals sr.ID
                    join co in db.CP_Course on rc.CourseID equals co.ID
                    join du in db.CP_Duration on rc.DurationID equals du.ID
                    join re in db.CP_Result on rc.ResultID equals re.ID
                    where co.ID == CourseId && du.ID == DurationId && sr.StudentFaculty == FacultyName && sr.StudentCode != null
                    orderby rc.ResultID
                    select new StudentRegistration
                    {
                        RegisterId = rc.RegisteredID,
                        StudentName = sr.StudentName,
                        NationalId = sr.NationalID,
                        DurationSerial = rc.SerialNumber,
                        ResultName = re.Name,
                        DurationFrom = du.FromDate,
                        DurationTo = du.ToDate,
                        FacultyName = sr.StudentFaculty
                    }).ToList();
        }

        public static StudentResult GetCourseStudentResult(int RegistrationId, int CourseId)
        {
            return (from rc in db.CP_RegisteredCourse
                    join sr in db.CP_Registration on rc.RegisteredID equals sr.ID
                    join co in db.CP_Course on rc.CourseID equals co.ID
                    join du in db.CP_Duration on rc.DurationID equals du.ID
                    join re in db.CP_Result on rc.ResultID equals re.ID
                    where rc.RegisteredID == RegistrationId && co.ID == CourseId && rc.ResultID != null
                    select new StudentResult
                    {
                        DurationFrom = du.FromDate,
                        DurationTo = du.ToDate,
                        StudentName = sr.StudentName,
                        StudentFaculty = sr.StudentFaculty,
                        StudentLevel = sr.LevelName,
                        ResultName = re.Name
                    }).FirstOrDefault();
        }

        public static List<string> GetCourseRegistrationFacultyNames(int CourseId, int DurationId)
        {
            return (from rc in db.CP_RegisteredCourse
                    join sr in db.CP_Registration on rc.RegisteredID equals sr.ID
                    join co in db.CP_Course on rc.CourseID equals co.ID
                    join du in db.CP_Duration on rc.DurationID equals du.ID
                    join re in db.CP_Result on rc.ResultID equals re.ID
                    where co.ID == CourseId && du.ID == DurationId && sr.StudentCode != null
                    select sr.StudentFaculty).Distinct().ToList();
        }
    }
}