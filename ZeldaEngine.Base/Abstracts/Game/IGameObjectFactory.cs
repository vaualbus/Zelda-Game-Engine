using System;
using System.Collections.Generic;
using ZeldaEngine.Base.Game.GameObjects;
using ZeldaEngine.Base.Game.ValueObjects;
using ZeldaEngine.Base.Game.ValueObjects.MapLoaderDataTypes;
using ZeldaEngine.Base.ValueObjects;

namespace ZeldaEngine.Base.Abstracts.Game
{
    public interface IGameObjectFactory
    {
        TObject Create<TObject>(string name, params object[] ctorParams) where TObject : class, IGameObject;

        TObject Create<TObject>(string name, Action<TObject> func) where TObject : class, IGameObject;

        TObject CreateEmpty<TObject>(string name) where TObject : class, IGameObject;

        void Delete(string name);

        IGameObject Find(string name);

        TObject Find<TObject>(string name) where TObject : class, IGameObject;

        IEnumerable<TObject> Find<TObject>() where TObject : class, IGameObject;

        IEnumerable<TileGameObject> FindNearGameObject(Vector2 distance);

        IGameObject GetFromDefinition(GameObjectDefinition gameObject);

        void TryGet<TGameObject>(string name, out TGameObject go) where TGameObject : class, IGameObject;

        void UpdateGameObject(IGameObject enemy);
    }
}