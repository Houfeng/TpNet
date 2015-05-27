using System.IO;
using System.Reflection;

namespace TpNet
{
    /// <summary>
    /// 辅助工具类
    /// </summary>
    public class Utils
    {
        /// <summary>
        /// 从嵌入资源中读取文本
        /// </summary>
        /// <param name="fullName"></param>
        /// <returns></returns>
        public static string ReadAllTextFromRes(string fullName)
        {
            Assembly assembly = typeof(Utils).Assembly;
            Stream stream = assembly.GetManifestResourceStream(fullName);
            StreamReader reader = new StreamReader(stream);
            string str = reader.ReadToEnd();
            return str;
        }
    }
}
