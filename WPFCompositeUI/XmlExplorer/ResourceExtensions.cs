using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;

namespace XmlExplorer
{
    public static class ResourceExtensions
    {
        public static TResource TryFindResource<TResource>(this FrameworkElement element, string name)
            where TResource : class
        {
            ThrowOnNullOrEmpty(name, "name");
            return element.TryFindResource(name) as TResource;
        }

        public static TResource TryFindResource<TResource>(this Application application, string name)
            where TResource : class
        {
            ThrowOnNullOrEmpty(name, "name");
            return application.TryFindResource(name) as TResource;
        }

        internal static void ThrowOnNullOrEmpty(string parameterValue, string parameterName)
        {
            if (parameterValue == null)
            {
                throw new ArgumentNullException(parameterName);
            }

            if (parameterValue == String.Empty)
            {
                throw new ArgumentException("An empty string is not a valid parameter.", parameterName);
            }
        }
    }
}
