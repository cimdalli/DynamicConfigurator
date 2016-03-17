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

            var result3 = ConfigurationClient.Instance.GetConfiguration<Test2Settings>("test2");
            var result4 = ConfigurationClient.Instance.GetConfiguration("system");
        }
    }

    public class Test2Settings
    {
        public string Application { get; set; }
        public PersistenceSettings Persistence { get; set; }
        public string NotPrimitive { get; set; }
        public PersistenceSettings NotComplex { get; set; }
    }

    public class PersistenceSettings
    {
        public Persistence Mongo { get; set; }
    }

    public class Persistence
    {
        public string Url { get; set; }
    }
}
