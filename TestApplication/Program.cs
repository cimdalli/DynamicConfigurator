using System.Collections.Generic;

namespace DynamicConfigurator.Client
{
    public class Program
    {
        static void Main(string[] args)
        {
            var data = new
            {
                Application = "test2",
                Setting1 = 1,
                Persistence = new
                {
                    Mongo = new
                    {
                        Url = "mongo"
                    },
                    Sql = new
                    {
                        Url = "sql"
                    }
                },
                Timeout = 3
            };


            var result1 = ConfigurationClient.Instance.GetConfiguration("test1");
            var result2 = ConfigurationClient.Instance.GetConfiguration("test2");

            ConfigurationClient.Instance.SetConfiguration("test2", data);

            var result3 = ConfigurationClient.Instance.GetConfiguration("test2");
            var result4 = ConfigurationClient.Instance.GetConfiguration("system");
        }
    }
}
