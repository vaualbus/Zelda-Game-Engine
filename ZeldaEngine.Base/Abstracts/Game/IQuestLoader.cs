using ZeldaEngine.Base.Game.ValueObjects;

namespace ZeldaEngine.Base.Abstracts.Game
{
    public interface IQuestLoader
    {
        QuestDefinition Load(string fileName);
    }
}