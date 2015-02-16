using System.Collections.Generic;
using System.Linq;
using ZeldaEngine.Base.Abstracts.Game;
using ZeldaEngine.Base.Game.GameObjects;
using ZeldaEngine.Base.Game.ValueObjects;

namespace ZeldaEngine.Base.Game
{
    public class Quest
    {
        private readonly IGameEngine _gameEngine;
        public string Name { get; private set; }

        public string Version { get; private set; }

        public string MinimumEngineVersion { get; private set; }

        public string AuthorName { get; private set; }

        public IEnumerable<ItemGameObject> Items { get; private set; }

        public IEnumerable<Map> Maps { get; private set; }

        public IEnumerable<EnemyGameObject> Enemies { get; private set; }

        public IEnumerable<DrawableGameObject> GameObjects { get; private set; } 

        public Quest(IGameEngine gameEngine)
        {
            _gameEngine = gameEngine;
        }

        public Quest Create(QuestDefinition questDefinition)
        {
            Name = questDefinition.Name;
            Version = questDefinition.Version;
            MinimumEngineVersion = questDefinition.MinimumEngineVersion;
            AuthorName = questDefinition.AuthorName;

            GameObjects = questDefinition.GameObjects.Select(t => new DrawableGameObject(_gameEngine).Create(t)).ToList();

            Enemies = questDefinition.Enemies.Select(t => new EnemyGameObject(_gameEngine).Create(t)).ToList();
            Items = questDefinition.Items.Select(t => new ItemGameObject(_gameEngine).Create(t)).ToList();
            Maps = questDefinition.Maps.Select(t => new Map(_gameEngine).CreateMap(t, questDefinition.Scripts)).ToList();

            return new Quest(_gameEngine)
            {
                Name = questDefinition.Name,
                Version = questDefinition.Version,
                MinimumEngineVersion = questDefinition.MinimumEngineVersion,
                AuthorName = questDefinition.AuthorName,
                Maps =  Maps,
                Enemies =  Enemies,
                Items =  Items,
                GameObjects = GameObjects
            };
        }
    }
}