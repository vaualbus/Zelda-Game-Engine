using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZeldaEngine.GameTest.Abstracts
{
    public interface IGameObjectFactory
    {
        TComponent Find<TComponent>() where TComponent : class, IGameComponent;

        void AddComponent<TComponent>(string name = "") where TComponent : class, IGameComponent;

        void Remove<TComponent>() where TComponent : class, IGameComponent;

        void Remove(string name);
    }
}
