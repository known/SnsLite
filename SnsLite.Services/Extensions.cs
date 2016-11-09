using Known;
using System.Collections.Generic;
using System.Data;

namespace SnsLite
{
    static class Extensions
    {
        public static IEnumerable<T> QueryPage<T>(this IDbConnection connection, string sql, string[] orderFields, int pageIndex, object param = null)
        {
            return connection.QueryPage<T>(sql, orderFields, Setting.PageSize, pageIndex, param);
        }
    }
}
