using System;
using System.Configuration.Abstractions;
using Newtonsoft.Json;

namespace DynamicConfigurator.Server.Configuration
{
    public class ServerSettings
    {
        private static ServerSettings _serverSettings;
        public static ServerSettings Current()
        {
            return _serverSettings ?? (_serverSettings = new ServerSettings
            {
                App = ConfigurationManager.Instance.AppSettings.Map<AppSettings>(),
                Component = ConfigurationManager.Instance.GetSection<ComponentSettings>("dynamicConfigurator")
            });
        }

        public ComponentSettings Component { get; set; }
        public AppSettings App { get; set; }
    }

    public class ComponentSettings
    {
        public TypeDescriptor Repository { get; set; }
    }

    public class AppSettings
    {
        public string BaseUrl { get; set; }
        public string SystemKey { get; set; }
        public string DefaultKey { get; set; }
        public int ServerPort { get; set; }
        public string Formatting { get; set; }
        public Formatting FormattingValue
        {
            get
            {
                Formatting formatting;
                Enum.TryParse(Formatting, out formatting);
                return formatting;
            }
            set { Formatting = value.ToString(); }
        }
    }
}
