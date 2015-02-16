using System.Runtime.InteropServices;

namespace ZeldaEngine.ScriptEngine.Abstracts
{
    [Guid("DBD0029C-3AFC-4792-B8AC-B17BA23DAB6E")]
    public interface INpc
    {
        string Name { get; set; }
    }
}
