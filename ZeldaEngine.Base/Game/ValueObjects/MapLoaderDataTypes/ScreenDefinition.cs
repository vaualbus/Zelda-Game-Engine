using System;
using System.Collections.Generic;
using ZeldaEngine.Base.ValueObjects.Game;

namespace ZeldaEngine.Base.Game.ValueObjects.MapLoaderDataTypes
{
    public class ScreenDefinition : IEquatable<ScreenDefinition>
    {
        public int Id { get; private set; }

        public string ScreenName { get; private set; }

        public MapType ScreenType { get; private set; }

        public int ScreenPositionX { get; set; }

        public int ScreenPositionY { get; set; }

        public string SongName { get; private set; }

        public int ColorPalette { get; private set; }

        public string IntroText { get; private set; }

        public IEnumerable<TileDefinition> Tiles { get; private set; }

        public int PlayerStartPositionX { get; private set; }

        public IEnumerable<ScreenEnemyDefinition> Enemies { get; private set; }

        public IEnumerable<QuestLoaderScriptType> Scripts { get; private set; }

        public ScreenDefinition(int id, string screenName, MapType screenType,
                                int playerStartPositionX, int playerStartPositionY, string songName, 
                                int colorPalette, string introText,
                                IEnumerable<TileDefinition> tiles,
                                IEnumerable<ScreenEnemyDefinition> enemies,
                                IEnumerable<QuestLoaderScriptType> scripts)
        {
            Id = id;
            ScreenName = screenName;

            ScreenType = screenType;
            SongName = songName;
            ColorPalette = colorPalette;
            IntroText = introText;
            Tiles = tiles;

            PlayerStartPositionX = playerStartPositionX;
            PlayerStartPositionY = playerStartPositionY;

            Scripts = scripts ?? new List<QuestLoaderScriptType>();

            Enemies = enemies ?? new List<ScreenEnemyDefinition>();
        }

        public int PlayerStartPositionY { get; private set; }

        public bool Equals(ScreenDefinition other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return Id == other.Id && string.Equals(ScreenName, other.ScreenName) && ScreenType == other.ScreenType &&
                   ScreenPositionY == other.ScreenPositionY && ColorPalette == other.ColorPalette &&
                   string.Equals(SongName, other.SongName) && string.Equals(IntroText, other.IntroText) &&
                   Equals(Tiles, other.Tiles) && ScreenPositionX == other.ScreenPositionX;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((ScreenDefinition) obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                var hashCode = Id;
                hashCode = (hashCode*397) ^ (ScreenName != null ? ScreenName.GetHashCode() : 0);
                hashCode = (hashCode*397) ^ (int) ScreenType;
                hashCode = (hashCode*397) ^ ScreenPositionY;
                hashCode = (hashCode*397) ^ ColorPalette;
                hashCode = (hashCode*397) ^ (SongName != null ? SongName.GetHashCode() : 0);
                hashCode = (hashCode*397) ^ (IntroText != null ? IntroText.GetHashCode() : 0);
                hashCode = (hashCode*397) ^ (Tiles != null ? Tiles.GetHashCode() : 0);
                hashCode = (hashCode*397) ^ ScreenPositionX;
                return hashCode;
            }
        }

        public static bool operator ==(ScreenDefinition left, ScreenDefinition right)
        {
            return Equals(left, right);
        }

        public static bool operator !=(ScreenDefinition left, ScreenDefinition right)
        {
            return !Equals(left, right);
        }
    }
}