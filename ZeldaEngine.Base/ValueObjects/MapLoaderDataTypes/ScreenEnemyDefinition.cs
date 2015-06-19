using System.Collections.Generic;

namespace ZeldaEngine.Base.ValueObjects.MapLoaderDataTypes
{
    public class ScreenEnemyDefinition
    {
        public string Name { get; private set; }

        public EnemySpawnLocation SpawnLocation { get; private set; }

        public int Count { get; private set; }

        public Dictionary<string, object> Properties { get; private set; }

        public ScreenEnemyDefinition(string name, EnemySpawnLocation spawnLocation, int count, Dictionary<string, object> properties)
        {
            Name = name;
            SpawnLocation = spawnLocation;
            Count = count;
            Properties = properties ?? new Dictionary<string, object>();
        }
    }
}