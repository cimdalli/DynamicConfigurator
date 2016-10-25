namespace DynamicConfigurator.Client.Test
{
    public class SampleConfigData 
    {
        public string Application { get; set; }

        public PersistenceSettings Persistence { get; set; }

        public int ShutdownThreshold { get; set; }
    }

    public class PersistenceSettings
    {
        public DbSettings Mongo { get; set; }
        public DbSettings Sql { get; set; }
    }

    public class DbSettings
    {
        public string Url { get; set; }
    }
}
