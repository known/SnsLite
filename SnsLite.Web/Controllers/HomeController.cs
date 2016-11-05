using Known.Core;
using Known.Web;
using SnsLite.Services;
using SnsLite.Web.ViewModels;
using System;
using System.Web.Mvc;
using System.Web.Security;

namespace SnsLite.Web.Controllers
{
    public class HomeController : SnsBaseController
    {
        public ActionResult Index()
        {
            return View();
        }

        [LoginAuthorize]
        public ActionResult Ajax(int id, string param)
        {
            var requestType = (AjaxRequestType)id;
            var name = Enum.GetName(typeof(AjaxRequestType), requestType).Split('_');
            var handlerType = Type.GetType(string.Format("SnsLite.Web.AjaxHandlers.{0}AjaxHandler", name[0]));
            var handler = Activator.CreateInstance(handlerType, this, CurrentUser);
            var method = handlerType.GetMethod(name[1]);
            return (ActionResult)method.Invoke(handler, new object[] { param });
        }

        [LoginAuthorize]
        public ActionResult Public(string u, int cate = 0)
        {
            var user = GetSnsUser(u);
            var model = new PublicModel(user, cate);
            return View(model);
        }

        public ActionResult FindUser(string r, string t, string q, int p = 1)
        {
            var model = new FindUserModel(r, t, q, p);
            return View(model);
        }

        public ActionResult Register(string account, string password, string captcha)
        {
            if (captcha != CaptchaCode)
                return ErrorResult("验证码不正确！");

            account = account.ToLower();
            var result = LoadService<IUserService>().Register(account, password);
            if (!result.IsValid)
                return ErrorResult(result.Message);

            FormsAuthentication.SetAuthCookie(result.Data.Account, true);

            var returnUrl = Url.ToHomePublic();
            return SuccessResult("注册成功，正在跳转页面......", returnUrl);
        }

        public ActionResult Login(string account, string password, bool rememberMe, string returnUrl)
        {
            account = account.ToLower();
            var result = LoadService<IUserService>().Login(account, password);
            if (!result.IsValid)
                return ErrorResult(result.Message);

            FormsAuthentication.SetAuthCookie(result.Data.Account, rememberMe);

            if (string.IsNullOrEmpty(returnUrl))
                returnUrl = Url.ToHomePublic();

            return SuccessResult("登录成功，正在跳转页面......", returnUrl);
        }

        [LoginAuthorize]
        public void Logout()
        {
            FormsAuthentication.SignOut();
            Session.Clear();
        }

        [HttpPost]
        [LoginAuthorize]
        public ActionResult ChangeAvatar()
        {
            var file = Request.Files["avatar_file"];
            if (file.ContentLength == 0)
            {
                return JsonResult(new { message = "文件内容不能为空！" });
            }

            var avatar = string.Format("~/static/img/avatars/{0}.jpg", CurrentUser.Id);
            var fileName = Server.MapPath(avatar);
            var fileInfo = new System.IO.FileInfo(fileName);
            if (!fileInfo.Directory.Exists)
            {
                fileInfo.Directory.Create();
            }
            file.SaveAs(fileName);

            CurrentUser.Avatar = avatar + "?t=" + DateTime.Now.Ticks;
            var result = LoadService<ISnsUserService>().UpdateAvatar(CurrentUser.Id, avatar);
            if (!result.IsValid)
            {
                return JsonResult(new { message = result.Message });
            }
            return JsonResult(new { result = Url.Content(CurrentUser.Avatar) });
        }

        [LoginAuthorize]
        public new ActionResult Profile(int t)
        {
            var tab = (ProfileType)t;
            var user = GetSnsUser();
            var tabs = CodeTable.GetCodes(typeof(ProfileType));
            var partialView = string.Format("Profiles/{0}", tab);
            var model = new TabPageModel(user, "账户设置", partialView, tabs);
            switch (tab)
            {
                case ProfileType.Basic:
                    model.Data = CurrentUser;
                    break;
            }
            return View("_TabPage", model);
        }

        [LoginAuthorize]
        public ActionResult Content(int t)
        {
            var tab = (ContentType)t;
            var user = GetSnsUser();
            var partialView = string.Format("Contents/{0}", tab);
            var tabs = CodeTable.GetCodes(typeof(ContentType));
            var model = new TabPageModel(user, "内容管理", partialView, tabs);
            switch (tab)
            {
                case ContentType.Company:
                    model.Data = LoadService<ICompanyService>().GetCompany(CurrentUser.Id);
                    break;
            }
            return View("_TabPage", model);
        }
    }
}