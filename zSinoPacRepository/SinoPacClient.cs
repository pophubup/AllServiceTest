using Microsoft.Extensions.DependencyInjection;
using System;

namespace zSinoPacRepository
{
    public static class SinoPacClient
    {
        public static IServiceCollection AddPyPackageService(this IServiceCollection service)
        {
            var engine = IronPython.Hosting.Python.CreateEngine();
            var scope = engine.CreateScope();
            var source = engine.CreateScriptSourceFromFile(@"C:\Users\Yohoo\OneDrive\桌面\pyfiles\hello.py");
            source.Execute(scope);
            

            dynamic Calculator = scope.GetVariable("Calculator");
            dynamic calc = Calculator();
            int result = calc.add(4, 5);
            var ddd = calc.doSelf();

            return service;
        }
    }
}
