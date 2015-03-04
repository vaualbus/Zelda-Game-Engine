using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using OpenTK;
using OpenTK.Graphics.OpenGL;
using OpenTK.Input;
using ZeldaEngine.Base;
using ZeldaEngine.Base.Abstracts.Game;
using ZeldaEngine.Base.Abstracts.ScriptEngine;
using ZeldaEngine.Base.Game;
using ZeldaEngine.Base.Game.GameComponents;
using ZeldaEngine.Base.Game.GameEngineClasses;
using ZeldaEngine.Base.Game.GameObjects;
using ZeldaEngine.Base.Game.MapLoaders;
using ZeldaEngine.ScriptEngine;
using Vector2 = ZeldaEngine.Base.ValueObjects.Vector2;

namespace ZeldaEngine.Game
{
    public class ZeldaGame : IGame
    {
        internal class TestView : BaseGameView
        {
            public TestView(string screenName, Vector2 screenPosition, Vector2 playerStartPosition = null) 
                : base(screenName, screenPosition, playerStartPosition)
            {
            }
        }

        private QuestManager _questManager;

        public IGameEngine GameEngine { get; set; }

        public ZeldaGame()
        {
        }

        public bool Init()
        {
            var scriptEngine = new GameScriptEngine(GameEngine.GameConfig);
            scriptEngine.InitializeEngine();
            GameEngine.ScriptEngine = scriptEngine;

            _questManager = new QuestManager(GameEngine, new JsonMapLoader(GameEngine));
            _questManager.LoadQuest("TestQuest");
  
            return true;
        }

        public void Update(float delta)
        {
            foreach (var view in _questManager.LoadedQuest.Maps.SelectMany(map => map.GameViews))
            {
                GameEngine.ScriptEngine.Update(view, delta);
            }

            _questManager.Update(delta);
        }

        public void Render(IRenderEngine engine)
        {

            // render graphics
            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);
         
            GL.MatrixMode(MatrixMode.Projection);
            GL.LoadIdentity();
            GL.Ortho(-1.0, 1.0, -1.0, 1.0, 0.0, 4.0);

            GL.Begin(PrimitiveType.Triangles);

            GL.Color3(Color.MidnightBlue);
            GL.Vertex2(-1.0f, 1.0f);
            GL.Color3(Color.SpringGreen);
            GL.Vertex2(0.0f, -1.0f);
            GL.Color3(Color.Ivory);
            GL.Vertex2(1.0f, 1.0f);

            GL.End();
            
            _questManager.Draw(engine);
        }

        public void HandleInput(float delta)
        {
            if (GameEngine.InputManager.IsKeyDown(Key.A))
                MessageBox.Show("Test Working", "Test", MessageBoxButtons.OKCancel);
        }

        public void Dispose()
        {
            GameEngine.ScriptEngine.Dispose();
        }
    }
}