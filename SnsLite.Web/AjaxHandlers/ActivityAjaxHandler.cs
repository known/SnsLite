using Known.Web;
using SnsLite.Services;
using System.Web.Mvc;

namespace SnsLite.Web.AjaxHandlers
{
    class ActivityAjaxHandler : AjaxHandler
    {
        public ActivityAjaxHandler(BaseController controller, SnsUser current) : base(controller, current) { }

        public ActionResult AddActivity(string param)
        {
            var message = string.Empty;
            string[] arr = null;

            if (!ParamCheck.CheckLogin(CurrentUser, out message) ||
                !ParamCheck.GetParamArray(param, 2, out message, out arr))
                return ErrorResult(message);

            var range = (ViewRange)int.Parse(arr[0]);
            var result = LoadService<IActivityService>().PublishActivity(CurrentUser, range, arr[1]);
            return JsonResult(result);
        }

        public ActionResult RemoveActivity(string param)
        {
            return ErrorResult("功能未实现！");
        }

        public ActionResult AddLike(string param)
        {
            return ErrorResult("功能未实现！");
        }

        public ActionResult RemoveLike(string param)
        {
            return ErrorResult("功能未实现！");
        }
    }
}