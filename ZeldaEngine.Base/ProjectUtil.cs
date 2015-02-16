using System.Collections.Generic;
using System.Reflection;
using ZeldaEngine.Base.Abstracts;
using ZeldaEngine.Base.Abstracts.ScriptEngine;
using ZeldaEngine.Base.ValueObjects;

namespace ZeldaEngine.Base
{
    public static class ProjectUtil
    {
        private static IScriptEngine _scriptEngine;

        public static bool CreateProject(IScriptEngine engine)
        {
            _scriptEngine = engine;
            if (!engine.Config.GameScriptConfig.GenerateProject)
                return false;

            return true;
        }

        public static void UpdateProject(IScriptEngine engine)
        {
            _scriptEngine = engine;
        }


        public static IEnumerable<Assembly> GetProjectAssemblies(string projName)
        {
            return null;
        }

        public static bool GetProject(string projectName)
        {
            return false;
        }

        public static void DeleteProject(string projectName)
        {
        }
    }
}