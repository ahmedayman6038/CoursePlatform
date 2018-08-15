using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using CoursePlatform.Helper;
using CoursePlatform.Models;


namespace CoursePlatform.Attributes
{
    [AttributeUsage(AttributeTargets.Method | AttributeTargets.Class, Inherited = true, AllowMultiple = true)]
    public class UserAuthorizeAttribute : AuthorizeAttribute
    {
        private CoursePlatformEntities db;

        public bool CoursePrivilege { get; set; } = false;

        public UserAuthorizeAttribute(params object[] roles)
        {
            if (roles.Any(r => r.GetType().BaseType != typeof(Enum)))
                throw new ArgumentException("roles");

            this.Roles = string.Join(",", roles.Select(r => Enum.GetName(r.GetType(), r)));
            db = new CoursePlatformEntities();
        }

        public UserAuthorizeAttribute(bool CoursePrivilege, params object[] roles)
        {
            if (roles.Any(r => r.GetType().BaseType != typeof(Enum)))
                throw new ArgumentException("roles");

            this.Roles = string.Join(",", roles.Select(r => Enum.GetName(r.GetType(), r)));
            this.CoursePrivilege = CoursePrivilege;
            db = new CoursePlatformEntities();
        }

        protected override bool AuthorizeCore(HttpContextBase httpContext)
        {
            var authorize = false;
            if (httpContext.Session["UserId"] != null)
            {
                var type = httpContext.Session["UserType"].ToString();
                if (CoursePrivilege)
                {
                    int userId = (int)httpContext.Session["UserId"];
                    int courseId = Int32.Parse(httpContext.Request.Url.Segments[3]);
                    var user = db.CP_User.Find(userId);
                    if(user.CoursePrivilege == courseId)
                    {
                        authorize = true;
                    }
                }
                if (this.Roles.Contains(type))
                {
                    authorize = true;
                }
            }
            return authorize;
        }

        protected override void HandleUnauthorizedRequest(AuthorizationContext filterContext)
        {
            var returnUrl = filterContext.HttpContext.Request.Url.AbsolutePath;
            if (this.Roles.Contains(Enum.GetName(typeof(Role), 1)))
            {
                filterContext.Result = new RedirectToRouteResult(
                    new RouteValueDictionary {
                        { "action", "Login" },
                        { "controller", "Account" },
                        { "returnUrl", returnUrl }
                    }
                );
            }
            else
            {
                filterContext.Result = new RedirectToRouteResult(
                   new RouteValueDictionary {
                        { "action", "Login" },
                        { "controller", "Dashboard" },
                        { "returnUrl", returnUrl }
                   }
               );
            }          
        }
    }
}