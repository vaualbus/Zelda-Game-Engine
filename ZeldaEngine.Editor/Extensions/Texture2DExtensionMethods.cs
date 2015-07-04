using SharpDX;
using SharpDX.Toolkit.Graphics;

namespace ZeldaEngine.Game.Extensions
{
    public static class Texture2DExtensionMethods
    {
        public static Texture2D ApplyColor(this Texture2D that, Color color)
        {
            var colors = new Color[that.Width * that.Height];
            for (int i = 0; i < colors.Length; i++)
                colors[i] = Color.White;

            that.SetData(colors);

            return that;
        }
    }
}