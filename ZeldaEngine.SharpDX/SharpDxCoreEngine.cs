using SharpDX.Toolkit;
using SharpDX.Toolkit.Graphics;
using ZeldaEngine.Base;
using ZeldaEngine.Base.Abstracts.Game;
using ZeldaEngine.Base.Abstracts.ScriptEngine;
using ZeldaEngine.Base.Game;
using ZeldaEngine.Base.Game.GameObjects;
using ZeldaEngine.Base.ValueObjects;
using ZeldaEngine.SharpDx.DataFormat;
using ZeldaEngine.SharpDx.GameEngineClasses;

namespace ZeldaEngine.SharpDx
{
    // Use these namespaces here to override SharpDX.Direct3D11
    
    /// <summary>
    /// Simple ZeldaEngineSharpDXTest game using SharpDX.Toolkit.
    /// </summary>
    public class SharpDxCoreEngine : Game, IGameEngine
    {
        private readonly IGame _currentGame;

        private readonly GraphicsDeviceManager _graphicsDeviceManager;

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

            _graphicsDeviceManager = new GraphicsDeviceManager(this);

            Content.RootDirectory = config.ResourceDirectory;

            GameConfig = config;

            ResourceLoader = new CustomResourceLoader(this);
            GameObjectFactory = new GameObjectFactory(this);

            InputManager = new CustomInputManager(this, ConfigurationManager.GetInputConfiguration());
            
            AudioEngine = new CustomAudioEngine(this);

            game.GameEngine = this;
            _currentGame = game;

            _graphicsDeviceManager.PreferredBackBufferWidth = GameConfig.ScreenWidth;
            _graphicsDeviceManager.PreferredBackBufferHeight = GameConfig.ScreenHeight;

#if DEBUG
            IsMouseVisible = true;
#endif
        }

        public IResourceData Texture2DData(string assetName)
        {
            return new Texture2DResourceData(ResourceLoader.Load<Texture2D>(assetName));
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

            base.LoadContent();
        }

        protected override void Update(GameTime gameTime)
        {
            //((CustomInputManager) InputManager).Update();

            //GameObjectFactory.Update(gameTime.ElapsedGameTime.Milliseconds);

            _currentGame.HandleInput(gameTime.ElapsedGameTime.Milliseconds);
            _currentGame.Update(gameTime.ElapsedGameTime.Milliseconds);

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(SharpDX.Color.Black);

            RenderEngine.UpdateRenderGameTime(gameTime.ElapsedGameTime.Milliseconds);

            _currentGame.Render(RenderEngine);

            base.Draw(gameTime);
        }

        protected override void Dispose(bool disposeManagedResources)
        {
            RenderEngine.Dispose();
            AudioEngine.Dispose();

            base.Dispose(disposeManagedResources);
        }
    }
}
