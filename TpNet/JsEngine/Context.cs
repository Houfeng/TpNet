using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
#if Jint
using Jint;
#elif Jurassic
using Jurassic;
#elif IronJS
using IronJS;
using IronJSHosting = IronJS.Hosting;
#elif Noesis
using Noesis.Javascript;
#endif

/*
 * IronJS 复杂难用，依赖多
 * Jurassic 无法向 JS 传递任意普通对象
 * Noesis 不完全是托管代码，需要释放资源，List 可直接遍历，对象属性不可遍历
 * Jint 完全托管代码，List 不可遍历但 ToArray 后可遍历，对象属性不可遍历
 */

namespace TpNet.JsEngine
{
    /// <summary>
    /// 脚本执行上下文
    /// </summary>
    public class Context : IDisposable
    {
#if Jint
        private Engine ScriptContext { get; set; }
#elif Jurassic
        private ScriptEngine ScriptContext { get; set; }
#elif IronJS
        private IronJSHosting.CSharp.Context ScriptContext { get; set; }
#elif Noesis
        private JavascriptContext ScriptContext { get; set; }
#endif
        public NativeJS Native { get; set; }

        /// <summary>
        /// 构造一个新一执行上下文对象
        /// </summary>
        public Context()
        {
#if Jint
            this.ScriptContext = new Engine(cfg => cfg.AllowClr());
#elif Jurassic
            this.ScriptContext = new ScriptEngine();
            this.ScriptContext.EnableExposedClrTypes = true;
#elif IronJS
            this.ScriptContext = new IronJSHosting.CSharp.Context();
#elif Noesis
            this.ScriptContext = new JavascriptContext();
#endif
            /**
             * 向 JS 上下文公开的 CLR 对象，不同于原生的 JS 对象，无法用 JS 语法扩展属性、方法
             * CLR 对象也无法在 JS 中遍历。
             */
            this.Native = new NativeJS();
            this.SetParameter("$Context", this);
            this.SetParameter("$Native", this.Native);
            var adapterScript = Utils.ReadAllTextFromRes("TpNet.Scripts.adapter.js");
            this.Execute(adapterScript);
        }

        /// <summary>
        /// 向上下文中添加一个对象变量
        /// </summary>
        /// <param name="name">变量</param>
        /// <param name="_object">对象</param>
        public void SetParameter(string name, object _object)
        {
#if Jint
            this.ScriptContext.SetValue(name, _object);
#elif Jurassic
            if (_object == null) _object = new { };
            this.ScriptContext.SetGlobalValue(name, _object);
#elif IronJS
            this.ScriptContext.SetGlobal(name, _object);
#elif Noesis
            this.ScriptContext.SetParameter(name, _object);
#endif
        }

        /// <summary>
        /// 从上下文件中获取一个参数变量
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public object GetParameter(string name)
        {
#if Jint
            return this.ScriptContext.GetValue(name);
#elif Jurassic
            return this.ScriptContext.GetGlobalValue(name);
#elif IronJS
            return this.ScriptContext.GetGlobal(name).Unbox<object>();
#elif Noesis
            return this.ScriptContext.GetParameter(name);
#endif
        }

        /// <summary>
        /// 向上下文中添加一个类型变量
        /// </summary>
        /// <param name="name">变量</param>
        /// <param name="type">类型</param>
        public void SetParameterWithType(string name, Type type)
        {
            if (type == null)
            {
                this.Execute(string.Format("var {0}=null;", name));
                return;
            }
            StringBuilder buffer = new StringBuilder();
            buffer.Append(string.Format("var {0}=function(){{return $Native.CreateInstance('{1}',$Utils.argumentsToArray(arguments));}};", name, type.FullName));
            List<string> added = new List<string>();
            MethodInfo[] methodList = type.GetMethods();
            foreach (MethodInfo method in methodList)
            {
                if (!method.IsStatic || added.Contains(method.Name))
                {
                    continue;
                }
                buffer.Append(string.Format("{0}.{2}=function(){{return $Native.InvokeStaticMethod('{1}','{2}',$Utils.argumentsToArray(arguments));}};", name, type.FullName, method.Name));
                added.Add(method.Name);
            }
            this.Execute(buffer.ToString());
        }

        /// <summary>
        /// 向上下文中添加一个类型变量
        /// </summary>
        /// <param name="name">变量</param>
        /// <param name="typeName">类型名称</param>
        public void SetParameterWithType(string name, string typeName)
        {
            Type type = this.Native.GetType(typeName);
            this.SetParameterWithType(name, type);
        }

        /// <summary>
        /// 执行一段脚本
        /// </summary>
        /// <param name="script"></param>
        /// <returns></returns>
        public object Execute(string script)
        {
#if Jint
            return this.ScriptContext.Execute(script);
#elif Jurassic
            return this.ScriptContext.Evaluate(script);
#elif IronJS
            return this.ScriptContext.Execute(script);
#elif Noesis
            return this.ScriptContext.Run(script);
#endif
        }

        /// <summary>
        /// 释放资源
        /// </summary>
        public void Dispose()
        {
            //this.InnerContext.Dispose();
        }
    }
}
