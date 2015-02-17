using System.Collections.Generic;
using System.Drawing;
using OpenTK.Graphics.OpenGL;
using ZeldaEngine.Base.Abstracts.Game;
using ZeldaEngine.Base.Game;
using ZeldaEngine.Base.Game.GameObjects;
using ZeldaEngine.Base.ValueObjects;
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

        public void DrawCircle(Vector2 position, float radious, Color fillColor)
        {
            throw new System.NotImplementedException();
        }

        public void DrawTriangle(Vector2 position, float radious, Color fillColor)
        {
            throw new System.NotImplementedException();
        }

        public void DrawBox(Vector2 position, int width, Color color)
        {
            throw new System.NotImplementedException();
        }

        public void DrawCollisionLines(GameObject go, IEnumerable<GameObject> nearestObjects)
        {
            throw new System.NotImplementedException();
        }
    }
}