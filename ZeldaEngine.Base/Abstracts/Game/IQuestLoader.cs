using ZeldaEngine.Base.ValueObjects.MapLoaderDataTypes;

namespace ZeldaEngine.Base.Abstracts.Game
{
    public interface IQuestLoader
    {
        QuestDefinition Load(string fileName);
    }
}