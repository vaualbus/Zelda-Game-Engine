using System.Reflection;

namespace ZeldaEngine.Base.Abstracts.ScriptEngine.Project
{
    public interface IProjectManager
    {
        void CreateProject(string projectName);

        void UpdateProject(string projectName);


        Assembly[] GetProjectAssemblies(string projName);
    }
}