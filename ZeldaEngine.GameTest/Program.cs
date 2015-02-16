using System;
using ZeldaEngine.GameTest.Scripts;
using ZeldaEngine.ScriptEngine.Abstracts;

namespace ZeldaEngine.GameTest
{
    public class Program
    {

        private static void Main()
        {

            var scriptEngine = new GameScriptEngine();

            scriptEngine.ScriptManager.AddScript(scriptEngine.ScriptCompiler.Compile(@"Scripts\TestScript.cs"), "Test");

            var propValue = scriptEngine.ScriptManager.GetScriptValue<float>("Test", "TestProp");

            Console.WriteLine(propValue);

            var method = scriptEngine.ScriptManager.ExcuteFunction("Test", "TestFunc", new object[] {1.0f, 'A', 34f});

            Console.ReadKey();
        }

        private static void Test1(IScriptEngine engine)
        {
            try
            {
                ChangeParamaters(engine, new object[] { 10 });
                ChangeParamaters(engine, new object[] { 2, "Test" });
            }
            catch (Exception ex)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Error: {0} in {1}", ex.Message, ex.StackTrace);
                Console.ForegroundColor = ConsoleColor.Black;
                Console.ReadKey();
            }
        }

        private static void ChangeParamaters(IScriptEngine scriptEngine, object[] p)
        {
            Console.WriteLine("Changing paraters....");
            scriptEngine.ParamsProvider.ChangeParamaters<TestScript>(p);
            scriptEngine.ScriptCompiler.Compile(@"Scripts\TestScript.cs", true);
            Console.ReadKey();
        }
    }
}
