using System;

namespace ZeldaEngine.ScriptEngine.Attributes
{
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property)]
    public class ModifiableByAttribute : Attribute
    {
        public string ModClassPatternMatch { get; set; }

        public ModifiableByAttribute(string modClassPatternMatch)
        {
            ModClassPatternMatch = modClassPatternMatch;
        }
    }
}