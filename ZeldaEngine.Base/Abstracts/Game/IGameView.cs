using System;
using System.Collections.Generic;
using System.Drawing.Imaging;
using ZeldaEngine.Base.Abstracts.ScriptEngine;
using ZeldaEngine.Base.Game.GameObjects;
using ZeldaEngine.Base.ValueObjects;
using ZeldaEngine.Base.ValueObjects.Game;

namespace ZeldaEngine.Base.Abstracts.Game
{
    public interface IGameView : IDisposable
    {
        int ScreenId { get; set; }

        Vector2 PlayerStartPosition { get; }

        string Name { get; }

        ColorPalette ColorPalette { get; set; }

        WarpPoint[] WarpPoints { get; set; }

        List<IGameObject> GameObjects { get; }

        List<EnemyGameObject> Enemies { get; }

        List<DrawableGameObject> DrawableGameObjects { get; }

        List<ScriptableGameObject> Scripts { get; }

        void Draw(IRenderEngine renderEngine);

        void Update(float dt);
    }
}