using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using Microsoft.CSharp;
using ZeldaEngine.Base.Abstracts;
using ZeldaEngine.Base.Abstracts.Game;
using ZeldaEngine.Base.Abstracts.ScriptEngine;
using ZeldaEngine.Base.ValueObjects;
using ZeldaEngine.Base.ValueObjects.ScriptEngine;

namespace ZeldaEngine.Base
{
    public class CSharpScriptCompiler : IScriptCompiler
    {
        private readonly IScriptEngine _scriptEngine;
        private readonly ILogger _logger;

        public StringBuilder AdditionalCodeToCompile { get; private set; }
        public List<Assembly> AdditionalAssemblies { get; set; }

        public CSharpScriptCompiler(IScriptEngine engine, ILogger logger)
        {
            _scriptEngine = engine;
            _logger = logger;

            AdditionalAssemblies = new List<Assembly>();
            AdditionalCodeToCompile = new StringBuilder();

            _logger.LogInfo("Init script Compiler");
        }

        public CompiledScript Compile(string fileName)
        {
            //First wee need to find the file.
            if ((new FileInfo(fileName).Extension != ".cs"))
                fileName += ".cs";

            //Now we need the file to be in t e ScriptFolder or (later, much later, in memory).

            var filePath = "";
            filePath = Path.Combine($"{_scriptEngine.GameEngine.GameConfig.ScriptDirectory}", fileName);
            if (!File.Exists(filePath))
                throw new ArgumentException(string.Format("Cannot find the file: {0} in the path: {1}", fileName, $"{_scriptEngine.GameEngine.GameConfig.BaseDirectory}{_scriptEngine.GameEngine.GameConfig.ScriptDirectory}"));


            var compilerParams = new CompilerParameters
            {
                GenerateInMemory = true,
                GenerateExecutable = false,
            };

            compilerParams.ReferencedAssemblies.Add("System.dll");
            compilerParams.ReferencedAssemblies.Add("System.Data.dll");
            compilerParams.ReferencedAssemblies.Add("System.Xml.dll");
            compilerParams.ReferencedAssemblies.Add("System.Linq.dll");
            compilerParams.ReferencedAssemblies.Add("System.Drawing.dll");
            compilerParams.ReferencedAssemblies.Add("mscorlib.dll");

            compilerParams.ReferencedAssemblies.AddRange(AdditionalAssemblies.Select(t => t.Location).ToArray());

            using(var codeProvider = new CSharpCodeProvider())
            {
                var result = codeProvider.CompileAssemblyFromFile(compilerParams, filePath);

                if (result.Errors.Count > 0)
                {
                    var errors = new List<CompilerError>();
                    for (var i = 0; i < result.Errors.Count; i++)
                        errors.Add(result.Errors[i]);

                    _logger.LogError(string.Format("An error has occured during the script compilation. Info: {0}", string.Join("\n", errors)));
                    return null;
                }

                return new CompiledScript(result.CompiledAssembly, filePath);
            }
        }
    }
}
