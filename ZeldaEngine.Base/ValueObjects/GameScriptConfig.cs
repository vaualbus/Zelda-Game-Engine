namespace ZeldaEngine.Base.ValueObjects
{
    public class GameScriptConfig
    {
        public string EnginePath { get; private set; }

        public string ProjectFolder { get; private set; }

        public string ProjectName { get; private set; }

        public bool GenerateProject { get; private set; }


        public GameScriptConfig(string enginePath, string projectPath, string projectName = "", bool generateProject = false)
        {
            EnginePath = enginePath;
            ProjectFolder = projectPath;
            ProjectName = projectName;
            GenerateProject = generateProject;
        }
    }
}