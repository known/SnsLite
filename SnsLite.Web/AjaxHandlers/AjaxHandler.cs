using Known.Web;
using System.Web.Mvc;

namespace SnsLite.Web.AjaxHandlers
{
    class AjaxHandler
    {
        private BaseController controller;

        public AjaxHandler(BaseController controller, SnsUser current)
        {
            this.controller = controller;
            CurrentUser = current;
        }

        public SnsUser CurrentUser { get; private set; }

        public T LoadService<T>()
        {
            return controller.LoadService<T>();
        }

        public ActionResult ErrorResult(string message)
        {
            return controller.ErrorResult(message);
        }

        public ActionResult ErrorResult<T>(string message, T data)
        {
            return controller.ErrorResult<T>(message, data);
        }

        public ActionResult SuccessResult(string message)
        {
            return controller.SuccessResult(message);
        }

        public ActionResult SuccessResult<T>(string message, T data)
        {
            return controller.SuccessResult<T>(message, data);
        }

        public ActionResult JsonResult(object data)
        {
            return controller.JsonResult(data);
        }

        public ActionResult PartialViewResult(string viewName, object model)
        {
            return controller.PartialViewResult(viewName, model);
        }
    }
}