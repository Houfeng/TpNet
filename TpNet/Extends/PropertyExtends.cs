using TpNet.Reflection;
using System.Collections.Generic;
using System.Reflection;

namespace TpNet.Extends
{
    public static class PropertyExtends
    {
        public static T GetAttribute<T>(this PropertyInfo property)
        {
            return AttributeFactory.GetAttribute<T>(property);
        }
        public static List<T> GetAttributes<T>(this PropertyInfo property)
        {
            return AttributeFactory.GetAttributes<T>(property);
        }
    }
}
