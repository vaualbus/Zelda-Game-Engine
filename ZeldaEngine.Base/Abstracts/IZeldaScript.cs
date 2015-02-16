using ZeldaEngine.Base.ValueObjects;

namespace ZeldaEngine.Base.Abstracts
{
    public interface IZeldaScript
    {
        Vector2 Position { get; set; }

        INpc GetNpc(string npcName);

        IItem GetItem(string itemName);

        void ApplicationInit();

        void Init();

        void Run(); 
    }
}