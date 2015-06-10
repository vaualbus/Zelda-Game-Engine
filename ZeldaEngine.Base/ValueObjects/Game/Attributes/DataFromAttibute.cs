using System;

namespace ZeldaEngine.Base.ValueObjects.Game.Attributes
{
    [AttributeUsage(AttributeTargets.All)]
    public class DataFromAttribute : Attribute
    {
        public string ScriptName { get; private set; }

        public string FieldName { get; private set; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="pattern">ScriptName.ValueName</param>
        public DataFromAttribute(string pattern)
        {
            var @params = pattern.Split('.');
            ScriptName = @params[0];
            FieldName = @params[1];
        }
    }
}