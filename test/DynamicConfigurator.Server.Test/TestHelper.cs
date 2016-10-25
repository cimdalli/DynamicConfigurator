namespace DynamicConfigurator.Server.Test
{
    public class TestHelper
    {
        public const string Environment = "environment";
        public const string Client = "client";

        public static dynamic GetSampleConfig()
        {
            return new
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
        }

        public static dynamic GetEnvironmentOverrideConfig()
        {
            return new
            {
                Persistence = new
                {
                    Mongo = new
                    {
                        Url = "environment-mongo"
                    }
                }
            };
        }
    }
}
