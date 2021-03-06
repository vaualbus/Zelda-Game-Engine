﻿using System.Reflection;
using ZeldaEngine.Base.ValueObjects;

namespace ZeldaEngine.Base.Abstracts
{
    public interface IScriptManager
    {
        bool AddScript(CompiledScript script, string scriptName);

        void RemoveScript(string name);

        CompiledScript Find(string name);
           
        object ExcuteFunction(string scriptName, string funcName, object[] @params);

        MethodInfo GetMethod(string scriptName, string funcName, object[] @params);

        object GetScriptValue(string scriptName, string propName);

        void SetScriptValue(string scriptName, string propName, object value);

        void SetScriptFields(string scriptName, string fieldName, object scriptEngine);

        TValue GetScriptValue<TValue>(string scriptName, string propName);

        TReturn ExcuteFunction<TReturn>(string scriptName, string funcName, object[] @params);
    }
}
