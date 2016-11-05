using Known.Web;
using System.Web.Mvc;

namespace SnsLite.Web
{
    public static class Extensions
    {
        public static string GetAccount(this ViewContext context)
        {
            var account = context.RouteData.Values["account"];
            if (account == null)
                return context.HttpContext.User.Identity.Name;
            return account.ToString();
        }

        public static bool IsCurrentAccount(this ViewContext context)
        {
            var account = context.GetAccount();
            var current = context.HttpContext.User.Identity.Name;
            return account == current;
        }

        public static string ToAjax(this UrlHelper helper, AjaxRequestType type)
        {
            return helper.ToAction("Ajax", "Home", new { id = (int)type });
        }

        public static string ToLogoUrl(this UrlHelper helper, string logo)
        {
            return helper.Content(logo ?? Setting.NoLogo);
        }

        public static string ToHomePublic(this UrlHelper helper)
        {
            return helper.ToAction("Public", "Home");
        }

        public static string ToCompanyIndex(this UrlHelper helper, string account)
        {
            return helper.ToAction("Index", "Company", new { u = account });
        }

        public static string ToCountHtml(this int count)
        {
            if (count > 0)
            {
                return string.Format("({0})", count);
            }

            return string.Empty;
        }
    }
}