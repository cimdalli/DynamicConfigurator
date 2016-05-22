using System;
using System.Collections.Specialized;
using System.Configuration;
using System.Linq;
using System.Xml;

namespace DynamicConfigurator.Server.Configuration
{
    public class ConfigurationSectionHandler : IConfigurationSectionHandler
    {
        private const string RepositoryNode = "repository";
        private const string TypeAttribute = "type";

        public object Create(object parent, object configContext, XmlNode section)
        {
            return ReadConfiguration(section);
        }

        private static ComponentSettings ReadConfiguration(XmlNode section)
        {
            var defaultSettings = new ComponentSettings();

            foreach (XmlNode childNode in section.ChildNodes)
            {
                switch (childNode.Name)
                {
                    case RepositoryNode:
                        var attributes = childNode.Attributes;

                        if (attributes == null)
                        {
                            throw new Exception("Server repository settings has not been set");
                        }

                        var type = attributes[TypeAttribute].Value;

                        if (type == null)
                        {
                            throw new Exception("Server repository settings has not been set");
                        }

                        var argAttributes = (from attr in attributes.Cast<XmlAttribute>() select attr)
                            .Where(attribute => attribute.Name != TypeAttribute)
                            .Aggregate(new NameValueCollection(), (collection, attribute) =>
                            {
                                collection.Add(attribute.Name, attribute.Value);
                                return collection;
                            });


                        defaultSettings.Repository = new TypeDescriptor(type, argAttributes);

                        break;
                }
            }

            return defaultSettings;
        }



    }
}
