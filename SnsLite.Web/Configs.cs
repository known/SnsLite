using Known.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace SnsLite.Web
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute("Home", "", new { controller = "Home", action = "Index" });
            routes.MapRoute("Ajax", "ajax", new { controller = "Home", action = "Ajax" });
            routes.MapRoute("Index", "index", new { controller = "Home", action = "Public" });
            routes.MapRoute("FindUser", "find", new { controller = "Home", action = "FindUser" });
            routes.MapRoute("Register", "register", new { controller = "Home", action = "Register" });
            routes.MapRoute("Login", "login", new { controller = "Home", action = "Login" });
            routes.MapRoute("Logout", "logout", new { controller = "Home", action = "Logout" });
            routes.MapRoute("ChangePassword", "chgpwd", new { controller = "Home", action = "ChangePassword" });
            routes.MapRoute("Profile", "profile", new { controller = "Home", action = "Profile" });

            routes.MapRoute("Default", "{controller}/{action}/{id}", new { controller = "Home", action = "Index", id = UrlParameter.Optional });
        }
    }

    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new ValidateInputAttribute(false));
            filters.Add(new VisitLogAttribute());
            //filters.Add(new ValidateAntiForgeryTokenOnPostAttribute());
        }
    }
}