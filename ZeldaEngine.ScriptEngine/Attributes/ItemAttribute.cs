using System;

namespace ZeldaEngine.ScriptEngine.Attributes
{
    [AttributeUsage(AttributeTargets.Class)]
    public class ItemAttribute : Attribute
    {
        public uint ItemId { get; private set; }

        public ItemAttribute(uint itemId)
        {
            ItemId = itemId;
        }
    }
}
