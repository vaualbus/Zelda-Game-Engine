using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using ZeldaEngine.Base.Game;
using ZeldaEngine.Base.ValueObjects;
using ZeldaEngine.Base.ValueObjects.Game;

namespace ZeldaEngine.Base.Abstracts.Game
{
    public interface IGameObject : IDisposable
    {
        string Name { get; set; }

        IGameEngine GameEngine { get; set; }

        Vector2 Position { get; set; }

        Vector2 Rotation { get; set; }

        Vector2 Scaling { get; set; }

        Rectangle CollisionBounds { get; }

        ObjectType ObjectType { get; }

        TComponent GetComponent<TComponent>(string name) where TComponent : class, IGameComponent;

        TComponent AddComponent<TComponent>(string name) where TComponent : class, IGameComponent;

        void Init();

        void Draw(IRenderEngine renderEngine);

        void Update(float dt);
    }
}