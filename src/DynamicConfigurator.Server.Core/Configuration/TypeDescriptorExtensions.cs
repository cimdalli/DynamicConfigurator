using System;
using System.Collections.Generic;
using System.Linq;

namespace DynamicConfigurator.Server.Configuration
{
    public static class TypeDescriptorExtensions
    {
        public static object Create(this TypeDescriptor descriptor)
        {
            var type = Type.GetType(descriptor.Type);

            if (type == null)
            {
                throw new Exception($"Descriptor is not found: {descriptor.Type}");
            }

            var allCtors = type.GetConstructors();

            var suitableCtor = allCtors
                .OrderByDescending(ctor => ctor.GetParameters().Length)
                .FirstOrDefault(ctor => ctor.GetParameters().All(info => descriptor.Args.AllKeys.Contains(info.Name))) 
                ?? 
                allCtors.FirstOrDefault();

            if (suitableCtor == null)
            {
                throw new Exception($"Suitable constructor is not found: {type.AssemblyQualifiedName}" );
            }

            var args = suitableCtor.GetParameters().Aggregate(new List<object>(), (list, info) =>
            {
                var arg = descriptor.Args[info.Name];
                list.Add(arg);
                return list;
            });

            return suitableCtor.Invoke(args.ToArray());
        }
    }
}
