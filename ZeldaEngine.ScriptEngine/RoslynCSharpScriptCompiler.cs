using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using ZeldaEngine.Base.Abstracts.Game;
using ZeldaEngine.Base.Abstracts.ScriptEngine;
using ZeldaEngine.Base.ValueObjects.ScriptEngine;

namespace ZeldaEngine.ScriptEngine
{
    public class RoslynCSharpScriptCompiler : IScriptCompiler
    {
        private IScriptEngine _scriptEngine;
        private ILogger _logger;

        public StringBuilder AdditionalCodeToCompile { get; private set; }

        public List<Assembly> AdditionalAssemblies { get; set; }

        public RoslynCSharpScriptCompiler(IScriptEngine engine, ILogger logger)
        {
            _scriptEngine = engine;
            _logger = logger;

            AdditionalAssemblies = new List<Assembly>();
            AdditionalCodeToCompile = new StringBuilder();

            _logger.LogInfo("Init script Compiler");
        }

        public CompiledScript Compile(string fileName)
        {
            if ((new FileInfo(fileName).Extension != ".cs"))
                fileName += ".cs";

            //Now we need the file to be in t e ScriptFolder or (later, much later, in memory).

            var filePath = "";
            filePath = Path.Combine($"{_scriptEngine.GameEngine.GameConfig.ScriptDirectory}", fileName);
            if (!File.Exists(filePath))
                throw new ArgumentException(string.Format("Cannot find the file: {0} in the path: {1}", fileName, $"{_scriptEngine.GameEngine.GameConfig.BaseDirectory}{_scriptEngine.GameEngine.GameConfig.ScriptDirectory}"));

            var fileText = File.ReadAllText(filePath);
            var codeTree = SyntaxFactory.ParseSyntaxTree(fileText);

            var compilerOption = new CSharpCompilationOptions(OutputKind.DynamicallyLinkedLibrary);

            var referenceAssemblies = AdditionalAssemblies.Where(t => t != null)
                                                          .Select(MetadataReference.CreateFromAssembly);
        
            AdditionalAssemblies.Add(typeof(object).Assembly);

            var compilation = CSharpCompilation.Create("ScriptCompiled")
                .WithOptions(compilerOption)
                .AddSyntaxTrees(codeTree)
                .AddReferences(referenceAssemblies);

            var compilationErrors = compilation.GetDiagnostics();

            //foreach (var error in compilationErrors.Where(t => t.DefaultSeverity.ToString() != "Hidden"))
            //    _logger.LogError(string.Format("An error has occured during the script compilation. Info: {0}", error));

            using (var ms = new MemoryStream())
            {
                compilation.Emit(ms);
                return new CompiledScript(Assembly.Load(ms.GetBuffer()), filePath);
            }
        }
    }
}