using System;
using SharpDX.Toolkit.Graphics;
using ZeldaEngine.Base.Abstracts.Game;

namespace ZeldaEngine.SharpDx.GameEngineClasses
{
    public class CustomResourceLoader : IContentLoader
    {
        private SharpDxCoreEngine _engine;

        private static CustomResourceLoader _instance;

        private static CustomResourceLoader Instance
        {
            get { return _instance; }
        }


        public CustomResourceLoader(SharpDxCoreEngine engine)
        {
            _engine = engine;
            _instance = this;
        }

        public TData Load<TData>(string assetName)
        {
            if(typeof(TData) == typeof(Texture) || typeof(TData) == typeof(Texture2D) || typeof(TData) == typeof(Texture3D))
                return _instance._engine.Content.Load<TData>(@"Textures\" + assetName);
            if(typeof(TData) == typeof(SpriteFont))
                return _instance._engine.Content.Load<TData>(@"Fonts\" + assetName);

            throw new Exception("Invalid Data format to load");
        }

        public void Update()
        {
            throw new Exception("This Engine Not Support Texture Binding like OpenGL");
        }
    }
}