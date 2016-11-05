using Known;
using Known.Core;
using System.Collections.Generic;

namespace SnsLite.Services
{
    public interface ICompanyService : ISnsUserService
    {
        Company GetCompany(string userId);
        Result UpdateCompany(Company company);
        List<CodeTable> GetRegions();
        List<CodeTable> GetTrades();
        List<Company> SearchCompanys(string region, string trade, string q, int pageIndex, out int total);
    }
}