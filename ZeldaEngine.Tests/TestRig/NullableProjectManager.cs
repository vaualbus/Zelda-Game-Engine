using System.Reflection;
using ZeldaEngine.Base.Abstracts.ScriptEngine;
using ZeldaEngine.Base.Abstracts.ScriptEngine.Project;

namespace ZeldaEngine.Tests.TestRig
{
    public class NullableProjectManager : IProjectManager
    {
        public NullableProjectManager(IScriptEngine scriptEngine)
        {
        }

        public void CreateProject(string projectName)
        {
            throw new System.NotImplementedException();
        }

        public void UpdateProject(string projectName)
        {
            throw new System.NotImplementedException();
        }

        public Assembly[] GetProjectAssemblies(string projName)
        {
            throw new System.NotImplementedException();
        }

        public void CopyEngineFileIfNecessary()
        {
            throw new System.NotImplementedException();
        }
    }
}