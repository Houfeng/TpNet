(function (owner) {
    var $Utils = owner.$Utils = (owner.$Utils || {});
    //将函数的 arguments 转为数组
    $Utils.argumentsToArray = function (_arguments) {
        if (_arguments === null) return null;
        var rs = [];
        for (var i in _arguments) {
            rs.push(_arguments[i]);
        }
        return rs;
    };
    //转换 CLR 对象为真正的 JavaScript 对象，将 “丢失” 方法
    $Utils.toJs = $Utils.toJS = $Utils.toObject = function (clrObject) {
        var jsonText = $Native.ToJSON(clrObject);
        return JSON.parse(jsonText);
    };
    //导入一个 CLR 类型
    owner.$Import = function (path) {
        var name = $Native.Replace(path, '.', '_');
        $Context.SetParameterWithType(name, path);
        return eval(name);
    };
}(this));