using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using TpNet.Reflection;
using TpNet.Serializes;

namespace TpNet.JsEngine
{
    /// <summary>
    /// Native 扩展类型 
    /// </summary>
    public class NativeJS
    {
        private static JsonSerializer _Serializer = null;
        public JsonSerializer Serializer
        {
            get
            {
                if (_Serializer == null)
                    _Serializer = new JsonSerializer();
                return _Serializer;
            }
        }
        /// <summary>
        /// 构造一个 NativeJs
        /// </summary>
        /// <param name="context"></param>
        public NativeJS() { }
        public Type GetType(string typeName)
        {
            return TypeFactory.GetType(typeName);
        }
        public MethodInfo GetMethod(Type type, string methodName, Type[] types)
        {
            return MethodFactory.GetMethodInfo(type, methodName, types);
        }
        public MethodInfo GetMethod(string typeName, string methodName, Type[] types)
        {
            return this.GetMethod(this.GetType(typeName), methodName, types);
        }
        public object InvokeStaticMethod(string typeName, string methodName, object[] arguments)
        {
            MethodInfo method = this.GetMethod(typeName, methodName, GetArgumentsTypeList(arguments));
            if (method == null || !method.IsStatic) return null;
            return method.Invoke(null, arguments);
        }
        public object CreateInstance(string typeName, object[] arguments)
        {
            Type type = this.GetType(typeName);
            if (type == null) return null;
            if (arguments != null)
            {
                return InstanceFactory.GetInstance(type, arguments);
            }
            else
            {
                return InstanceFactory.GetInstance(type);
            }
        }
        public string Replace(string str, string str1, string str2)
        {
            return str.Replace(str1, str2);
        }
        public static Type[] GetArgumentsTypeList(object[] arguments)
        {
            if (arguments == null && arguments.Count() <= 0)
            {
                return null;
            }
            List<Type> typeList = new List<Type>();
            foreach (var argument in arguments)
            {
                if (argument != null)
                {
                    typeList.Add(argument.GetType());
                }
                else
                {
                    typeList.Add(typeof(object));
                }
            }
            return typeList.ToArray();
        }
        public string ToJSON(object _object)
        {
            return this.Serializer.Serialize(_object);
        }
    }
}
