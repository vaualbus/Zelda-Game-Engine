using System.Collections.Generic;

namespace ZeldaEngine.Base.Abstracts.Game
{
    public interface IGameResourceLoader
    {
        string AssetDirectorySuffix { get; }

        string Name { get;  }

        IEnumerable<string> AssetFileExtensions { get;  }

        object LoadObject(string pathName);

        void Bind();
    }
}