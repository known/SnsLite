using Known.Core;
using System;
using System.Web;
using System.Web.Helpers;
using System.Web.Mvc;
using System.Web.Security;

namespace Known.Web
{
    #region Extensions
    public static class Extensions
    {
        public static IHtmlString RawHtml(this HtmlHelper helper, string text)
        {
            var value = WebUtils.FormatHtml(text);
            return helper.Raw(value);
        }

        public static string ToAction(this UrlHelper helper, string actionName)
        {
            return helper.Action(actionName).ToLower();
        }

        public static string ToAction(this UrlHelper helper, string actionName, string controllerName)
        {
            return helper.Action(actionName, controllerName).ToLower();
        }

        public static string ToAction(this UrlHelper helper, string actionName, object routeValues)
        {
            return helper.Action(actionName, routeValues).ToLower();
        }

        public static string ToAction(this UrlHelper helper, string actionName, string controllerName, object routeValues)
        {
            return helper.Action(actionName, controllerName, routeValues).ToLower();
        }
    }
    #endregion

    #region Filters
    public class VisitLogAttribute : ActionFilterAttribute
    {
        private VisitLog log = new VisitLog();

        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            var httpContext = filterContext.HttpContext;
            log.UserName = httpContext.User.Identity.Name ?? "Anonymous";
            log.RawUrl = httpContext.Request.RawUrl;
            log.IPAddress = httpContext.Request.GetIPAddress();
            log.IPAddressName = Utils.GetIPAddressName(log.IPAddress);
            log.OSName = httpContext.Request.GetOSName();
            log.Browser = httpContext.Request.Browser.Browser;
            log.BrowserVersion = httpContext.Request.Browser.Version;
            log.VisitTime = DateTime.Now;
        }

        public override void OnResultExecuted(ResultExecutedContext filterContext)
        {
            log.FinishTime = DateTime.Now;
            var service = ServiceFactory.GetService<ILogService>();
            service.AddVisitLog(log);
        }
    }

    public class ActionLogAttribute : ActionFilterAttribute
    {
        private string moduleName;
        private string actionName;
        private ActionLog log = new ActionLog();

        public ActionLogAttribute(string moduleName, string actionName)
        {
            this.moduleName = moduleName;
            this.actionName = actionName;
        }

        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            var httpContext = filterContext.HttpContext;
            log.UserName = httpContext.User.Identity.Name ?? "Anonymous";
            log.ModuleName = moduleName;
            log.ActionName = actionName;
            log.VisitTime = DateTime.Now;
        }

        public override void OnResultExecuted(ResultExecutedContext filterContext)
        {
            log.FinishTime = DateTime.Now;
            var service = ServiceFactory.GetService<ILogService>();
            service.AddActionLog(log);
        }
    }

    public class LoginAuthorizeAttribute : AuthorizeAttribute
    {
        public override void OnAuthorization(AuthorizationContext filterContext)
        {
            var httpContext = filterContext.HttpContext;
            if (!httpContext.User.Identity.IsAuthenticated)
            {
                if (httpContext.Request.IsAjaxRequest())
                {
                    filterContext.Result = new JsonResult()
                    {
                        Data = new { timeout = true },
                        JsonRequestBehavior = JsonRequestBehavior.AllowGet
                    };
                }
                else
                {
                    filterContext.Result = new RedirectResult(FormsAuthentication.LoginUrl);
                }
            }
        }
    }

    public class ValidateAntiForgeryTokenOnPostAttribute : AuthorizeAttribute
    {
        public override void OnAuthorization(AuthorizationContext filterContext)
        {
            var request = filterContext.HttpContext.Request;
            if (request.HttpMethod.ToLower() == "post")
            {
                if (request.IsAjaxRequest())
                {
                    var antiForgeryCookie = request.Cookies[AntiForgeryConfig.CookieName];
                    var cookieValue = antiForgeryCookie != null ? antiForgeryCookie.Value : null;
                    AntiForgery.Validate(cookieValue, request.Headers["__RequestVerificationToken"]);
                }
                else
                {
                    new ValidateAntiForgeryTokenAttribute().OnAuthorization(filterContext);
                }
            }
        }
    }
    #endregion

    public class BaseController : Controller
    {
        public ActionResult Captcha()
        {
            var code = string.Empty;
            var bytes = Utils.CreateCaptcha(4, out code);
            Session["CaptchaCode"] = code;
            return File(bytes, "image/jpeg");
        }

        public T LoadService<T>()
        {
            return ServiceFactory.GetService<T>();
        }

        public string CaptchaCode
        {
            get { return Session["CaptchaCode"] != null ? Session["CaptchaCode"].ToString() : string.Empty; }
        }

        protected string UserName
        {
            get { return User.Identity.Name; }
        }

        protected bool IsAuthenticated
        {
            get { return User.Identity.IsAuthenticated; }
        }

        public ActionResult ErrorResult(string message)
        {
            return JsonResult(Result.Error(message));
        }

        public ActionResult ErrorResult<T>(string message, T data)
        {
            return JsonResult(Result.Error(message, data));
        }

        public ActionResult SuccessResult(string message)
        {
            return JsonResult(Result.Success(message));
        }

        public ActionResult SuccessResult<T>(string message, T data)
        {
            return JsonResult(Result.Success(message, data));
        }

        public ActionResult JsonResult(object data)
        {
            return Json(data, JsonRequestBehavior.AllowGet);
        }

        public ActionResult PartialViewResult(string viewName, object model)
        {
            return PartialView(viewName, model);
        }
    }
}