using Dapper;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace SnsLite
{
    static class Extensions
    {
        public static int QueryCount(this IDbConnection connection, string sql, object param = null)
        {
            var sqlCount = string.Format("select count(1) from ({0}) t", sql);
            return connection.ExecuteScalar<int>(sqlCount, param);
        }

        public static IEnumerable<T> QueryPage<T>(this IDbConnection connection, string sql, string[] orderFields, int pageIndex, object param = null)
        {
            var sqlPage = GetPagingSql(sql, orderFields, Setting.PageSize, pageIndex);
            return connection.Query<T>(sqlPage, param);
        }

        public static string GetPagingSql(string sql, string[] orderFields, int pageSize, int pageIndex)
        {
            var orderBy = string.Join(",", orderFields.Select(f => string.Format("t1.{0}", f)));
            return string.Format("select top {0} t.* from (select t1.*,row_number() over (order by {1}) RowNo from ({2}) t1) t where t.RowNo>{3}", pageSize, orderBy, sql, pageSize * (pageIndex - 1));
        }
    }
}
