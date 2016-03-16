using Nancy;

namespace ConfigurationServer
{
    public class ConfigurationModule : NancyModule
    {
        public ConfigurationModule(ConfigRepository repository)
        {
            var system = repository.GetOrCreate("system", new SystemConfiguration());

            Get["/application/{application}"] = parameters =>
            {
                var application = parameters.application;

                return repository.Get(application);
                //return new
                //{
                //    Application = application,
                //    Setting1 = 1,
                //    Persistence = new
                //    {
                //        Mongo = new
                //        {
                //            Url = "mongo"
                //        },
                //        Sql = new
                //        {
                //            Url = "sql"
                //        }
                //    },
                //    Timeout = 3
                //};
            };

            Post["/register"] = ctx =>
            {
                var clientAddress = Request.UserHostAddress;
                var clients = system.Clients;

                if (!clients.Contains(clientAddress))
                {
                    clients.Add(clientAddress);
                }

                return HttpStatusCode.OK;

            };
        }
    }
}