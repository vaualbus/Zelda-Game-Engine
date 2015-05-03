using System.Collections.Generic;
using Newtonsoft.Json;
using ZeldaEngine.Base.Game.MapLoaders;

namespace ZeldaEngine.Base.Game.ValueObjects.MapLoaderDataTypes
{
    public class GameObjectDefinition
    {
        public string Name { get; private set; }

        [JsonConverter(typeof(JsonMapLoader.CustomDeserializer))]
        public Dictionary<string, object> Properties { get; private set; }

        public GameObjectDefinition(string name, Dictionary<string, object> properties = null)
        {
            Name = name;
            Properties = properties ?? new Dictionary<string, object>();
        }
    }
}