using System.Collections.Generic;

namespace ZeldaEngine.Base.Game.ValueObjects
{
    public class MapDescription
    {
        public string Name { get; private set; }

        public int StartPositionX { get; private set; }

        public int StartPositionY { get; private set; }

        public string SubScreenName { get; private set; }

        public IEnumerable<ScreenDefinition> Screens { get; private set; }

        public MapDescription(string name, int startPositionX, int startPositionY, string subScreenName,
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