using SharpDX.Toolkit.Graphics;
using ZeldaEngine.Base.Abstracts.Game;

namespace ZeldaEngine.SharpDx.DataFormat
{
    class Texture2DResourceData : IResourceData
    {
        private readonly Texture2D _texture;

        public int Width => _texture.Width;

        public int Height => _texture.Height;
        public object Value => _texture;

        public Texture2DResourceData(Texture2D texture)
        {
            _texture = texture;
        }
    }
}