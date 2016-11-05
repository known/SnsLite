using Dapper;
using Known;
using Known.Core;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace SnsLite.Services
{
    public class SnsUserService : UserService, ISnsUserService
    {
        public SnsUser GetSnsUser(string account)
        {
            var sql = "select * from K_Users a left join T_SnsUsers b on b.UserId=a.Id where a.Account=@Account";
            var user = connection.QuerySingleOrDefault<SnsUser>(sql, new { Account = account });
            return user;
        }

        public SnsUser GetSnsUserById(string id)
        {
            var sql = "select * from K_Users a left join T_SnsUsers b on b.UserId=a.Id where a.Id=@Id";
            var user = connection.QuerySingleOrDefault<SnsUser>(sql, new { Id = id });
            return user;
        }

        public void UpdateSnsCount(SnsUser user)
        {
            SnsUserHelper.UpdateCount(connection, user);
        }

        public Result UpdateAvatar(string id, string avatar)
        {
            return connection.Transaction("修改", (conn, trans) =>
            {
                var param = new { Avatar = avatar, Id = id };
                if (SnsUserHelper.Exists(conn, trans, id))
                {
                    var sql = "update T_SnsUsers set Avatar=@Avatar where UserId=@Id";
                    conn.Execute(sql, param, trans);
                }
                else
                {
                    var sql = "insert into T_SnsUsers(UserId,Avatar) values(@Id,@Avatar)";
                    conn.Execute(sql, param, trans);
                }
            });
        }

        public Result UpdateUser(SnsUser user)
        {
            if (user == null)
                return Result.Error("User对象为空！");

            if (!ExistsUser(connection , null, user.Id))
                return Result.Error("用户不存在！");

            return connection.Transaction("修改", (conn, trans) =>
            {
                UpdateSnsUser(conn, trans, user);
            });
        }

        protected void UpdateSnsUser(IDbConnection conn, IDbTransaction trans, SnsUser user)
        {
            if (SnsUserHelper.Exists(conn, trans, user.Id))
            {
                var sql = "update T_SnsUsers set Introduction=@Introduction,FriendValidate=@FriendValidate,AllowComment=@AllowComment where UserId=@Id";
                conn.Execute(sql, user, trans);
            }
            else
            {
                var sql = "insert into T_SnsUsers(UserId,Introduction,FriendValidate,AllowComment) values(@Id,@Introduction,@FriendValidate,@AllowComment)";
                conn.Execute(sql, user, trans);
            }

            UpdateUser(conn, trans, user);
        }

        public List<SnsUser> GetActiveUsers()
        {
            var sql = string.Format(@"select top {0} * from K_Users a left join T_SnsUsers b on b.UserId=a.Id order by a.LoginCount desc", Setting.GalleryUserSize);
            return connection.Query<SnsUser>(sql).ToList();
        }

        public List<SnsUser> GetNewestUsers()
        {
            var sql = string.Format(@"select top {0} * from K_Users a left join T_SnsUsers b on b.UserId=a.Id order by a.RegisterTime desc", Setting.GalleryUserSize);
            return connection.Query<SnsUser>(sql).ToList();
        }

        public List<SnsUser> GetFriendUsers(string userId)
        {
            var sql = string.Format(@"select top {0} * from K_Users a left join T_SnsUsers b on b.UserId=a.Id where exists (select 1 from T_UserFriends where ((UserId=@UserId and FriendId=a.Id) or (UserId=a.Id and FriendId=@UserId)) and Status='1')", Setting.GalleryUserSize);
            return connection.Query<SnsUser>(sql, new { UserId = userId }).ToList();
        }

        public bool CheckFriend(string userId, string friendId, bool? isValidate = null)
        {
            var sql = "select count(*) from T_UserFriends where ((UserId=@UserId and FriendId=@FriendId) or (UserId=@FriendId and FriendId=@UserId))";
            if (isValidate.HasValue)
            {
                sql += " and Status=" + (isValidate.Value ? "1" : "0");
            }
            var count = connection.ExecuteScalar<int>(sql, new { UserId = userId, FriendId = friendId });
            return count > 0;
        }

        public Result CreateFriend(string userId, string friendId, bool isValidate, string requestMessage)
        {
            if (CheckFriend(userId, friendId, false))
                return Result.Error("好友关系已存在！");

            if (!ExistsUser(connection, null, userId))
                return Result.Error("当前用户不存在！");

            if (!ExistsUser(connection, null, friendId))
                return Result.Error("好友用户不存在！");

            if (isValidate && string.IsNullOrWhiteSpace(requestMessage))
                return Result.Error("验证消息不能为空！");

            var sql = "insert into T_UserFriends(Id,UserId,FriendId,Status,RequestMessage,ResponseMessage,ResponseTime) values(@Id,@UserId,@FriendId,@Status,@RequestMessage,@ResponseMessage,@ResponseTime)";
            var param = new
            {
                Id = Utils.NewGuid,
                UserId = userId,
                FriendId = friendId,
                Status = isValidate ? "0" : "1",
                RequestMessage = requestMessage,
                ResponseMessage = isValidate ? null : "无需验证",
                ResponseTime = isValidate ? new DateTime?() : DateTime.Now
            };

            return connection.Transaction("添加", (conn, trans) =>
            {
                conn.Execute(sql, param, trans);
                SnsUserHelper.UpdateCount(conn, trans, userId);
            });
        }

        public Result AdoptFriend(string userId, string friendId, bool passed, string responseMessage)
        {
            if (!CheckFriend(userId, friendId))
                return Result.Error("好友关系不存在！");

            var sql = "update T_UserFriends set Status=@Status,ResponseMessage=@ResponseMessage,ResponseTime=@ResponseTime where UserId=@UserId and FriendId=@FriendId";
            var param = new
            {
                Status = passed ? "1" : "2",
                ResponseMessage = responseMessage,
                ResponseTime = DateTime.Now,
                UserId = userId,
                FriendId = friendId
            };

            return connection.Transaction("接受", (conn, trans) =>
            {
                conn.Execute(sql, param, trans);
                SnsUserHelper.UpdateCount(conn, trans, userId);
            });
        }

        public Result RemoveFriend(string userId, string friendId)
        {
            if (!CheckFriend(userId, friendId))
                return Result.Error("好友关系不存在！");

            var sql = "delete from T_UserFriends where UserId=@UserId and FriendId=@FriendId";
            var param = new { UserId = userId, FriendId = friendId };

            return connection.Transaction("删除", (conn, trans) =>
            {
                conn.Execute(sql, param, trans);
                SnsUserHelper.UpdateCount(conn, trans, userId);
            });
        }

        public bool CheckAttention(string userId, string attentionId)
        {
            var sql = "select count(*) from T_UserAttentions where UserId=@UserId and AttentionId=@AttentionId";
            var count = connection.ExecuteScalar<int>(sql, new { UserId = userId, AttentionId = attentionId });
            return count > 0;
        }

        public Result CreateAttention(string userId, string attentionId)
        {
            if (CheckAttention(userId, attentionId))
                return Result.Error("关注关系已存在！");

            if (!ExistsUser(connection, null, userId))
                return Result.Error("当前用户不存在！");

            if (!ExistsUser(connection, null, attentionId))
                return Result.Error("关注用户不存在！");

            var sql = "insert into T_UserAttentions(UserId,AttentionId) values(@UserId,@AttentionId)";
            var param = new { UserId = userId, AttentionId = attentionId };

            return connection.Transaction("关注", (conn, trans) =>
            {
                conn.Execute(sql, param, trans);
                SnsUserHelper.UpdateCount(conn, trans, userId);
                SnsUserHelper.UpdateCount(conn, trans, attentionId);
            });
        }

        public Result RemoveAttention(string userId, string attentionId)
        {
            if (!CheckAttention(userId, attentionId))
                return Result.Error("关注关系不存在！");

            var sql = "delete from T_UserAttentions where UserId=@UserId and AttentionId=@AttentionId";
            var param = new { UserId = userId, AttentionId = attentionId };

            return connection.Transaction("取消", (conn, trans) =>
            {
                conn.Execute(sql, param, trans);
                SnsUserHelper.UpdateCount(conn, trans, userId);
                SnsUserHelper.UpdateCount(conn, trans, attentionId);
            });
        }
    }
}
