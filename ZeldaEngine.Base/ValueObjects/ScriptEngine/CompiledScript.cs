using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using ZeldaEngine.Base.Abstracts.Game;
using ZeldaEngine.Base.Abstracts.ScriptEngine;

namespace ZeldaEngine.Base.ValueObjects.ScriptEngine
{
    public class CompiledScript : IDisposable
    {
        public Assembly CompiledAssembly { get; private set; }

        public Type ScriptType { get; private set; }

        public IEnumerable<MethodInfo> Methods { get; private set; }

        public IEnumerable<PropertyInfo> Properties { get; private set; }

        public IEnumerable<FieldInfo> Fields { get; private set; }

        public IEnumerable<Type> Classes { get; private set; }

        public IEnumerable<MemberInfo> Members { get; private set; }

        public string CompiledPath { get; private set; }

        public CompiledScript(Assembly compiledAssembly, string compilePath)
        {

            CompiledAssembly = compiledAssembly;
            Classes = CompiledAssembly.GetTypes().ToList();

            ScriptType = Classes.FirstOrDefault(t => typeof(GameScript).IsAssignableFrom(t));

            Methods = Classes.Select(t => t.GetMethods())
                                                   .FirstOrDefault()
                                                   .ToList();

            Properties = Classes.Select(t => t.GetProperties(BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public))
                                                  .FirstOrDefault()
                                                  .ToList();

            Fields = Classes.Select(t => t.GetFields(BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public))
                                                  .FirstOrDefault()
                                                  .ToList();

            Members = Classes.Select(t => t.GetMembers(BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public))
                                                  .FirstOrDefault()
                                                  .ToList();

            CompiledPath = compilePath;
        }

        public void Dispose()
        {
        }
    }
}
