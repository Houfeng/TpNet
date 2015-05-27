using System;

namespace TpNet
{
    public class Option
    {
        /// <summary>
        /// 代码块开始标记
        /// </summary>
        public string CodeBegin { get; set; }

        /// <summary>
        /// 代码块结束标记
        /// </summary>
        public string CodeEnd { get; set; }

        /// <summary>
        /// 编译或角析时扩展模板方法
        /// </summary>
        public Type Extend { get; set; }

        private void Init(string codeBegin, string codeEnd, Type extend)
        {
            this.CodeBegin = codeBegin;
            this.CodeEnd = codeEnd;
            this.Extend = extend;
        }

        /// <summary>
        /// 构造选项
        /// </summary>
        /// <param name="codeBegin">代码块开始标记</param>
        /// <param name="codeEnd">代码块结束标记</param>
        /// <param name="extend">编译或角析时扩展模板方法</param>
        public Option(string codeBegin, string codeEnd, Type extend)
        {
            this.Init(codeBegin, codeEnd, extend);
        }

        /// <summary>
        /// 构造选项
        /// </summary>
        /// <param name="codeBegin">代码块开始标记</param>
        /// <param name="codeEnd">代码块结束标记</param>
        public Option(string codeBegin, string codeEnd)
        {
            this.Init(codeBegin, codeEnd, null);
        }

        /// <summary>
        /// 构造选项
        /// </summary>
        /// <param name="extend">编译或角析时扩展模板方法</param>
        public Option(Type extend)
        {
            this.Init(null, null, extend);
        }
    }
}
