using TpNet.Reflection;
using System.Collections.Generic;
using System.Reflection;

namespace TpNet.Extends
{
    public static class MethodExtends
    {
        public static T GetAttribute<T>(this MethodInfo method)
        {
            return AttributeFactory.GetAttribute<T>(method);
        }
        public static List<T> GetAttributes<T>(this MethodInfo method)
        {
            return AttributeFactory.GetAttributes<T>(method);
        }
    }
}
