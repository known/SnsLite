using Known.Web;
using SnsLite.Services;
using System.Web.Mvc;

namespace SnsLite.Web.AjaxHandlers
{
    class RelationAjaxHandler : AjaxHandler
    {
        public RelationAjaxHandler(BaseController controller, SnsUser current) : base(controller, current) { }

        public ActionResult AddFriend(string param)
        {
            var message = string.Empty;
            string[] arr = null;

            if (!ParamCheck.CheckLogin(CurrentUser, out message) ||
                !ParamCheck.GetParamArray(param, 1, out message, out arr))
                return ErrorResult(message);

            var friend = LoadService<ISnsUserService>().GetSnsUserById(arr[0]);
            if (friend == null || string.IsNullOrWhiteSpace(friend.Id))
                return ErrorResult("好友用户不存在！");

            if (friend.FriendValidate)
            {
                friend.ValidateMessage = string.Format("我是{0}", CurrentUser.Name);
                return PartialViewResult("_ValidateFriend", friend);
            }

            var result = LoadService<ISnsUserService>().CreateFriend(CurrentUser.Id, arr[0], false, string.Empty);
            return JsonResult(result);
        }

        public ActionResult ValidateFriend(string param)
        {
            var message = string.Empty;
            string[] arr = null;

            if (!ParamCheck.CheckLogin(CurrentUser, out message) ||
                !ParamCheck.GetParamArray(param, 2, out message, out arr))
                return ErrorResult(message);

            var result = LoadService<ISnsUserService>().CreateFriend(CurrentUser.Id, arr[0], true, arr[1]);
            return JsonResult(result);
        }

        public ActionResult AddAttention(string param)
        {
            var message = string.Empty;
            string[] arr = null;

            if (!ParamCheck.CheckLogin(CurrentUser, out message) ||
                !ParamCheck.GetParamArray(param, 1, out message, out arr))
                return ErrorResult(message);

            var result = LoadService<ISnsUserService>().CreateAttention(CurrentUser.Id, arr[0]);
            return JsonResult(result);
        }

        public ActionResult RemoveFriend(string param)
        {
            var message = string.Empty;
            string[] arr = null;

            if (!ParamCheck.CheckLogin(CurrentUser, out message) ||
                !ParamCheck.GetParamArray(param, 1, out message, out arr))
                return ErrorResult(message);

            var result = LoadService<ISnsUserService>().RemoveFriend(CurrentUser.Id, arr[0]);
            return JsonResult(result);
        }

        public ActionResult RemoveAttention(string param)
        {
            var message = string.Empty;
            string[] arr = null;

            if (!ParamCheck.CheckLogin(CurrentUser, out message) ||
                !ParamCheck.GetParamArray(param, 1, out message, out arr))
                return ErrorResult(message);

            var result = LoadService<ISnsUserService>().RemoveAttention(CurrentUser.Id, arr[0]);
            return JsonResult(result);
        }
    }
}