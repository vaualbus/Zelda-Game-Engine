using System;
using System.Collections.Generic;
using ZeldaEngine.Base.ValueObjects.Game;

namespace ZeldaEngine.Base.Game.ValueObjects
{
    public class TileDefinition : IEquatable<TileDefinition>
    {
        public int TilePositionX { get; private set; }

        public int TilePositionY { get; private set; }

        public TileType TileType { get; private set; }

        public string TileColor { get; private set; }

        public int LayerNumber { get; private set; }

        public string TextureAssetName { get; private set; }

        public GameObjectDefinition GameObject { get; private set; }
 
        public GameScriptDefinition GameScript { get; private set; }

        public TileDefinition(int tilePositionX, int tilePositionY, string textureAssetName, 
                               TileType tileType, string tileColor = "COLOR_DEFAULT", int layerNumber = 0,
                               GameObjectDefinition gameObject = null, GameScriptDefinition gameScript = null)
        {
            TilePositionX = tilePositionX;
            TilePositionY = tilePositionY;
            TileType = tileType;
            LayerNumber = layerNumber;
            TextureAssetName = textureAssetName;
            GameObject = gameObject;
            GameScript = gameScript;
            TileColor = tileColor;
        }

        public bool Equals(TileDefinition other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return TilePositionX == other.TilePositionX && TilePositionY == other.TilePositionY &&
                   TileType == other.TileType && string.Equals(TileColor, other.TileColor) &&
                   LayerNumber == other.LayerNumber && string.Equals(TextureAssetName, other.TextureAssetName) &&
                   Equals(GameObject, other.GameObject) && Equals(GameScript, other.GameScript);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((TileDefinition) obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                var hashCode = TilePositionX;
                hashCode = (hashCode*397) ^ TilePositionY;
                hashCode = (hashCode*397) ^ (int) TileType;
                hashCode = (hashCode*397) ^ (TileColor != null ? TileColor.GetHashCode() : 0);
                hashCode = (hashCode*397) ^ LayerNumber;
                hashCode = (hashCode*397) ^ (TextureAssetName != null ? TextureAssetName.GetHashCode() : 0);
                hashCode = (hashCode*397) ^ (GameObject != null ? GameObject.GetHashCode() : 0);
                hashCode = (hashCode*397) ^ (GameScript != null ? GameScript.GetHashCode() : 0);
                return hashCode;
            }
        }

        public static bool operator ==(TileDefinition left, TileDefinition right)
        {
            return Equals(left, right);
        }

        public static bool operator !=(TileDefinition left, TileDefinition right)
        {
            return !Equals(left, right);
        }
    }
}