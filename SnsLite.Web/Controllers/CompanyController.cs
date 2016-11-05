using SnsLite.Web.ViewModels;
using System.Web.Mvc;

namespace SnsLite.Web.Controllers
{
    public class CompanyController : SnsBaseController
    {
        public ActionResult Index(string u, int cate = 0)
        {
            var user = GetSnsUser(u);
            var model = new CompanyIndexModel(user, CurrentUser, cate);
            return View(model);
        }

        public ActionResult Info(string u)
        {
            var user = GetSnsUser(u);
            var model = new CompanyInfoModel(user);
            return View(model);
        }
    }
}