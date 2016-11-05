using Dapper;
using System.Data;

namespace SnsLite.Services
{
    enum CountType
    {
        Share,
        Friend,
        Attention,
        Fans
    }

    class SnsUserHelper
    {
        internal static void UpdateCount(IDbConnection conn, SnsUser user)
        {
            UpdateCount(conn, null, user);
        }

        internal static void UpdateCount(IDbConnection conn, IDbTransaction trans, string userId)
        {
            UpdateCount(conn, trans, new SnsUser { Id = userId });
        }

        internal static bool Exists(IDbConnection conn, IDbTransaction trans, string userId)
        {
            var sql = "select count(*) from T_SnsUsers where UserId=@UserId";
            var count = conn.ExecuteScalar<int>(sql, new { UserId = userId }, trans);
            return count > 0;
        }

        private static void UpdateCount(IDbConnection conn, IDbTransaction trans, SnsUser user)
        {
            if (user == null)
                return;

            user.ShareCount = GetCount(conn, trans, CountType.Share, user.Id);
            user.FriendCount = GetCount(conn, trans, CountType.Friend, user.Id);
            user.AttentionCount = GetCount(conn, trans, CountType.Attention, user.Id);
            user.FansCount = GetCount(conn, trans, CountType.Fans, user.Id);

            if (Exists(conn, trans, user.Id))
            {
                var sql = "update T_SnsUsers set ShareCount=@ShareCount,FriendCount=@FriendCount,AttentionCount=@AttentionCount,FansCount=@FansCount where UserId=@Id";
                conn.Execute(sql, user, trans);
            }
            else
            {
                var sql = "insert into T_SnsUsers(UserId,ShareCount,FriendCount,AttentionCount,FansCount) values(@Id,@ShareCount,@FriendCount,@AttentionCount,@FansCount)";
                conn.Execute(sql, user, trans);
            }
        }

        private static int GetCount(IDbConnection conn, IDbTransaction trans, CountType countType, string userId)
        {
            var param = new { UserId = userId };
            switch (countType)
            {
                case CountType.Share:
                    return conn.ExecuteScalar<int>("select count(*) from T_Activitys where UserId=@UserId", param, trans);
                case CountType.Friend:
                    return conn.ExecuteScalar<int>("select count(*) from T_UserFriends where (UserId=@UserId or FriendId=@UserId) and Status='1'", param, trans);
                case CountType.Attention:
                    return conn.ExecuteScalar<int>("select count(*) from T_UserAttentions where UserId=@UserId", param, trans);
                case CountType.Fans:
                    return conn.ExecuteScalar<int>("select count(*) from T_UserAttentions where AttentionId=@UserId", param, trans);
                default:
                    return 0;
            }
        }
    }
}
