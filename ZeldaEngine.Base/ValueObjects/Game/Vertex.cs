namespace ZeldaEngine.Base.ValueObjects.Game
{
    public struct Vertex
    {
        public Vector2 VertexPosition { get; private set; }

        public object VertexColor { get; private set; }

        public Vertex(Vector2 position, object vertexColor = null) : this()
        {
            VertexPosition = position;
            VertexColor = vertexColor;
        }
    }
}