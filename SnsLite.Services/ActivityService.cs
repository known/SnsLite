using Dapper;
using Known;
using Known.Core;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SnsLite.Services
{
    public class ActivityService : ServiceBase, IActivityService
    {
        public List<Activity> GetNewestActivities(string userId, ActivityCategory category, int pageIndex)
        {
            var sql = Helper.GetActivitySql(category);
            var sqlPage = Utils.GetPagingSql(sql, new string[] { "CreateTime desc" }, Setting.ActivitySize, pageIndex);
            return connection.Query<Activity, SnsUser, Activity>(
                sqlPage
              , (activity, user) => { activity.User = user; return activity; }
              , new { UserId = userId }
              , null, false, "CommentCount", null, null
            ).ToList();
        }

        public List<Activity> GetUserActivities(string account, SnsUser current, ActivityCategory category, int pageIndex)
        {
            var usr = LoadService<IUserService>().GetUser(account);
            if (usr == null)
                return new List<Activity>();

            var sql = Helper.GetActivitySql(category);
            if (current != null)
            {
                if (current.IsCurrentFriend)
                    sql += " and (ViewRange=0 or ViewRange=1 or ViewRange=3)";
                else if (current.IsCurrentAttention)
                    sql += " and (ViewRange=0 or ViewRange=2 or ViewRange=3)";
            }
            else
            {
                sql += " and ViewRange=0";
            }

            var sqlPage = Utils.GetPagingSql(sql, new string[] { "CreateTime desc" }, Setting.ActivitySize, pageIndex);
            return connection.Query<Activity, SnsUser, Activity>(
                sqlPage
              , (activity, user) => { activity.User = user; return activity; }
              , new { Account = account, UserId = usr.Id }
              , null, false, "CommentCount", null, null
            ).ToList();
        }

        public Result<Activity> PublishActivity(SnsUser user, ViewRange range, string content)
        {
            if (user == null || string.IsNullOrWhiteSpace(user.Id))
                return Result.Error<Activity>("当前用户不存在，请确认是否登录！");

            if (string.IsNullOrWhiteSpace(content))
                return Result.Error<Activity>("您不可以发布空内容！");

            var sql = "insert into T_Activitys(Id,UserId,Content,CreateTime,ViewRange) values(@Id,@UserId,@Content,@CreateTime,@ViewRange)";
            var createTime = DateTime.Now;
            var param = new
            {
                Id = Utils.NewGuid,
                UserId = user.Id,
                Content = content,
                CreateTime = createTime,
                ViewRange = (int)range
            };
            var activity = new Activity
            {
                Id = param.Id,
                Content = content,
                ViewRange = range,
                CreateTime = createTime,
                User = user,
                ViewCount = 0,
                LikeCount = 0
            };

            return connection.Transaction("发布", (conn, trans) =>
            {
                conn.Execute(sql, param, trans);
                SnsUserHelper.UpdateCount(conn, trans, user.Id);
            }, activity);
        }

        class Helper
        {
            internal static string GetActivitySql(ActivityCategory category)
            {
                var sql = @"select a.Id,a.Content,a.CreateTime,a.ViewCount,a.CommentCount,b.Account,b.Name,c.Avatar 
from T_Activitys a 
left join K_Users b on b.Id=a.UserId 
left join T_SnsUsers c on c.UserId=a.UserId 
where a.IsDelete=0";

                switch (category)
                {
                    case ActivityCategory.All:
                        sql += " and (a.UserId=@UserId";
                        sql += " or exists(select 1 from T_UserFriends where FriendId=a.UserId and UserId=@UserId and Status=1)";
                        sql += " or exists(select 1 from T_UserAttentions where AttentionId=a.UserId and UserId=@UserId)";
                        sql += ")";
                        break;
                    case ActivityCategory.Friend:
                        sql += " and exists(select 1 from T_UserFriends where FriendId=a.UserId and UserId=@UserId and Status=1)";
                        break;
                    case ActivityCategory.Attention:
                        sql += " and exists(select 1 from T_UserAttentions where AttentionId=a.UserId and UserId=@UserId)";
                        break;
                }

                return sql;
            }
        }
    }
}