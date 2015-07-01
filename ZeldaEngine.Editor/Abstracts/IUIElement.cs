using System;
using SharpDX;
using ZeldaEngine.Base.Abstracts.Game;
using ZeldaEngine.SharpDxImp.DataFormat;
using Vector2 = ZeldaEngine.Base.ValueObjects.Vector2;

namespace ZeldaEngine.Game.Abstracts
{
    public interface IUIElement
    {
        IGameEngine GameEngine { get; set; }

        string Name { get; set; }

        string Content { get; set; }

        int Width { get; set; }

        int Height { get; set; }

        Vector2 Position { get; set; }

        Vector2 Rotation { get; set; }

        Vector2 Scale { get; set; }

        bool IsActive { get; set; }

        Color Color { get; set; }

        IResourceData Background { get; set; }

        IUIElement AddParentElement(IUIElement element);

        IUIElement AddChildrenElement(IUIElement element);

        void AddListener(Action action);

        void Draw(IRenderEngine renderEngine);

        void Update(float dt);
        void ReAdjustUiElements(Vector2 newPosition);
    }
}