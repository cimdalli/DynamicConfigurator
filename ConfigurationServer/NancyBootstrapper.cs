using Nancy;
using Nancy.TinyIoc;

namespace ConfigurationServer
{
    public class NancyBootstrapper : DefaultNancyBootstrapper
    {
        protected override void ConfigureApplicationContainer(TinyIoCContainer container)
        {
            container.Register(new InMemoryConfigRepository());
            base.ConfigureApplicationContainer(container);
        }
    }
}
