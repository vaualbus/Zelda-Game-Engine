using System;
using System.Collections.Generic;
using System.Linq;
using ZeldaEngine.Base.Abstracts.Game;
using ZeldaEngine.Base.Game.ValueObjects;
using ZeldaEngine.Base.ValueObjects;

namespace ZeldaEngine.Base.Game.GameObjects
{
    public class GameObjectFactory
    {
        private static IGameEngine _gameEngine;
        private static Dictionary<Tuple<Type, string>, IGameObject> _registeredGameObjects;

        public static IEnumerable<IGameObject> RegisteredGameObjects => _registeredGameObjects.Values;

        public GameObjectFactory(IGameEngine gameEngine)
        {
            _gameEngine = gameEngine;
            _registeredGameObjects = new Dictionary<Tuple<Type, string>, IGameObject>();
        }

        public static TObject Create<TObject>(string name, params object[] ctorParams) where TObject : class, IGameObject
        {
            if (_registeredGameObjects.Select(t => t.Value).Any(t => t.Name == name))
            {
                _gameEngine.Logger.LogError("Trying adding a game object with the same name");
                return default(TObject);
            }

            var @params = new List<object> {_gameEngine};
            @params.AddRange(ctorParams);

            var temp = (TObject) Activator.CreateInstance(typeof (TObject), @params.ToArray());
            temp.Name = name;
            temp.GameEngine = _gameEngine;

            _registeredGameObjects.Add(Tuple.Create(typeof(TObject), temp.Name), temp);

            return temp;
        }

        public static TObject Create<TObject>(string name, Action<TObject> func) where TObject : class, IGameObject
        {
            if (_registeredGameObjects.Select(t => t.Value).Any(t => t.Name == name))
            {
                _gameEngine.Logger.LogError("Trying adding a game object with the same name");
                return default(TObject);
            }

            var temp = (TObject)Activator.CreateInstance(typeof(TObject), _gameEngine);
            temp.Name = name;
            func(temp);

            _registeredGameObjects.Add(Tuple.Create(typeof(TObject), temp.Name), temp);

            return temp;
        }

        public static void Delete(string name)
        {
            var go = _registeredGameObjects.Select(t => t.Value).FirstOrDefault(t => t.Name == name);
            if (go == null)
            {
                _gameEngine.Logger.LogError("Trying to delete {0} but game object with the given name has not been found", name);
                return;
            }
            _registeredGameObjects.Remove(Tuple.Create(go.GetType(), go.Name));
        }

        public static IGameObject Find(string name)
        {
            return _registeredGameObjects.Select(t => t.Value).FirstOrDefault(t => t.Name == name);
        }

        public static IEnumerable<TObject> Find<TObject>() where TObject : class, IGameObject
        {
            return _registeredGameObjects.Where(t => t.Key.Item1 == typeof (TObject)).Select(t => t.Value).Cast<TObject>();
        }

        public static IEnumerable<TileGameObject> FindNearGameObject(Vector2 distance)
        {
            return Find<TileGameObject>().Where(t => t.Position.IsInRange(distance));
        }

        public static void Delete<TObject>()
        {
            var gos = _registeredGameObjects.Where(t => t.Key.Item1 == typeof(TObject)).Select(t => t.Value).ToList();
            foreach (var go in gos.Where(t => t != null))
                _registeredGameObjects.Remove(Tuple.Create(go.GetType(), go.Name));
        }

        public static void Draw(IRenderEngine renderEngine)
        {
            foreach (var registeredGameObject in _registeredGameObjects)
                registeredGameObject.Value.Draw(renderEngine);
        }

        public static void Update(float dt)
        {
            foreach (var registeredGameObject in _registeredGameObjects)
                registeredGameObject.Value.Update(dt);
        }

        public static IGameObject GetFromDefinition(GameObjectDefinition gameObject)
        {
            return Find(gameObject.Name);
        }

        public static void UpdateGameObject(IGameObject enemy)
        {
            var go = Find(enemy.Name);
            _registeredGameObjects[Tuple.Create(enemy.GetType(), go.Name)] = enemy;

        }
    }
}