using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using Microsoft.CSharp;
using ZeldaEngine.Base.Abstracts;
using ZeldaEngine.Base.ValueObjects;
using ZeldaEngine.ScriptEngine.Abstracts;


namespace ZeldaEngine.ScriptEngine
{
    public class CSharpScriptCompiler : IScriptCompiler
    {
        private readonly string _projectFolder;
        private readonly string _projectName;
        private readonly float _cSharpVersion;

        private readonly string[] _addtionalAssenblies;

        private readonly IScriptParamaterProvider _paramaterProvider;

        private readonly IScriptEngine _scriptEngine;

        public CompiledScript CompiledScript { get; private set; }

        public CSharpScriptCompiler(IScriptEngine engine, string projectFolder = "", string projectName = "", float cSharpVersion = 3.5f, string[] additionalyAssenblies = null)
        {
            engine.Report("Init script Compiler", ReportReason.Log);

            _scriptEngine = engine;
            _projectFolder = projectFolder;
            _projectName = projectName;
            _cSharpVersion = cSharpVersion;
            _paramaterProvider = _scriptEngine.ParamsProvider;
            _addtionalAssenblies = additionalyAssenblies;
        }

        public CompiledScript Compile(string fileName, bool isDebug)
        {
            //Fun!

            //First wee need to find the file.
            var filePath = Path.Combine(Environment.CurrentDirectory, fileName);
            if (!File.Exists(filePath))
                throw new ArgumentException(string.Format("Cannot find the file: {0} in the path: {1}", fileName, Environment.CurrentDirectory));

            var compilerParams = new CompilerParameters
            {
                GenerateInMemory = true,
                GenerateExecutable = false,
                
            };

            compilerParams.ReferencedAssemblies.Add("System.dll");
            compilerParams.ReferencedAssemblies.Add("System.Data.dll");
            compilerParams.ReferencedAssemblies.Add("System.Xml.dll");
            compilerParams.ReferencedAssemblies.Add("System.Linq.dll");
            compilerParams.ReferencedAssemblies.Add("mscorlib.dll");
            compilerParams.ReferencedAssemblies.Add("ZeldaEngine.ScriptEngine.dll");

            compilerParams.ReferencedAssemblies.AddRange(_addtionalAssenblies ?? new string[] { });

            using(var codeProvider = new CSharpCodeProvider())
            {
                var result = codeProvider.CompileAssemblyFromFile(compilerParams, filePath);

                if (result.Errors.Count > 0)
                {
                    var errors = new List<CompilerError>();
                    for (var i = 0; i < result.Errors.Count; i++)
                        errors.Add(result.Errors[i]);

                    _scriptEngine.Report(string.Format("An error has occured during the script compilation. Info: {0}", string.Join("\n", errors)), ReportReason.Error);
                    return null;
                }

                CompiledScript = new CompiledScript(result.CompiledAssembly, filePath);

                if(isDebug)
                    TestScriptFunctionality();

                return CompiledScript;
            }
        }


        private void TestScriptFunctionality()
        {
            var scriptClass = CompiledScript.ScriptType;

            var @class = Activator.CreateInstance(scriptClass) as ZeldaScript;

            //Test porpuses
            var methodParams = _paramaterProvider.GetParamatersForScript(scriptClass.Name).ToArray();
            var runMethod = scriptClass.GetMethods().FirstOrDefault(t => t.Name == "Run" && t.GetParameters().Length == methodParams.Length);

            if(runMethod == null)
                return;

            runMethod.Invoke(@class, methodParams);
        }

        public CompiledScript Compile()
        {
            //Double Fun We need to load the solution and build it!
            return null;
        }

        public void Dispose()
        {
            CompiledScript.Dispose();
        }
    }
}
