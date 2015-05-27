using System;
using TpNet.JsEngine;

namespace TpNet
{
    /// <summary>
    /// Tp 模板引擎 .NET 版
    /// 基于 Tp 模板引擎 JavaScript 版的 v3.2
    /// </summary>
    public class Tp
    {
        /// <summary>
        /// 使用的脚本引擎
        /// </summary>
        public static Context ScriptContext { get; set; }

        /// <summary>
        /// 静态构造
        /// </summary>
        static Tp()
        {
            ScriptContext = new Context();
            string tpScript = Utils.ReadAllTextFromRes("TpNet.Scripts.tp.js");
            ScriptContext.Execute(tpScript);
        }

        /// <summary>
        /// 产生一个 GUID
        /// </summary>
        /// <returns></returns>
        private static string CreateVariableName()
        {
            return "__" + Guid.NewGuid().ToString().Replace("-", "");
        }

        /// <summary>
        /// 构造一个编译后模板对象
        /// </summary>
        /// <param name="functionName">js 函数名</param>
        private Tp(string functionName)
        {
            this.FunctionName = functionName;
        }

        /// <summary>
        /// 对应的 js 函数名
        /// </summary>
        private string FunctionName { get; set; }

        /// <summary>
        /// 执行编译后的模板
        /// </summary>
        /// <param name="model">模型数据对象</param>
        /// <param name="extend">执行时扩展模板方法</param>
        /// <returns>模板执行结果</returns>
        public string Execute(object model, Type extend)
        {
            string resultName = CreateVariableName();
            string modelName = CreateVariableName();
            string extendName = CreateVariableName();
            //
            //
            ScriptContext.SetParameter(modelName, model);
            ScriptContext.SetParameterWithType(extendName, extend);
            //
            var compileCode = string.Format("var {0}={1}({2},{3});",
                resultName,
                FunctionName,
                modelName,
                extendName);
            ScriptContext.Execute(compileCode);
            var result = Convert.ToString(ScriptContext.GetParameter(resultName));
            //
            var clearCode = string.Format("{0}={1}={2}=null;",
                resultName,
                modelName,
                extendName);
            ScriptContext.Execute(clearCode);
            //
            return result;
        }

        /// <summary>
        /// 执行编译后的模板
        /// </summary>
        /// <param name="model">模型数据对象</param>
        /// <returns>模板执行结果</returns>
        public string Execute(object model)
        {
            return Execute(model, null);
        }
        /// <summary>
        /// 代码块开始标记
        /// </summary>
        public static string CodeBegin { get; set; }

        /// <summary>
        /// 代码块结束标记
        /// </summary>
        public static string CodeEnd { get; set; }

        /// <summary>
        /// 全局扩展模板方法
        /// </summary>
        /// <param name="extObject">扩展方法所属对象</param>
        public static void Extend(Type extend)
        {
            string extendName = CreateVariableName();
            //
            ScriptContext.SetParameterWithType(extendName, extend);
            //
            var extendCode = string.Format("tp.extend({0});", extendName);
            ScriptContext.Execute(extendCode);
            //
            var clearCode = string.Format("{0}=null;", extendName);
            ScriptContext.Execute(clearCode);
        }

        /// <summary>
        /// 编译一个模板
        /// </summary>
        /// <param name="source">模板源</param>
        /// <param name="option">编译选项</param>
        /// <returns>编译后的模板对象</returns>
        public static Tp Compile(string source, Option option)
        {
            string resultName = CreateVariableName();
            string sourceName = CreateVariableName();
            string optionName = CreateVariableName();
            string optionExtendName = CreateVariableName();
            //
            ScriptContext.SetParameter(sourceName, source);
            ScriptContext.SetParameter(optionName, option);
            //
            if (option != null)
            {
                ScriptContext.SetParameterWithType(optionExtendName, option.Extend);
                ScriptContext.Execute(string.Format("{0}.extend={1};", optionName, optionExtendName));
            }
            //
            var compileCode = string.Format("var {0} = tp.compile({1},{2});",
                resultName,
                sourceName,
                optionName);
            ScriptContext.Execute(compileCode);
            var result = Convert.ToString(ScriptContext.GetParameter(resultName));
            //
            var clearCode = string.Format("{0}={1}=null;",
                sourceName,
                optionName);
            ScriptContext.Execute(clearCode);
            //
            Tp tp = new Tp(resultName);
            return tp;
        }

        /// <summary>
        /// 编译一个模板
        /// </summary>
        /// <param name="source">模板源</param>
        /// <returns></returns>
        public static Tp Compile(string source)
        {
            return Compile(source, null);
        }

        /// <summary>
        /// 解析一个模板
        /// </summary>
        /// <param name="source">模板源</param>
        /// <param name="model">模型数据对象</param>
        /// <param name="option">解析选项</param>
        /// <param name="extend">执行时扩展模板方法</param>
        /// <returns>模板执行结果</returns>
        public static string Parse(string source, object model, Option option, Type extend)
        {
            string resultName = CreateVariableName();
            string sourceName = CreateVariableName();
            string modelName = CreateVariableName();
            string optionName = CreateVariableName();
            string extendName = CreateVariableName();
            //
            ScriptContext.SetParameter(sourceName, source);
            ScriptContext.SetParameter(modelName, model);
            ScriptContext.SetParameter(optionName, option);
            ScriptContext.SetParameterWithType(extendName, extend);
            //
            var parseCode = string.Format("var {0} = tp.parse({1},{2},{3},{4});",
                resultName,
                sourceName,
                modelName,
                optionName,
                extendName);
            ScriptContext.Execute(parseCode);
            var result = Convert.ToString(ScriptContext.GetParameter(resultName));
            //
            var clearCode = string.Format("{0}={1}={2}={3}={4}=null;",
                resultName,
                sourceName,
                modelName,
                optionName,
                extendName);
            ScriptContext.Execute(clearCode);
            //
            return result;
        }

        /// <summary>
        /// 解析一个模板
        /// </summary>
        /// <param name="source">模板源</param>
        /// <param name="model">模型数据对象</param>
        /// <param name="option">解析选项</param>
        /// <returns>模板执行结果</returns>
        public static string Parse(string source, object model, Option option)
        {
            return Parse(source, model, option, null);
        }

        /// <summary>
        /// 解析一个模板
        /// </summary>
        /// <param name="source">模板源</param>
        /// <param name="model">模型数据对象</param>
        /// <returns>模板执行结果</returns>
        public static string Parse(string source, object model)
        {
            return Parse(source, model, null);
        }
    }
}
