using Known.Core;
using Known.Web;
using SnsLite.Services;

namespace SnsLite.Web.Controllers
{
    public class SnsBaseController : BaseController
    {
        protected SnsUser CurrentUser
        {
            get
            {
                var user = Session["CurrentUser"] as SnsUser;
                if (user == null)
                {
                    user = ServiceFactory.GetService<ISnsUserService>().GetSnsUser(UserName);
                    if (user != null)
                    {
                        user.IsCurrent = true;
                        Session["CurrentUser"] = user;
                    }
                }

                return user;
            }
        }

        protected SnsUser GetSnsUser(string account = "")
        {
            var userService = LoadService<ISnsUserService>();

            SnsUser user = null;
            if (!string.IsNullOrEmpty(account))
            {
                user = userService.GetSnsUser(account);
                user.IsCurrent = CurrentUser != null && CurrentUser.Account == account;
            }
            else if (CurrentUser != null)
            {
                user = CurrentUser;
            }
            else
            {
                user = new SnsUser();
            }

            user.IsLogin = IsAuthenticated;

            if (!string.IsNullOrEmpty(user.Id))
            {
                user.UpdateCount(userService);
            }

            if (CurrentUser != null && !string.IsNullOrWhiteSpace(user.Id))
            {
                user.IsCurrentFriend = userService.CheckFriend(CurrentUser.Id, user.Id, true);
                user.IsCurrentValidate = userService.CheckFriend(CurrentUser.Id, user.Id, false);
                user.IsCurrentAttention = userService.CheckAttention(CurrentUser.Id, user.Id);
                var mutual = userService.CheckAttention(user.Id, CurrentUser.Id);
                user.IsMutualAttention = user.IsCurrentAttention && mutual;
            }

            return user;
        }
    }
}