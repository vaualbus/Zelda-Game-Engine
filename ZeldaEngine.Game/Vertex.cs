using System.Runtime.InteropServices;
using OpenTK;
using Vector2 = ZeldaEngine.Base.ValueObjects.Vector2;

namespace ZeldaEngine.Game
{
    public struct Vertex
    {
        public static int VertexSize = Marshal.SizeOf(default(Vertex));

        public Vector2 Position { get; private set; }

        public Vector2 TextureCoords { get; private set; }

        public Vertex(Vector2 position)
            : this()
        {
            Position = position;
        }

        public Vertex(Vector2 position, Vector2 texturePos)
            : this()
        {
            Position = position;
            TextureCoords = texturePos;
        }
    }
}