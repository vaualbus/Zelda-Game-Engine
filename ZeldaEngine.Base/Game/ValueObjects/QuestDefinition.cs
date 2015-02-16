using System.Collections.Generic;
using ZeldaEngine.Base.Abstracts.Game;

namespace ZeldaEngine.Base.Game.ValueObjects
{
    public class QuestDefinition
    {
        public string Name { get; private set; }

        public string Version { get; private set; }

        public string MinimumEngineVersion { get; private set; }

        public string AuthorName { get; private set; }

        public IEnumerable<ItemDefinition> Items { get; private set; } 

        public IEnumerable<MapDefinition> Maps { get; private set; }

        public IEnumerable<EnemyDefinition> Enemies { get; private set; }

        public IEnumerable<GameObjectDefinition> GameObjects { get; private set; }

        public Dictionary<string, string> Scripts { get; private set; }  

        public QuestDefinition(string name, string version, string minimumEngineVersion, 
                               string authorName, IEnumerable<ItemDefinition> items, 
                               IEnumerable<MapDefinition> maps, IEnumerable<EnemyDefinition> enemies,
                               IEnumerable<GameObjectDefinition> gameObjects, Dictionary<string, string> scripts)
        {
            Name = name;
            Version = version;
            MinimumEngineVersion = minimumEngineVersion;
            AuthorName = authorName;
            Items = items;
            Maps = maps;
            Enemies = enemies;
            GameObjects = gameObjects;
            Scripts = scripts;
        }
    }
}