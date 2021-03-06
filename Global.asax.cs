﻿using CoursePlatform.App_Start;
using CoursePlatform.Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

namespace CoursePlatform
{
    public class MvcApplication : System.Web.HttpApplication
    {
        public static IDictionary<int, object> courses = new Dictionary<int, object>();

        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
            CourseConfig.RegisterAllCourses(courses);
            SystemHelper.CreateAdminUser();
            SystemHelper.CreateDefaultResult();
            SystemHelper.CreateDefaultGender();
        }
    }
}
