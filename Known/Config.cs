using System.Configuration;
using System.Data;
using System.Data.Common;

namespace Known
{
    public sealed class Config
    {
        public const string DefaultConnectionName = "Default";

        public static string DbPrefix
        {
            get { return GetDbPrefix(DefaultConnectionName); }
        }

        public static IDbConnection DbConnection
        {
            get { return GetDbConnection(DefaultConnectionName); }
        }

        public static string AppSetting(string key)
        {
            return ConfigurationManager.AppSettings[key];
        }

        public static T AppSetting<T>(string key)
        {
            var value = AppSetting(key);
            return Utils.ConvertTo<T>(value);
        }

        public static string GetDbPrefix(string name)
        {
            var setting = ConfigurationManager.ConnectionStrings[name];
            switch (setting.ProviderName)
            {
                case "Oracle.DataAccess.Client":
                case "System.Data.OracleClient":
                    return ":";
                default:
                    return "@";
            }
        }

        public static IDbConnection GetDbConnection(string name)
        {
            var setting = ConfigurationManager.ConnectionStrings[name];
            var providerFactory = DbProviderFactories.GetFactory(setting.ProviderName);
            var connection = providerFactory.CreateConnection();
            connection.ConnectionString = setting.ConnectionString;
            return connection;
        }
    }
}
