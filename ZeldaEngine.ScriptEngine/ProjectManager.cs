using System.Reflection;
using ZeldaEngine.Base.Abstracts;
using ZeldaEngine.Base.Abstracts.ScriptEngine;
using ZeldaEngine.Base.Abstracts.ScriptEngine.Project;
using ZeldaEngine.Base.ValueObjects;

namespace ZeldaEngine.ScriptEngine
{
    public class ProjectManager : IProjectManager
    {
        private readonly IScriptEngine _scriptEngine;

        public ProjectManager(IScriptEngine scriptEngine)
        {
            _scriptEngine = scriptEngine;
        }

        public void CreateProject(string projectName)
        {            
        }

        public void UpdateProject(string projectName)
        {
        }


        public Assembly[] GetProjectAssemblies(string projName)
        {
            return null;
        }
    }
}