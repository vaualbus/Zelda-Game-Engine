using System;
using System.Collections.Generic;
using ZeldaEngine.Base.Abstracts.Game;
using ZeldaEngine.Base.Abstracts.ScriptEngine;

namespace ZeldaEngine.Base.ValueObjects.ScriptEngine
{
    public class RuntimeScript : IEquatable<RuntimeScript>
    {
        private readonly IScriptRepository _scriptRepository;

        public IGameView GameView { get; private set; }

        public string Name { get; private set; }

        public Dictionary<Type, List<object>> CtorParams { get; private set; }

        public CompiledScript CompiledScript { get; private set; }

        public RuntimeScript(IGameView gameView, IScriptCompiler compiler, IScriptRepository scriptRepository, string name, string scriptFileName)
        {
            Name = name;
            CtorParams = new Dictionary<Type, List<object>>();
            CompiledScript = compiler.Compile(scriptFileName);
            GameView = gameView;
            _scriptRepository = scriptRepository;
        }

        internal RuntimeScript(IScriptRepository scriptRepository, IGameView gameView, CompiledScript script, string name)
        {
            Name = name;
            CompiledScript = script;
            GameView = gameView;
            CtorParams = new Dictionary<Type, List<object>>();

            _scriptRepository = scriptRepository;
        }

        public bool Equals(RuntimeScript other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return Equals(CtorParams, other.CtorParams) && Equals(CompiledScript, other.CompiledScript);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((RuntimeScript) obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return ((CtorParams != null ? CtorParams.GetHashCode() : 0)*397) ^ (CompiledScript != null ? CompiledScript.GetHashCode() : 0);
            }
        }

        public static bool operator ==(RuntimeScript left, RuntimeScript right)
        {
            return Equals(left, right);
        }

        public static bool operator !=(RuntimeScript left, RuntimeScript right)
        {
            return !Equals(left, right);
        }

        public GameScript Compile()
        {
            return _scriptRepository.Compile(this);
        }
    }
}