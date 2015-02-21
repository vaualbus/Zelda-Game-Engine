using System.Collections.Generic;
using System.Drawing;
using OpenTK.Graphics.OpenGL;
using ZeldaEngine.Base.Abstracts.Game;
using ZeldaEngine.Base.Game;
using ZeldaEngine.Base.Game.GameObjects;
using ZeldaEngine.Base.ValueObjects;
using ZeldaEngine.Base.ValueObjects.Game;
using ZeldaEngine.OpenGL.GameEngineClasses.ResourceLoader;

namespace ZeldaEngine.OpenGL.GameEngineClasses
{
    public class OpenGLRenderEngine : BaseGameClass, IRenderEngine
    {
        public OpenGLRenderEngine(IGameEngine gameEngine)
            : base(gameEngine)
        {
            GameEngine.Configuration.GameConfig.OpenGLVersion = GL.GetString(StringName.Version);
            GameEngine.Logger.LogInfo("Render Engine  Initialized");
        }

        public void Render(IGameObject gameObject)
        {

        }

        public void ApplyTexture(DrawableGameObject gameObject, IResourceData texture)
        {
        }

        public void UpdateRenderGameTime(int milliseconds)
        {
        }

        public void DrawCircle(Vector2 position, int radius, object fillColor)
        {
            throw new System.NotImplementedException();
        }

        public void DrawLine(Vector2 start, Vector2 end, object lineColor, int thickness)
        {
            throw new System.NotImplementedException();
        }

        public void DrawLine(int x0, int y0, int x1, int y1, object lineColor, int thickness)
        {
            throw new System.NotImplementedException();
        }

        public void DrawLine(Vector2 start, int lenght, float rotation, object lineColor, int thinckness)
        {
            throw new System.NotImplementedException();
        }

        public void DrawLine(int x0, int y0, int lenght, float rotation, object lineColor, int thickness)
        {
            throw new System.NotImplementedException();
        }

        public void DrawFillCircle(Vector2 position, int radious, object fillColor)
        {
            throw new System.NotImplementedException();
        }

        public void DrawTriangle(Vector2 position, Vertex[] verticies, object lineColor, int thickness)
        {
            throw new System.NotImplementedException();
        }

        public void DrawFillTriangle(Vector2 position, Vertex[] verticies)
        {
            throw new System.NotImplementedException();
        }

        public void DrawBox(Vector2 position, int width, object color)
        {
            throw new System.NotImplementedException();
        }

        public void DrawCollisionLines(GameObject go, IEnumerable<GameObject> nearestObjects)
        {
            throw new System.NotImplementedException();
        }
    }
}