using Known.Core;
using SnsLite.Services;
using System.Collections.Generic;

namespace SnsLite.Web.ViewModels
{
    public class PublicModel : UserModel
    {
        public PublicModel(SnsUser user, int cate) : base(user)
        {
            var activityService = ServiceFactory.GetService<IActivityService>();
            ViewRanges = CodeTable.GetCodes(typeof(ViewRange));
            Activities = activityService.GetNewestActivities(user.Id, (ActivityCategory)cate, 0);
        }

        public List<CodeTable> ViewRanges { get; private set; }
        public List<Activity> Activities { get; private set; }
    }
}