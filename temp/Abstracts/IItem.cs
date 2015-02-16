using System.Runtime.InteropServices;

namespace ZeldaEngine.ScriptEngine.Abstracts
{
    [Guid("E10DDB8C-DB51-4A7E-A60E-62EB70CC1D34")]
    public interface IItem
    {
        string Name { get; set; }
    }
}
