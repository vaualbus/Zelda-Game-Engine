using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using ZeldaEngine.Base.Abstracts.Game;
using ZeldaEngine.Base.Game.Abstracts;
using ZeldaEngine.Base.Game.GameComponents;
using ZeldaEngine.Base.Game.GameEngineClasses;
using ZeldaEngine.Base.ValueObjects;
using ZeldaEngine.Base.ValueObjects.Game;

namespace ZeldaEngine.Base.Game.GameObjects
{
    public class GameObject : IGameObject
    {
        private static long _nullGOCount;

        private readonly Dictionary<string, IGameComponent> _gameComponents;

        public string Name { get; set; }

        public Vector2 Position { get; set; }

        public Vector2 Rotation { get; set; }

        public Vector2 Scaling { get; set; }

        public Rectangle CollisionBounds { get; protected set; }

        public virtual ObjectType ObjectType { get { return ObjectType.Null; } }

        public ScriptTuple ScriptTuple { get; private set; }

        public List<IGameObject> ParentGameObjects { get; protected set; }

        public GameObject(IGameEngine gameEngine)
        {
            _gameComponents = new Dictionary<string, IGameComponent>();
            ScriptTuple = new ScriptTuple();
            Position = new Vector2(0,0);

            //Add all the default Component
            AddComponent<ColliderComponent>("basicCollider");

            GameEngine = gameEngine;

            Name = "Default_GameObject_" + _nullGOCount++;
            ParentGameObjects = new List<IGameObject>();
        }

        public IGameEngine GameEngine { get; set; }

        public TComponent GetComponent<TComponent>(string name) where TComponent : class, IGameComponent
        {
            return (TComponent) _gameComponents[name];
        }

        private TComponent CreateComponent<TComponent>() where TComponent : class, IGameComponent
        {
            try
            {
                var tComponent = Activator.CreateInstance<TComponent>();
                tComponent.GameEngine = GameEngine;

                return tComponent;
            }
            catch
            {
                GameEngine.Logger.LogError("The given component has not a default constructur. Components must not have constructures. (For now)");
            }

            return null;
        }

        public void RemoveComponent(string name)
        {
            _gameComponents.Remove(name);
        }

        public TComponent AddComponent<TComponent>(string componentName) where TComponent : class, IGameComponent
        {
            if (_gameComponents.ContainsKey(componentName))
                GameEngine.Logger.LogError("Adding component {0} to {1} more than one", componentName, Name);

            var tComponent = CreateComponent<TComponent>();
            tComponent.Name = componentName;

            _gameComponents.Add(componentName, tComponent);

            return tComponent;
        }

        public void Draw(IRenderEngine renderEngine)
        {
            OnDraw(renderEngine);
        }

        public void Update(float dt)
        {
            foreach (var parentGameComponent in _gameComponents)
            {
                if (parentGameComponent.Value.GetType() == typeof (DrawableGameObject))
                    continue;

                parentGameComponent.Value.Action(this);
            }

            OnUpdate(dt);
        }

        public void Init()
        {
            OnInit();
        }

        protected virtual void OnInit() {  }

        protected virtual void OnUpdate(float dt) { }

        protected virtual void OnDraw(IRenderEngine renderEngine) { }

        public void Dispose()
        {
            _gameComponents.Clear();
        }
    }
}