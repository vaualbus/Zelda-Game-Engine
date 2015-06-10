using System.Drawing;
using ZeldaEngine.Base.Abstracts.Game;
using ZeldaEngine.Base.Game;
using ZeldaEngine.Base.Game.GameObjects;


namespace ZeldaEngine.Base.ValueObjects.Game
{
    public class Tile
    {
        public IGameEngine GameEngine { get; set; }
        public int Width { get; set; }

        public int Height { get; set; }

        public Color Color { get; set; }

        public bool IsTransparent { get; set; }

        public int LayerNumber { get; set; }

        GameScript GameScript { get; set; }

        public IResourceData Texture { get; set; }

        public Tile(IGameEngine gameEngine, int width, int height, int layer = 0)
        {
            GameEngine = gameEngine;
            Width = width;
            Height = height;
            LayerNumber = layer;

            Texture = GameEngine.Texture2DData("Default");
        }

        public void Draw(IRenderEngine renderEngine)
        {
        }
    }
}