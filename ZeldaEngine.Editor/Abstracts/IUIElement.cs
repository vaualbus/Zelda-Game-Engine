using SharpDX;
using ZeldaEngine.Base.Abstracts.Game;
using ZeldaEngine.SharpDxImp.DataFormat;

namespace ZeldaEngine.Game.Abstracts
{
    public interface IUIElement
    {
        string Name { get; }

        string Content { get; }

        Vector3 Position { get; }

        Vector3 Rotation { get; }

        Vector3 Scale { get; }

        bool IsActive { get; }

        Color Color { get;  }

        IResourceData Background { get; }

        void AddParentElement(IUIElement element);

        void AddChildrenElement(IUIElement element);

        void Draw(IRenderEngine renderEngine);

        void Update(float dt);
    }
}