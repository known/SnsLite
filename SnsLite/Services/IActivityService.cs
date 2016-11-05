using Known;
using System.Collections.Generic;

namespace SnsLite.Services
{
    public enum ActivityCategory
    {
        All = 0,
        Friend = 1,
        Attention = 2
    }

    public interface IActivityService
    {
        List<Activity> GetNewestActivities(string userId, ActivityCategory category, int pageIndex);
        List<Activity> GetUserActivities(string account, SnsUser current, ActivityCategory category, int pageIndex);
        Result<Activity> PublishActivity(SnsUser user, ViewRange range, string content);
    }
}