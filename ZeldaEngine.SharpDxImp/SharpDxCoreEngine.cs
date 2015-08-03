using SharpDX.Toolkit;
using SharpDX.Toolkit.Graphics;
using ZeldaEngine.Base;
using ZeldaEngine.Base.Abstracts.Game;
using ZeldaEngine.Base.Abstracts.ScriptEngine;
using ZeldaEngine.Base.Game;
using ZeldaEngine.Base.ValueObjects;
using ZeldaEngine.SharpDxImp.DataFormat;
using ZeldaEngine.SharpDxImp.GameEngineClasses;

namespace ZeldaEngine.SharpDxImp
{
    // Use these namespaces here to override SharpDX.Direct3D11
    
    /// <summary>
    /// Simple ZeldaEngineSharpDXTest game using SharpDX.Toolkit.
    /// </summary>
    public class SharpDxCoreEngine : Game, IGameEngine
    {
        private readonly IGame _currentGame;

        private SpriteBatch _spriteBatch;

        public GameConfig GameConfig { get; set; }

        public IInputManager InputManager { get; set; }

        public IRenderEngine RenderEngine { get; set; }

        public IScriptEngine ScriptEngine { get; set; }

        public IAudioEngine AudioEngine { get; set; }

        public IContentLoader ResourceLoader { get; private set; }

        public ILogger Logger { get; private set; }

        public IGameObjectFactory GameObjectFactory { get; set; }

        public SharpDxCoreEngine(IGame game, GameConfig config, ILogger logger)
        {
            Logger = logger;

            var graphicsDeviceManager1 = new GraphicsDeviceManager(this);

            Content.RootDirectory = config.ResourceDirectory;

            GameConfig = config;

            ResourceLoader = new CustomResourceLoader(this);
            GameObjectFactory = new GameObjectFactory(this);

            InputManager = new CustomInputManager(this, ConfigurationManager.GetInputConfiguration());
            
            AudioEngine = new CustomAudioEngine(this);

            game.GameEngine = this;
            _currentGame = game;

            graphicsDeviceManager1.PreferredBackBufferWidth = GameConfig.ScreenWidth;
            graphicsDeviceManager1.PreferredBackBufferHeight = GameConfig.ScreenHeight;

#if DEBUG
            IsMouseVisible = true;
#endif
        }

        public IResourceData Texture2DData(string assetName)
        {
            return new Texture2DResourceData(ResourceLoader.Load<Texture2D>(assetName));
        }

        public void ExitGame()
        {
            Exit();
        }

        protected override void Initialize()
        {
            Window.Title = "ZeldaEngineSharpDXTest";
            Window.AllowUserResizing = false;
            

            base.Initialize();

            Logger.LogInfo("Core Engine Initialized Successfully");
        }

        protected override void LoadContent()
        {
            _spriteBatch = ToDisposeContent(new SpriteBatch(GraphicsDevice));
            RenderEngine = new CustomRenderEngine(this, _spriteBatch, GraphicsDevice);

            _currentGame.Init();

            ScriptEngine.PerformScriptBinding();

            base.LoadContent();
        }

        protected override void Update(GameTime gTime)
        {
            //GameObjectFactory.Update(gameTime.ElapsedGameTime.Milliseconds);

            InputManager.Update();

            _currentGame.HandleInput(gTime.ElapsedGameTime.Milliseconds);

            _currentGame.Update(gTime.ElapsedGameTime.Milliseconds);

            RenderEngine.UpdateRenderGameTime(gTime.ElapsedGameTime.Milliseconds);

            base.Update(gTime);
        }

        protected override void Draw(GameTime gTime)
        {
            GraphicsDevice.Clear(SharpDX.Color.Black);

            _currentGame.Render(RenderEngine);

            base.Draw(gTime);
        }

        protected override void Dispose(bool disposeManagedResources)
        {
            RenderEngine.Dispose();

            AudioEngine.Dispose();

            base.Dispose(disposeManagedResources);
        }
    }
}
