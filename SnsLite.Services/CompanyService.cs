using Dapper;
using Known;
using Known.Core;
using System.Collections.Generic;
using System.Linq;

namespace SnsLite.Services
{
    public class CompanyService : SnsUserService, ICompanyService
    {
        public Company GetCompany(string userId)
        {
            var sql = "select * from K_Users a left join T_SnsUsers b on b.UserId=a.Id left join T_Companys c on c.CompanyId=a.Id where a.Id=@Id";
            var company = connection.QuerySingleOrDefault<Company>(sql, new { Id = userId });
            return company;
        }

        public Result UpdateCompany(Company company)
        {
            if (company == null)
                return Result.Error("Company对象为空！");

            if (!ExistsUser(connection, null, company.Id))
                return Result.Error("公司不存在！");

            var sql = "select count(*) from T_Companys where CompanyId=@CompanyId";
            var count = connection.ExecuteScalar<int>(sql, new { CompanyId = company.Id });
            if (count > 0)
            {

            }
            else
            {

            }

            return Result.Success("修改成功！");
        }

        public List<CodeTable> GetRegions()
        {
            var sql = "select a.Region Code,b.Name,count(1) cnt from T_Companys a left join K_CodeTables b on b.Code=a.Region and b.Category='Region' group by a.Region,b.Name order by cnt desc";
            return connection.Query<CodeTable>(sql).ToList();
        }

        public List<CodeTable> GetTrades()
        {
            var sql = "select a.Trade Code,b.Name,count(1) cnt from T_Companys a left join K_CodeTables b on b.Code=a.Trade and b.Category='Trade' group by a.Trade,b.Name order by cnt desc";
            return connection.Query<CodeTable>(sql).ToList();
        }

        class SearchParam
        {
            public string Q { get; internal set; }
            public string Region { get; internal set; }
            public string Trade { get; internal set; }
        }

        public List<Company> SearchCompanys(string region, string trade, string q, int pageIndex, out int total)
        {
            var param = new SearchParam();
            var sql = "select * from K_Users a left join T_SnsUsers b on b.UserId=a.Id left join T_Companys c on c.CompanyId=a.Id where 1=1";
            if (!string.IsNullOrWhiteSpace(region))
            {
                sql += " and c.Region=@Region";
                param.Region = region;
            }
            if (!string.IsNullOrWhiteSpace(trade))
            {
                sql += " and c.Trade=@Trade";
                param.Trade = trade;
            }
            if (!string.IsNullOrWhiteSpace(q))
            {
                sql += " and (c.Code like @Q or a.Name like @Q)";
                param.Q = "%" + q + "%";
            }

            total = connection.QueryCount(sql, param);
            return connection.QueryPage<Company>(sql, new string[] { "LoginCount desc" }, pageIndex, param).ToList();
        }
    }
}