using Known;
using Known.Core;

namespace SnsLite.Services
{
    public class SnsServiceFactory
    {
        public static void RegisterAll()
        {
            Container.Register<ILogService, LogService>();
            Container.Register<IUserService, UserService>();
            Container.Register<ISnsUserService, SnsUserService>();
            Container.Register<ICompanyService, CompanyService>();
            Container.Register<IActivityService, ActivityService>();
        }
    }
}
