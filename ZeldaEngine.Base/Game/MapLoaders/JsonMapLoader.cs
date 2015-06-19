using System;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;
using ZeldaEngine.Base.Abstracts.Game;
using ZeldaEngine.Base.ValueObjects.MapLoaderDataTypes;

namespace ZeldaEngine.Base.Game.MapLoaders
{
    public class JsonMapLoader : IQuestLoader
    {
        internal class CustomDeserializer : JsonConverter
        {
            public override bool CanWrite
            {
                get { return false; }
            }

            public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
            {
            }

            public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
            {
                var result = new Dictionary<string, object>();
                reader.Read();

                while (reader.TokenType == JsonToken.PropertyName)
                {
                    var propName = reader.Value as string;
                    reader.Read();

                    object value;
                    if (reader.TokenType == JsonToken.Integer)
                        value = Convert.ToInt32(reader.Value);
                    else
                        value = serializer.Deserialize(reader);

                    result.Add(propName, value);
                    reader.Read();
                }

                return result;
            }

            public override bool CanConvert(Type objectType)
            {
                return objectType == typeof(Dictionary<string, object>);
            }
        }

        private readonly IGameEngine _gameEngine;

        public JsonMapLoader(IGameEngine gameEngine)
        {
            _gameEngine = gameEngine;
        }

        public QuestDefinition Load(string fileName)
        {
            using (var sr = new StreamReader(fileName))
                return JsonConvert.DeserializeObject<QuestDefinition>(sr.ReadToEnd());
        }
    }
}