using CoursePlatform.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Configuration;

namespace CoursePlatform.App_Start
{
    public class CourseConfig
    {
        public static void RegisterAllCourses(IDictionary<int, object> courses)
        {
            var courseId = Int32.Parse(WebConfigurationManager.AppSettings["Tarbea"]);
            courses.Add(courseId, new Tarbea());
        }
    }
}