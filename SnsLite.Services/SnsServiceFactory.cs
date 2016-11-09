using Known;

namespace SnsLite.Services
{
    public class SnsServiceFactory
    {
        public static void RegisterAll()
        {
            Container.Register<ISnsUserService, SnsUserService>();
            Container.Register<ICompanyService, CompanyService>();
            Container.Register<IActivityService, ActivityService>();
        }
    }
}
