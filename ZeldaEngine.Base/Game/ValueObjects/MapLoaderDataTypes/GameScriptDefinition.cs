using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using ZeldaEngine.Base.Game.MapLoaders;
using ZeldaEngine.Base.ValueObjects.ScriptEngine;

namespace ZeldaEngine.Base.Game.ValueObjects.MapLoaderDataTypes
{
    public class QuestLoaderScriptType : IEquatable<QuestLoaderScriptType>
    {
        public string GameObjectName { get; private set; }

        public ScriptType ScriptType { get; private set; }

        public GameScriptDefinition Script { get; private set; }

        public QuestLoaderScriptType(string gameObjectName, ScriptType scriptType, GameScriptDefinition script)
        {
            GameObjectName = gameObjectName;
            Script = script;
            ScriptType = scriptType;
        }

        public bool Equals(QuestLoaderScriptType other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return string.Equals(GameObjectName, other.GameObjectName) && Equals(Script, other.Script);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((QuestLoaderScriptType) obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return ((GameObjectName != null ? GameObjectName.GetHashCode() : 0)*397) ^ (Script != null ? Script.GetHashCode() : 0);
            }
        }

        public static bool operator ==(QuestLoaderScriptType left, QuestLoaderScriptType right)
        {
            return Equals(left, right);
        }

        public static bool operator !=(QuestLoaderScriptType left, QuestLoaderScriptType right)
        {
            return !Equals(left, right);
        }
    }

    public class GameScriptDefinition
    {
        public string Name { get; private set; }

        [JsonConverter(typeof(JsonMapLoader.CustomDeserializer))]
        public Dictionary<string, object> ScriptParams { get; private set; }

        public GameScriptDefinition(string name, Dictionary<string, object> scriptParams = null)
        {
            Name = name;
            ScriptParams = scriptParams ?? new Dictionary<string, object>();
        }
    }
}