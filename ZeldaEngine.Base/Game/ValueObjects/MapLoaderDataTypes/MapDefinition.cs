using System.Collections.Generic;

namespace ZeldaEngine.Base.Game.ValueObjects.MapLoaderDataTypes
{
    public class MapDefinition
    {
        public string Name { get; private set; }

        public float StartPositionX { get; private set; }

        public float StartPositionY { get; private set; }

        public string SubScreenName { get; private set; }

        public IEnumerable<ScreenDefinition> Screens { get; private set; }

        public MapDefinition(string name, float startPositionX, float startPositionY, string subScreenName,
            IEnumerable<ScreenDefinition> screens)
        {
            Name = name;
            StartPositionX = startPositionX;
            StartPositionY = startPositionY;
            SubScreenName = subScreenName;
            Screens = screens;
        }
    }
}