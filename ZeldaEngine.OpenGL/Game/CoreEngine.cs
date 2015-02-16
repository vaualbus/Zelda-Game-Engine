using System;
using System.Windows.Forms;
using OpenTK;
using OpenTK.Graphics.OpenGL;
using ZeldaEngine.Base;
using ZeldaEngine.Base.Game;
using ZeldaEngine.Base.Game.Abstracts;
using ZeldaEngine.Base.Game.GameEngineClasses;
using ZeldaEngine.Base.Game.GameObjects;
using ZeldaEngine.Base.ValueObjects;
using ZeldaEngine.OpenGL.GameEngineClasses;

namespace ZeldaEngine.OpenGL.Game
{
        public class CoreEngine : GameEngine, IDisposable
        {
            private readonly GameWindow _screen;
            private readonly IGame _currentGame;

            public CoreEngine(IGame game, Config config)
                : base(new GameLogger(config))
            {
                _screen = new GameWindow();
                _screen.Load += IntializeEvent;
                _screen.RenderFrame += RenderEvent;
                _screen.UpdateFrame += UpdateEvent;
                _screen.Resize += (sender, args) => GL.Viewport(0, 0, _screen.Width, _screen.Height);

                //game.GameWindow = _screen;
                _currentGame = game;


                _screen.Title = config.GameConfig.Title;
                _screen.Width = config.GameConfig.ScreenWidth;
                _screen.Height = config.GameConfig.ScreenHeight;

                Configuration = config;

                this.ResourceLoader = new Resource(this);
                new GameObjectFactory(this);


                InputManager = new OpenTKInputManager(this, ConfigurationManager.GetInputConfiguration());
                RenderEngine = new OpenGLRenderEngine(this);
                
                AudioEngine = new AudioEngine(this);

                game.GameEngine = this;

                Logger.LogInfo("Core Engine Initialized Successfully");
            }

            public void CreateWindow()
            {
                _screen.Title = string.Format("OpenGL Version: {0}", Configuration.GameConfig.OpenGLVersion);
            }

            public void Start()
            {
                _screen.Run(Configuration.GameConfig.Framerate);
            }

            private void IntializeEvent(object sender, EventArgs e)
            {
                //_screen.X = Configuration.GameConfig.ScreenPosition.X;
                //_screen.Y = Configuration.GameConfig.ScreenPosition.Y;

                if (!_currentGame.Init())
                {
                    MessageBox.Show("Error Initializing the engine", "Error", MessageBoxButtons.OK);
                    _screen.Close();
                }
            }

            private void UpdateEvent(object sender, FrameEventArgs e)
            {
                // GameInput.Update(_screen);
                ResourceLoader.Update();

                _currentGame.HandleInput((float)e.Time);
                _currentGame.Update((float)e.Time);
            }

            //Render Method
            private void RenderEvent(object sender, FrameEventArgs e)
            {
                _currentGame.Render(RenderEngine);
                _screen.SwapBuffers();
            }

            public void Dispose()
            {
                _currentGame.Dispose();
                _screen.Dispose();
            }
        }
}