using System;
using System.Collections.Generic;
using System.Linq;

namespace VsTools.Projects
{
    public static class Reflection
    {
        private static readonly Dictionary<string, Type> _typelookup;

        static Reflection()
        {
            _typelookup = typeof(Item).Assembly.GetTypes().Where(x => typeof(Item).IsAssignableFrom(x)).ToDictionary(x => x.Name);
        }

        public static Type GetItemGroupContentTypeFromName(string name)
        {
            Type type;
            if (!_typelookup.TryGetValue(name, out type))
            {
                return typeof(Item);
            }
            return type;
        }
    }
}