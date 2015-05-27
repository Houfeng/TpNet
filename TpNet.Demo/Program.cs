using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Windows.Forms;

namespace TpNet.Demo
{
    public class Test
    {
        public string Name { get; set; }
        public static string Say(string text)
        {
            return text;
        }
    }
    class Program
    {
        public static void Main(string[] args)
        {
            Stopwatch watch = new Stopwatch(); 
            watch.Start();
            var code1 = @"
遍历:
<% 
$Native.ToJSON({name:'houfeng'});
var model = $Utils.toJs(this);
for(var i in model){
    $(i+':'+model[i].name+'\r\n'); 
}
%>

循环:
<% 
var model = this.ToArray();
for(var i=0;i<model.length;i++){
    $(i+':'+model[i].name+'\r\n'); 
}
%>

扩展:
<%

if($.Say){
    var rs= $.Say('houfeng');
    $(rs);
}
%>
";

            var tp = Tp.Compile(code1);
            Console.WriteLine("编译时间: " + watch.ElapsedMilliseconds);
            watch.Restart();
            var model = new List<object>();
            model.Add(new { name = "houfeng" });
            Console.WriteLine(tp.Execute(model, typeof(Test)).ToString());
            Console.WriteLine("执行时间: " + watch.ElapsedMilliseconds);

            //--
            Application.Run();
        }
    }
}
