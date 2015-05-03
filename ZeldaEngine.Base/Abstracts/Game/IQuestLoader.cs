using ZeldaEngine.Base.Game.ValueObjects;
using ZeldaEngine.Base.Game.ValueObjects.MapLoaderDataTypes;

namespace ZeldaEngine.Base.Abstracts.Game
{
    public interface IQuestLoader
    {
        QuestDefinition Load(string fileName);
    }
}