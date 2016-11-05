using Known.Core;
using SnsLite.Services;
using System.Collections.Generic;

namespace SnsLite.Web.ViewModels
{
    public class CompanyIndexModel : UserModel
    {
        public CompanyIndexModel(SnsUser user, SnsUser current, int cate) : base(user)
        {
            var activityService = ServiceFactory.GetService<IActivityService>();
            Activities = activityService.GetUserActivities(user.Account, current, (ActivityCategory)cate, 0);
        }

        public List<Activity> Activities { get; private set; }
    }
}