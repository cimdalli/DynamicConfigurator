using System;
using System.Collections.Generic;
using System.Linq;

namespace DynamicConfigurator.Server.Configuration
{
    public static class TypeDescriptorExtensions
    {
        public static T Create<T>(this TypeDescriptor descriptor)
        {
            var repositoryType = Type.GetType(descriptor.Type);

            if (repositoryType == null)
            {
                throw new Exception($"Descriptor is not found: {descriptor.Type}");
            }

            var allCtors = repositoryType.GetConstructors();

            var suitableCtor =
                allCtors
                    .OrderByDescending(ctor => ctor.GetParameters().Length)
                    .FirstOrDefault(ctor => ctor.GetParameters().All(info => descriptor.Args.AllKeys.Contains(info.Name)))
                    ??
                    allCtors.FirstOrDefault();

            var args = suitableCtor.GetParameters().Aggregate(new List<object>(), (list, info) =>
            {
                var arg = descriptor.Args[info.Name];
                list.Add(arg);
                return list;
            });

            return (T)suitableCtor.Invoke(args.ToArray());
        }
    }
}
