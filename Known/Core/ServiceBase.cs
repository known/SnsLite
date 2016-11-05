using System;
using System.Data;

namespace Known.Core
{
    public class ServiceBase
    {
        protected IDbConnection connection = Config.DbConnection;

        protected T LoadService<T>()
        {
            return ServiceFactory.GetService<T>();
        }
    }

    public class ServiceFactory
    {
        public static T GetService<T>()
        {
            var service = Container.Load<T>();
            if (service == null)
                throw new Exception(string.Format("服务{0}没有注册！", typeof(T)));

            return service;
        }
    }
}
