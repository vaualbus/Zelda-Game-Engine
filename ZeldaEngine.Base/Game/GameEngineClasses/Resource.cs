using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using ZeldaEngine.Base.Abstracts.Game;

namespace ZeldaEngine.Base.Game.GameEngineClasses
{
    public class Resource : IContentLoader
    {
        private static Resource Instance
        {
            get { return _instance; }
        }

        public static Dictionary<IGameResourceLoader, string> CachedResources { get; private set; } 

        private readonly IGameEngine _engine;

        private static Resource _instance;

       

        public Resource(IGameEngine engine)
        {
            _engine = engine;
            _instance = this;

            if(CachedResources == null)
                CachedResources = new Dictionary<IGameResourceLoader, string>();
        }

        public TData Load<TData>(string assetName)
        {
            var resourcePath = _instance._engine.Configuration.GameConfig.ResourceDirectory;
            var resourceLoader = (IGameResourceLoader) typeof (TData).GetConstructors()[0].Invoke(null);

            if (CachedResources.ContainsValue(assetName) && CachedResources.ContainsKey(resourceLoader))
            {
                return (TData) CachedResources.Where(t => t.Value == assetName).Select(t => t.Key).FirstOrDefault();
            }

            try
                {
                var fileInDir = Directory.GetFiles(Path.Combine(resourcePath, resourceLoader.AssetDirectorySuffix)).Select(t => new FileInfo(t));
                var fileExtension = fileInDir.FirstOrDefault(t => t.Name.Split('.')[0] == assetName).Extension.Replace(".", string.Empty);

                if (!resourceLoader.AssetFileExtensions.Contains(fileExtension))
                    throw new InvalidOperationException(
                        string.Format("File {0} extension is not supported by the resource loader", assetName));

                var assetPath = Path.Combine(resourcePath, resourceLoader.AssetDirectorySuffix,
                    string.Format("{0}.{1}", assetName, fileExtension));


                CachedResources.Add(resourceLoader, assetName);

                return (TData) resourceLoader.LoadObject(Path.Combine(_instance._engine.Configuration.GameConfig.BaseDirectory, assetPath));
            }
            catch (Exception ex)
            {
                throw new Exception(string.Format("Cannot load the asset with the resource loader: {0}", resourceLoader.Name));
            }

        }

        public void Update()
        {
            foreach (var resource in CachedResources)
            {
                resource.Key.Bind();
            }
        }
    }
}