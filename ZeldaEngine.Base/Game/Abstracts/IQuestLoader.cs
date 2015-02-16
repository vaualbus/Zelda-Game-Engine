using ZeldaEngine.Base.Game.ValueObjects;

namespace ZeldaEngine.Base.Game.Abstracts
{
    public interface IQuestLoader
    {
        QuestDefinition Load(string fileName);
    }
}