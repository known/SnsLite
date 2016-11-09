using Dapper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web.Script.Serialization;

namespace Known
{
    public static class Extensions
    {
        public static string ToJson(this object value)
        {
            var serializer = new JavaScriptSerializer();
            return serializer.Serialize(value);
        }

        public static T ToObject<T>(this string input)
        {
            var serializer = new JavaScriptSerializer();
            return serializer.Deserialize<T>(input);
        }

        public static Result Transaction(this IDbConnection connection, string name, Action<IDbConnection, IDbTransaction> action)
        {
            connection.Open();
            var trans = connection.BeginTransaction();
            try
            {
                action(connection, trans);
                trans.Commit();
                return Result.Success(name + "成功！");
            }
            catch (Exception ex)
            {
                trans.Rollback();
                return Result.Error(name + "失败，原因：" + ex.Message);
            }
            finally
            {
                connection.Close();
            }
        }

        public static Result<T> Transaction<T>(this IDbConnection connection, string name, Action<IDbConnection, IDbTransaction> action, T data)
        {
            var result = connection.Transaction(name, action);
            if (result.IsValid)
            {
                return Result.Success(result.Message, data);
            }

            return Result.Error<T>(result.Message);
        }

        public static int QueryCount(this IDbConnection connection, string sql, object param = null)
        {
            var sqlCount = string.Format("select count(1) from ({0}) t", sql);
            return connection.ExecuteScalar<int>(sqlCount, param);
        }

        public static IEnumerable<T> QueryPage<T>(this IDbConnection connection, string sql, string[] orderFields, int pageSize, int pageIndex, object param = null)
        {
            var sqlPage = Utils.GetPagingSql(sql, orderFields, pageSize, pageIndex);
            return connection.Query<T>(sqlPage, param);
        }

        public static string ToAgo(this DateTime target)
        {
            var timeSpan = DateTime.Now - target;
            if (timeSpan < TimeSpan.Zero) return "未来";
            if (timeSpan < Helper.OneMinute) return "刚刚";
            if (timeSpan < Helper.TwoMinutes) return "1 分钟前";
            if (timeSpan < Helper.OneHour) return string.Format("{0} 分钟前", timeSpan.Minutes);
            if (timeSpan < Helper.TwoHours) return "1 小时前";
            if (timeSpan < Helper.OneDay) return string.Format("{0} 小时前", timeSpan.Hours);
            if (timeSpan < Helper.TwoDays) return "昨天";
            if (timeSpan < Helper.OneWeek) return string.Format("{0} 天前", timeSpan.Days);
            if (timeSpan < Helper.TwoWeeks) return "1 周前";
            if (timeSpan < Helper.OneMonth) return string.Format("{0} 周前", timeSpan.Days / 7);
            if (timeSpan < Helper.TwoMonths) return "1 月前";
            //if (timeSpan < Helper.OneYear) return string.Format("{0} 月前", timeSpan.Days / 31);
            //if (timeSpan < Helper.TwoYears) return "1 年前";

            return string.Format("{0:yyyy-MM-dd HH:mm}", target);
        }

        class Helper
        {
            internal static readonly TimeSpan OneMinute = new TimeSpan(0, 1, 0);
            internal static readonly TimeSpan TwoMinutes = new TimeSpan(0, 2, 0);
            internal static readonly TimeSpan OneHour = new TimeSpan(1, 0, 0);
            internal static readonly TimeSpan TwoHours = new TimeSpan(2, 0, 0);
            internal static readonly TimeSpan OneDay = new TimeSpan(1, 0, 0, 0);
            internal static readonly TimeSpan TwoDays = new TimeSpan(2, 0, 0, 0);
            internal static readonly TimeSpan OneWeek = new TimeSpan(7, 0, 0, 0);
            internal static readonly TimeSpan TwoWeeks = new TimeSpan(14, 0, 0, 0);
            internal static readonly TimeSpan OneMonth = new TimeSpan(31, 0, 0, 0);
            internal static readonly TimeSpan TwoMonths = new TimeSpan(62, 0, 0, 0);
            internal static readonly TimeSpan OneYear = new TimeSpan(365, 0, 0, 0);
            internal static readonly TimeSpan TwoYears = new TimeSpan(730, 0, 0, 0);
        }
    }
}
