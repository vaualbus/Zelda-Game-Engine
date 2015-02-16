using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using ZeldaEngine.Base.Abstracts.Game;
using ZeldaEngine.Base.Game.Abstracts;
using ZeldaEngine.Base.Game.GameEngineClasses;
using ZeldaEngine.Base.Game.ValueObjects;


namespace ZeldaEngine.Base.Game
{
    public class QuestManager : BaseGameClass
    {
        private readonly IGameEngine _gameEngine;
        private readonly IQuestLoader _questLoader;

        public Quest LoadedQuest { get; private set; } 

        public QuestManager(IGameEngine gameEngine, IQuestLoader mapLoader)
            : base(gameEngine)
        {
            _gameEngine = gameEngine;
            _questLoader = mapLoader;
        }

        public Quest LoadQuest(string questName)
        {
            _gameEngine.Logger.LogDebug("Loading quest: {0}", questName);

            var questPath = Path.Combine(GameEngine.Configuration.GameConfig.BaseDirectory, GameEngine.Configuration.GameConfig.QuestDirectory);
            var file = Directory.GetFiles(questPath).FirstOrDefault(t => t.EndsWith(string.Format("{0}.{1}", questName, ConfigurationManager.QuestFileExtension)));
            LoadedQuest = new Quest(_gameEngine).Create(_questLoader.Load(file));
            return LoadedQuest;
        }

        public void Draw(IRenderEngine renderEngine)
        {
            foreach (var map in LoadedQuest.Maps)
                map.Draw(renderEngine);
        }

        public void Update(float dt)
        {
            foreach (var map in LoadedQuest.Maps)
                map.Update(dt);
        }

        public override void Dispose()
        {
            LoadedQuest = null;
            GameEngine.Logger.LogInfo("Disposing the Map Loader");
        }
    }
}