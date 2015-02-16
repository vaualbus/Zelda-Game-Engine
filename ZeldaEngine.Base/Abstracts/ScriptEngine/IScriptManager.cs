using System.Reflection;
using ZeldaEngine.Base.Abstracts.Game;
using ZeldaEngine.Base.ValueObjects.ScriptEngine;

namespace ZeldaEngine.Base.Abstracts.ScriptEngine
{
    public interface IScriptManager
    {
        GameScript CurrentMenagedScript { get; }

        RuntimeScript RuntimeScript { get; }

        GameScript AddScript(IGameView gameView, CompiledScript script, string scriptName);

        object ExcuteFunction(string funcName, object[] @params);

        MethodInfo GetMethod(string funcName, object[] @params);

        object GetScriptValue(string propName);

        void SetScriptValue(string propName, object value);

        void SetScriptFields(string fieldName, object scriptEngine);

        TValue GetScriptValue<TValue>(string propName);

        TReturn ExcuteFunction<TReturn>(string funcName, object[] @params);

        bool CreateScript(RuntimeScript runtimeScript); 
    }
}