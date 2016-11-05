using Known;
using Known.Web;
using SnsLite.Services;
using System.Web.Mvc;

namespace SnsLite.Web.AjaxHandlers
{
    class ProfileAjaxHandler : AjaxHandler
    {
        public ProfileAjaxHandler(BaseController controller, SnsUser current) : base(controller, current) { }

        public ActionResult SaveBasic(string param)
        {
            var current = CurrentUser;
            var user = param.ToObject<SnsUser>();
            current.Name = user.Name;
            current.Email = user.Email;
            current.Mobile = user.Mobile;
            current.Introduction = user.Introduction;
            current.FriendValidate = user.FriendValidate;
            current.AllowComment = user.AllowComment;
            var result = LoadService<ISnsUserService>().UpdateUser(current);
            return JsonResult(result);
        }

        class Password
        {
            public string OldPassword { get; set; }
            public string NewPassword { get; set; }
            public string NewPassword1 { get; set; }
        }

        public ActionResult ChangePassword(string param)
        {
            var pwd = param.ToObject<Password>();
            if (pwd.NewPassword != pwd.NewPassword1)
                return ErrorResult("两次输入的密码不一致！");

            var result = LoadService<ISnsUserService>().UpdatePassword(CurrentUser.Id, pwd.OldPassword, pwd.NewPassword);
            return JsonResult(result);
        }
    }
}