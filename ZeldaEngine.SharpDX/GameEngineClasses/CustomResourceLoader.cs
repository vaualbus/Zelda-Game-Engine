using System;
using SharpDX.Toolkit.Graphics;
using ZeldaEngine.Base.Abstracts.Game;

namespace ZeldaEngine.SharpDx.GameEngineClasses
{
    public class CustomResourceLoader : IContentLoader
    {
        private readonly SharpDxCoreEngine _engine;

        public CustomResourceLoader(SharpDxCoreEngine engine)
        {
            _engine = engine;
        }

        public TData Load<TData>(string assetName)
        {
            if(typeof(TData) == typeof(Texture) || typeof(TData) == typeof(Texture2D) || typeof(TData) == typeof(Texture3D))
                return _engine.Content.Load<TData>(@"Textures\" + assetName);
            if(typeof(TData) == typeof(SpriteFont))
                return _engine.Content.Load<TData>(@"Fonts\" + assetName);

            throw new Exception("Invalid Data format to load");
        }

        public void Update()
        {
            throw new Exception("This Engine Not Support Texture Binding like OpenGL");
        }
    }
}