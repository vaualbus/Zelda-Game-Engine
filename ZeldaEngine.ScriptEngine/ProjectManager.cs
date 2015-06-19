using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using ZeldaEngine.Base.Abstracts;
using ZeldaEngine.Base.Abstracts.ScriptEngine;
using ZeldaEngine.Base.Abstracts.ScriptEngine.Project;
using ZeldaEngine.Base.Game.GameEngineClasses;
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

        public void CopyEngineFileIfNecessary()
        {
            //If the project path is set, than copy the game engine dll in it
            ///TODO(alberto): Better do when we create the project?
            if (_scriptEngine.GameEngine.GameConfig.ProjectFolder != string.Empty && !File.Exists(".test"))
            {
                var applicationFiles = new List<string>()
                {
                    "ZeldaEngine.Base.dll",
                    "ZeldaEngine.ScriptEngine.dll",
                    "ZeldaEngine.Base.pdb",
                    "ZeldaEngine.ScriptEngine.pdb"
                };

                var directoryFiles = Directory.GetFiles(_scriptEngine.GameEngine.GameConfig.BaseDirectory).Select(t => new FileInfo(t).Name);
                var engineFiles = directoryFiles.Intersect(applicationFiles);

                foreach (var projectFile in engineFiles.Where(t => t != null))
                {
                    if (File.Exists(Path.Combine(_scriptEngine.GameEngine.GameConfig.ProjectFolder, projectFile)) &&
                        new FileInfo(Path.Combine(_scriptEngine.GameEngine.GameConfig.ProjectFolder, projectFile)).Extension == ".dll")
                    {
                        //We have a dll file, so now we will load and check if the new file is newer.
                        //If so we will copy it.
                        var currentFile = projectFile;
                        var baseFile = directoryFiles.FirstOrDefault(t => t == currentFile);

                        var engineFileAssembly = Assembly.LoadFrom(Path.Combine(_scriptEngine.GameEngine.GameConfig.BaseDirectory, baseFile));
                        var projectFileAssembly = Assembly.LoadFrom(Path.Combine(_scriptEngine.GameEngine.GameConfig.ProjectFolder, currentFile));

                        if (FileVersionInfo.GetVersionInfo(engineFileAssembly.Location).FileVersion ==
                            FileVersionInfo.GetVersionInfo(projectFileAssembly.Location).FileVersion)
                            continue;
                    }

                    if (File.Exists(Path.Combine(_scriptEngine.GameEngine.GameConfig.ProjectFolder, projectFile)))
                        continue;

                    if (!Directory.Exists(_scriptEngine.GameEngine.GameConfig.ProjectFolder))
                        Directory.CreateDirectory(_scriptEngine.GameEngine.GameConfig.ProjectFolder);

                    File.Copy(projectFile, Path.Combine(_scriptEngine.GameEngine.GameConfig.ProjectFolder, projectFile));
                }
            }
        }
    }
}