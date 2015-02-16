using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using ZeldaEngine.Base.ValueObjects.ScriptEngine;

namespace ZeldaEngine.Base.Game.Extensions
{
    public static class ScriptMenagerExtensions
    {

        public static RuntimeScript AddCtorParam<TInterface>(this RuntimeScript runtimeScript, TInterface @object)
        {
            runtimeScript.CtorParams.Add(typeof (TInterface), new List<object> { @object });
            return runtimeScript;
        }

        public static RuntimeScript AddCtorParam<TInterfaceType, TConcreateClass>(this RuntimeScript runtimeScript, params object[] @ctorValues)
        {
            var concreateClass = Activator.CreateInstance(typeof (TConcreateClass), @ctorValues);
            runtimeScript.CtorParams.Add(typeof(TInterfaceType), new List<object> { concreateClass });
            return runtimeScript;
        }

        public static RuntimeScript AddField<TType>(this RuntimeScript runtimeScript, string fieldName, Expression<Func<object,TType>> action)
        {
            var type = action.Body.Type;

            object fieldValue = null;
            var hasParser = type.GetMethod("Parse", new[] { typeof(string) });
            if (hasParser != null)
                fieldValue = hasParser.Invoke(null, new object[] { action.Body.ToString() });

           return runtimeScript;
        }

        public static ScriptManager AddRuntimeField<TType>(this ScriptManager scriptManager, string scriptName,
            string fieldName, Expression<Func<object, TType>> field)
        {
            var script = scriptManager.CurrentMenagedScript;
            return scriptManager;
        }

        public static ScriptManager AddRuntimeFunction<TType>(this ScriptManager scriptManager, string scriptName,
            string funcName, Expression<Func<object, TType>> func)
        {
            var script = scriptManager.CurrentMenagedScript;
            return scriptManager;
        }
    }
}